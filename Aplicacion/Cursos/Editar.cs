using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ErrorHandler;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest{
            public Guid CursoId{get;set;}
            public string Titulo{get;set;}
            public string Descripcion{get;set;}
            public DateTime? FechaPublicacion{get;set;} //para permitir nulos ?
            public List<Guid> ListaInstructor {get;set;}
            public decimal? Precio {get;set;}
            public decimal? Promocion {get;set;}
        }

        public class EjecutaValidation : AbstractValidator<Ejecuta>{
            public EjecutaValidation(){
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;
            public Handler(CursosOnlineContext context){
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso.FindAsync(request.CursoId);
                if(curso == null){
                    throw new ExceptionHandler(HttpStatusCode.NotFound,new {curso = "No se encontro el curso a editar"});
                }
                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;
                curso.FechaCreacion = DateTime.UtcNow;

                //procedimiento para editar precio
                var precioEntidad = _context.Precio.Where(x => x.CursoId == curso.CursoId).FirstOrDefault();
                if(precioEntidad != null){
                    precioEntidad.Promocion = request.Promocion ?? precioEntidad.Promocion;
                    precioEntidad.PrecioActual = request.Precio ?? precioEntidad.PrecioActual;
                }
                else{
                    precioEntidad = new Precio{
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.Precio ?? 0,
                        Promocion = request.Promocion ?? 0,
                        CursoId = request.CursoId
                    };
                    await _context.Precio.AddAsync(precioEntidad);
                }



                if(request.ListaInstructor != null){
                    if(request.ListaInstructor.Count >0){
                        //eliminar instructores actuales en la bd
                        var instructoresBD = _context.CursoInstructor.Where(x => x.CursoId == request.CursoId).ToList();
                        foreach (var instructorEliminar in instructoresBD)
                        {
                            _context.CursoInstructor.Remove(instructorEliminar);
                        }

                        //Procedimiento para agregar instructores que provienen del cliente
                        foreach (var id in request.ListaInstructor)
                        {
                            var nuevoInstructor = new CursoInstructor{
                                CursoId = request.CursoId,
                                InstructorId = id
                            };
                            _context.CursoInstructor.Add(nuevoInstructor);
                        }

                    }
                }

                var resultado = await _context.SaveChangesAsync();

                if(resultado>0){
                    return Unit.Value;
                }

                throw new Exception("No se guardaron los cambios en el curso");
            }
        }
    }
}
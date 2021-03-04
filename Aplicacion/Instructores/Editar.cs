using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Editar
    {
        public class Ejecuta : IRequest {
            public Guid InstructorId {get;set;}
            public string Nombre {get;set;}
            public string Apellidos {get;set;}
            public string Grado {get;set;}
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>{
            public EjecutaValidacion(){
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Grado).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor _instructorRepositorio;
            public Handler(IInstructor instructorRepositorio){
                _instructorRepositorio = instructorRepositorio;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var resultado = await _instructorRepositorio.Actualizar(request.InstructorId,request.Nombre,request.Apellidos,request.Grado);
                if(resultado > 0){
                    return Unit.Value;
                }

                throw new Exception("No se puedo actualizar la data del instructor");
            }
        }
    }
}
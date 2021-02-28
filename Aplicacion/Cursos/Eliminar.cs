using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ErrorHandler;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest{
            public Guid Id{get;set;}
        }
        public class Handler : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;
            public Handler(CursosOnlineContext context){
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //eliminar las relaciones antes
                var instructoresDB = _context.CursoInstructor.Where(x=>x.CursoId == request.Id);
                foreach (var instructor in instructoresDB)
                {
                    _context.CursoInstructor.Remove(instructor);
                }
                //eliminar comentarion
                var comentarioDB = _context.Comentario.Where(x => x.CursoId == request.Id);
                foreach (var comentario in comentarioDB)
                {
                    _context.Comentario.Remove(comentario);
                }
                //eliminar precio
                var precioDB = _context.Precio.Where(x => x.CursoId == request.Id).FirstOrDefault();
                if(precioDB != null){
                    _context.Precio.Remove(precioDB);
                }

                var curso = await _context.Curso.FindAsync(request.Id);
                if(curso == null){
                    //throw new Exception("No se puede eliminar el curso");
                    throw new ExceptionHandler(HttpStatusCode.NotFound,new {curso = "No se encontro el curso"});
                }

                _context.Remove(curso);

                var resultado = await _context.SaveChangesAsync();
                if(resultado > 0){
                    return Unit.Value;
                }

                throw new Exception("No se pudiero guardar los cambios");
            }
        }
    }
}
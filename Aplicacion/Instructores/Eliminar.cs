using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Eliminar
    {
        public class Ejecuta : IRequest{
            public Guid Id {get;set;}
        }
        public class Handler : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor _instructorRepositorio;
            public Handler(IInstructor instructorRepositorio){
                _instructorRepositorio = instructorRepositorio;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var resultado = await _instructorRepositorio.Eliminar(request.Id);
                if(resultado > 0){
                    return Unit.Value;
                }
                throw new Exception("No se pudo eliminar");
            }
        }
    }
}
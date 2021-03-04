using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ErrorHandler;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class ConsultaId
    {
        public class Ejecuta : IRequest<InstructorModel>{
            public Guid Id {get;set;}
        }
        public class Handler : IRequestHandler<Ejecuta,InstructorModel>
        {
            private readonly IInstructor _instructorRepositorio;
            public Handler(IInstructor instructorRepositorio){
                _instructorRepositorio = instructorRepositorio;
            }

            public async Task<InstructorModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var instructor = await _instructorRepositorio.ObtenerPorId(request.Id);
                if(instructor == null){
                    throw new ExceptionHandler(HttpStatusCode.NotFound,new {mensaje = "No se encontro el instructor"});
                }
                return instructor;
            }
        }
    }
}
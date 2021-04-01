using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ErrorHandler;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Documentos
{
    public class ObtenerArchivo
    {
        public class Ejecuta:IRequest<ArchivoGenerico>{
            public Guid Id {get;set;}
        }

        public class Handler : IRequestHandler<Ejecuta, ArchivoGenerico>
        {
            private readonly CursosOnlineContext _context;
            public Handler(CursosOnlineContext context){
                _context = context;
            }
            public async Task<ArchivoGenerico> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var archivo = await _context.Documento.Where(x => x.ObjectoReferencia == request.Id).FirstAsync();
                if(archivo == null){
                    throw new ExceptionHandler(HttpStatusCode.NotFound,new{mensaje = "No se encontro la imagen"});
                }

                var archivoGenerico = new ArchivoGenerico{
                    Data = Convert.ToBase64String(archivo.Contenido),
                    Extension = archivo.Extension,
                    Nombre = archivo.Nombre
                };
                
                return archivoGenerico;
            }
        }
    }
}
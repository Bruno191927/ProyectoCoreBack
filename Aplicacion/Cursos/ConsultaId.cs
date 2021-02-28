using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ErrorHandler;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<CursoDto>{
            public Guid Id {get;set;}
        }

        public class Handler : IRequestHandler<CursoUnico, CursoDto>{
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;
            public Handler(CursosOnlineContext context,IMapper mapper){
                _context = context;
                _mapper = mapper;
            }
            public async Task<CursoDto> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso
                .Include(x => x.ComentarioLista)
                .Include(x => x.PrecioPromocion)
                .Include(x => x.InstructoresLink)
                .ThenInclude(y => y.Instructor)
                .FirstOrDefaultAsync(a => a.CursoId == request.Id);
                
                


                if(curso == null){
                    throw new ExceptionHandler(HttpStatusCode.NotFound,new {curso = "No se encontro el curso que buscabas"});
                }
                var cursoDto = _mapper.Map<Curso,CursoDto>(curso);
                return cursoDto;
            }
        }
    }
}
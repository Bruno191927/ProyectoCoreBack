using MediatR;
using FluentValidation;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using Aplicacion.ErrorHandler;
using System.Net;
using System;

namespace Aplicacion.Seguridad
{
    public class RolNuevo
    {
        public class Ejecuta : IRequest{
            public string Nombre {get;set;}
        }

        public class ValidaEjecuta : AbstractValidator<Ejecuta>{
            public ValidaEjecuta(){
                RuleFor(x => x.Nombre).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Ejecuta>
        {
            //data que almacena el rol
            private readonly RoleManager<IdentityRole> _roleManager;
            public Handler(RoleManager<IdentityRole> roleManager){
                _roleManager = roleManager;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByNameAsync(request.Nombre);
                if(role != null){
                    throw new ExceptionHandler(HttpStatusCode.BadRequest,new {mensaje = "Ya existe el rol"});
                }

                var resultado = await _roleManager.CreateAsync(new IdentityRole(request.Nombre));

                if(resultado.Succeeded){
                    return Unit.Value;
                }

                throw new Exception("No se pudo crear el rol");
            }
        }
    }
}
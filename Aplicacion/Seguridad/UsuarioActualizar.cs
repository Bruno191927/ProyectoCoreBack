using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ErrorHandler;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class UsuarioActualizar
    {
        public class Ejecuta : IRequest<UsuarioData>{
            public string Nombre {get;set;}
            public string Apellidos {get;set;}
            public string Email {get;set;}
            public string Password {get;set;}
            public string Username {get;set;}
        }

        public class EjecutaValidator : AbstractValidator<Ejecuta>{
            public EjecutaValidator(){
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly CursosOnlineContext _context;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly IPasswordHasher<Usuario> _passwordHasher;

            public Handler(UserManager<Usuario> userManager,CursosOnlineContext context,IJwtGenerador jwtGenerador,IPasswordHasher<Usuario> passwordHasher){
                _userManager = userManager;
                _context = context;
                _jwtGenerador = jwtGenerador;
                _passwordHasher = passwordHasher;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuarioIden = await _userManager.FindByNameAsync(request.Username);
                if(usuarioIden == null){
                    throw new ExceptionHandler(HttpStatusCode.NotFound,new {mensaje = "El usuario no existe"});
                }

                var resultado = await _context.Users.Where(x => x.Email == request.Email && x.UserName != request.Username).AnyAsync();
                if(resultado == true){
                    throw new ExceptionHandler(HttpStatusCode.InternalServerError,new {mensaje = "El email ya esta en uso"});
                }

                usuarioIden.NombreCompleto = request.Nombre+" "+request.Apellidos;
                usuarioIden.PasswordHash = _passwordHasher.HashPassword(usuarioIden,request.Password);
                usuarioIden.Email = request.Email;

                var resultadoUpdate = await _userManager.UpdateAsync(usuarioIden);

                var resultadoRoles = await _userManager.GetRolesAsync(usuarioIden);
                var listaRoles = new List<string>(resultadoRoles);
                if(resultadoUpdate.Succeeded){
                    return new UsuarioData{
                        NombreCompleto = usuarioIden.NombreCompleto,
                        Username = usuarioIden.UserName,
                        Email = usuarioIden.Email,
                        Token = _jwtGenerador.CrearToken(usuarioIden,listaRoles)
                    };
                }

                throw new Exception("No se pudo actualizar");
            }
        }
    }
}
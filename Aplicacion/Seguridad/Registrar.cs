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
    public class Registrar
    {
        public class Ejecuta : IRequest<UsuarioData>{
            public string NombreCompleto {get;set;}
            public string Email {get;set;}
            public string Password {get;set;}
            public string Username {get;set;}
        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>{
            public EjecutaValidador(){
                RuleFor(x => x.NombreCompleto).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Ejecuta,UsuarioData>{
            private readonly CursosOnlineContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;
            public Handler(CursosOnlineContext context,UserManager<Usuario> userManager,IJwtGenerador jwtGenerador){
                _context = context;
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken){
                var existe = await _context.Users.Where(x => x.Email == request.Email).AnyAsync();
                if(existe){
                    throw new ExceptionHandler(HttpStatusCode.BadRequest,new {mensaje = "El email ingresado ya existe"});
                }

                var existeUsername = await _context.Users.Where(x => x.UserName == request.Username).AnyAsync();
                if(existeUsername){
                    throw new ExceptionHandler(HttpStatusCode.BadRequest,new {mensaje = "El userName ya existe"});
                }

                var usuario = new Usuario{
                    NombreCompleto = request.NombreCompleto,
                    Email = request.Email,
                    UserName = request.Username
                };

                var resultado = await _userManager.CreateAsync(usuario,request.Password);

                //como recien se crea el rol es nulo
                if(resultado.Succeeded){
                    return new UsuarioData{
                        NombreCompleto = usuario.NombreCompleto,
                        Token = _jwtGenerador.CrearToken(usuario,null),
                        Username = usuario.UserName,
                        Email=usuario.Email
                    };
                }

                throw new Exception("No se pudo agregar usuario");
            }
        }
    }
}

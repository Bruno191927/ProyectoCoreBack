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
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData>{
            public string Email {get;set;}
            public string Password {get;set;}
        }

        public class EjecutaValidacion :AbstractValidator<Ejecuta>{
            public EjecutaValidacion(){
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Ejecuta, UsuarioData>
        {

            private readonly UserManager<Usuario> _userManager;
            private readonly SignInManager<Usuario> _signInManager;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly CursosOnlineContext _context;
            public Handler(UserManager<Usuario> userManager,SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador, CursosOnlineContext context){
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtGenerador = jwtGenerador;
                _context = context;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken){
                //buscar usuario
                var usuario = await _userManager.FindByEmailAsync(request.Email);
                if(usuario == null){
                    throw new ExceptionHandler(HttpStatusCode.Unauthorized);
                }
                //checkar password
                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

                //agregar roles
                var resultadoRoles = await _userManager.GetRolesAsync(usuario);
                var listaRoles = new List<string>(resultadoRoles);

                //generar
                if(resultado.Succeeded){
                    //obtener imagen
                    var imagenPerfil = await _context.Documento.Where(x=>x.ObjectoReferencia == new Guid(usuario.Id)).FirstOrDefaultAsync();
                    if(imagenPerfil != null){
                        var imagenCliente = new ImagenGeneral{
                            Data = Convert.ToBase64String(imagenPerfil.Contenido),
                            Extension = imagenPerfil.Extension,
                            Nombre = imagenPerfil.Nombre
                        };
                        return new UsuarioData{
                            NombreCompleto = usuario.NombreCompleto,
                            Token = _jwtGenerador.CrearToken(usuario,listaRoles),
                            Username = usuario.UserName,
                            Email = usuario.Email,
                            ImagenPefil=imagenCliente
                        };
                    }
                    else{
                        return new UsuarioData{
                            NombreCompleto = usuario.NombreCompleto,
                            Token = _jwtGenerador.CrearToken(usuario,listaRoles),
                            Username = usuario.UserName,
                            Email = usuario.Email,
                            Imagen = null
                        };
                    }
                }
                else{
                    throw new ExceptionHandler(HttpStatusCode.BadRequest,new { mensaje = resultado});
                }

                throw new ExceptionHandler(HttpStatusCode.Unauthorized);
            }
        }
    }
}
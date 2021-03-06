using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class UsuarioActual
    {
        public class Ejecuta : IRequest<UsuarioData> {}

        public class Handler : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly IUsuarioSesion _usuarioSesion;
            private readonly CursosOnlineContext _context;
            public Handler(UserManager<Usuario> userManager,IJwtGenerador jwtGenerador,IUsuarioSesion usuarioSesion,CursosOnlineContext context){
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
                _usuarioSesion = usuarioSesion;
                _context = context;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());

                //agregar roles
                var resultadoRoles = await _userManager.GetRolesAsync(usuario);
                var listaRoles = new List<string>(resultadoRoles);

                var imagenPerfil = await _context.Documento.Where(x => x.ObjectoReferencia == new Guid(usuario.Id)).FirstOrDefaultAsync();

                if(imagenPerfil != null){
                    var imagenCliente = new ImagenGeneral{
                        Data = Convert.ToBase64String(imagenPerfil.Contenido),
                        Extension = imagenPerfil.Extension,
                        Nombre = imagenPerfil.Nombre
                    };
                    
                    return new UsuarioData{
                        NombreCompleto = usuario.NombreCompleto,
                        Username = usuario.UserName,
                        Token = _jwtGenerador.CrearToken(usuario,listaRoles),
                        Imagen = null,
                        Email = usuario.Email,
                        ImagenPerfil = imagenCliente
                    };

                }
                else{
                    return new UsuarioData{
                        NombreCompleto = usuario.NombreCompleto,
                        Username = usuario.UserName,
                        Token = _jwtGenerador.CrearToken(usuario,listaRoles),
                        Imagen = null,
                        Email = usuario.Email
                    };
                }
                
                
            }
        }
    }
}
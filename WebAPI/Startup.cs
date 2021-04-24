using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using MediatR;
using Aplicacion.Cursos;
using FluentValidation.AspNetCore;
using WebAPI.Middleware;
using Dominio;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication;
using Aplicacion.Contratos;
using Seguridad;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AutoMapper;
using Persistencia.DapperConexion;
using Persistencia.DapperConexion.Instructor;
using Microsoft.OpenApi.Models;
using Persistencia.DapperConexion.Paginacion;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //para utilizar cors
            services.AddCors(o => o.AddPolicy("corsApp", builder => {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            //cadena de conexion
            services.AddDbContext<CursosOnlineContext>(opt =>{
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            
            //dapper para pasarle la cadena de conexion a la clase
            services.AddOptions();
            services.Configure<ConexionConfiguracion>(Configuration.GetSection("ConnectionStrings"));
            
            //MediatR
            services.AddMediatR(typeof(Consulta.Handler).Assembly);

            //para q requiera en los endpoints q el usuario este autorizado
            services.AddControllers(opt =>{
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddFluentValidation(configure => configure.RegisterValidatorsFromAssemblyContaining<Nuevo>());
            
            //configuracion del core identity
            var builder = services.AddIdentityCore<Usuario>();
            var identityBuilder = new IdentityBuilder(builder.UserType,builder.Services);

            identityBuilder.AddRoles<IdentityRole>();//para manejo de roles seecion 17
            identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Usuario,IdentityRole>>();//para manejar token con roles seccion 17
            identityBuilder.AddEntityFrameworkStores<CursosOnlineContext>();
            identityBuilder.AddSignInManager<SignInManager<Usuario>>();//administrador de login de accesos

            //para registrar el usuario
            services.TryAddSingleton<ISystemClock,SystemClock>();

            

            //para q los enpoints necesiten la autenticacion
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {
                opt.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuerSigningKey = true, //cualquier tipo de request debe ser validado
                    IssuerSigningKey = key,
                    ValidateAudience = false, //para q cualquiera pueda hacer request
                    ValidateIssuer = false //para enviar a cualquiera
                };
            });

            //inyectar el jwtgenerador
            services.AddScoped<IJwtGenerador,JwtGenerador>();

            //para poder usar la interfaz de usuariosesion
            services.AddScoped<IUsuarioSesion,UsuarioSesion>();

            //para q funcione el mapper
            services.AddAutoMapper(typeof(Consulta.Handler));

            //para poder usar el dapper
            services.AddTransient<IFactoryConnection,FactoryConnection>();
            services.AddScoped<IInstructor,InstructorRepositorio>();
            //para usar la paginacion
            services.AddScoped<IPaginacion,PaginacionRepositorio>();

            //para usar swagger
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1",new OpenApiInfo{
                    Title = "Servicios para mantenimiento de cursos",
                    Version="v1"
                });
                c.CustomSchemaIds(c=>c.FullName);//para evitar conflictos usa el namespace
            });

            

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseCors("corsApp");
            //usar middleware
            app.UseMiddleware<ErrorHandlerMiddleware>();
            

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                //incluiremos nuestro propio middleware
            }

            //app.UseHttpsRedirection();

            //para usar la autenticacion del token
            app.UseAuthentication();

            app.UseRouting();
            //usar cors
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //para usar swagger
            app.UseSwagger();
            app.UseSwaggerUI(c=>{
                c.SwaggerEndpoint("/swagger/v1/swagger.json","Cursos Online v1");
            });
        }
    }
}

using System;
using System.Net;
using System.Threading.Tasks;
using Aplicacion.ErrorHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebAPI.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        public ErrorHandlerMiddleware(RequestDelegate next,ILogger<ErrorHandlerMiddleware> logger){
            _next = next;
            _logger = logger;
        }

        //siempre en ingles
        public async Task Invoke(HttpContext context){
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandlerExceptionAsync(context,ex,_logger);
                throw;
            }
            
        }

        private async Task HandlerExceptionAsync(HttpContext context,Exception exception, ILogger<ErrorHandlerMiddleware> logger){
            object errors = null;
            switch(exception){
                //si el error es http
                case ExceptionHandler me:
                    logger.LogError(exception , "Manejador Error");
                    errors = me.Errores;
                    context.Response.StatusCode = (int)me.Codigo;
                break;
                //si el error es generico
                case Exception e:
                    logger.LogError(exception,"Error de Servidor");
                    errors = string.IsNullOrWhiteSpace(e.Message) ? "Error":e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
            }
            context.Response.ContentType = "application/json";
            if(errors != null){
                var resultados = JsonConvert.SerializeObject(new {errors});
                await context.Response.WriteAsync(resultados);
            }
        }
    }
}
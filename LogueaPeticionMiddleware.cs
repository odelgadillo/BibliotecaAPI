using System;

namespace BibliotecaAPI
{
    public class LogueaPeticionMiddleware
    {
        private readonly RequestDelegate next;

        public LogueaPeticionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext contexto)
        {
            // Se realiza la solicitud al servidor
            var logger = contexto.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation($"Petición: {contexto.Request.Method} {contexto.Request.Path}");

            await next.Invoke(contexto);

            // El servidor responde la solicitud
            logger.LogInformation($"Respuesta: {contexto.Response.StatusCode}");
        }
    }

    public static class LogueaPeticionMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogueaPeticion(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogueaPeticionMiddleware>();
        }
    }
}
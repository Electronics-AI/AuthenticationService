using System.Net.Mime;
using AuthenticationService.Core.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace AuthenticationService.Web.Middleware
{
    public static class MiddlewareConfigurator
    {
        public static IApplicationBuilder ConfigureAndUseExceptionHandler(
            this IApplicationBuilder app
            )
        {
            app.UseExceptionHandler(errorApp => errorApp.Run(async context => 
            {
                var exception = context.Features.Get<IExceptionHandlerPathFeature>().Error;

                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = exception switch
                {
                    PasswordNotValidException _ => StatusCodes.Status401Unauthorized,
                    UserDoesNotExistException _ => StatusCodes.Status404NotFound,
                    UserAlreadyExistsException _ => StatusCodes.Status409Conflict,
                    TokenNotValidException _ => StatusCodes.Status400BadRequest, 
                    
                    _ => StatusCodes.Status500InternalServerError
                };

                if (context.Response.StatusCode < StatusCodes.Status500InternalServerError) {
                    await context.Response.WriteAsJsonAsync(exception.Message);
                    return;
                }
                await context.Response.WriteAsJsonAsync("Internal server error.");
            }));

            return app;
        }


        public static IApplicationBuilder ConfigureAndUseCors(
            this IApplicationBuilder app
            )
        {
            app.UseCors();

            return app;
        }

        public static IApplicationBuilder ConfigureAndUseSerilogRequestLogging(
            this IApplicationBuilder app
        )
        {
            app.UseSerilogRequestLogging(options => {
                options.MessageTemplate = "";
                options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Information;
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) => {
                    diagnosticContext.Set("Host", httpContext.Request.Host);
                    diagnosticContext.Set("Protocol", httpContext.Request.Protocol);
                    diagnosticContext.Set("Scheme", httpContext.Request.Scheme);
                    diagnosticContext.Set("ContentType", httpContext.Response.ContentType);
                    diagnosticContext.Set("EndpointName", httpContext.GetEndpoint()?.DisplayName);
                };
            });
            
            return app;
        }


        public static IApplicationBuilder ConfigureAndUseEndpoints(
            this IApplicationBuilder app
            )
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }

        
    }
}
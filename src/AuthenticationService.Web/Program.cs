using System;
using AuthenticationService.Web.Extensions.Host;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace AuthenticationService.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {   
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            try {
                Log.Information("Initializing and running the host.");
                CreateHostBuilder(args).Build().Run();
            } 
            catch (Exception exception) {
                Log.Fatal(exception, "Exception initializing and running the host.");
            }
            finally {
                Log.Information("Exiting the application.");
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAndUseAppConfiguration()
                .ConfigureAndUseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureAndUseKestrel();
                });
        
        
    }
}

using AuthenticationService.Web.Middleware;
using AuthenticationService.Web.Middleware.Custom;
using AuthenticationService.Web.Extensions.Service.API;
using AuthenticationService.Web.Extensions.Service.Core;
using AuthenticationService.Web.Extensions.Service.Library.Validators;
using AuthenticationService.Web.Extensions.Service.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AuthenticationService.Web.Extensions.Service.Infrastructure.Repositories;
using AuthenticationService.Web.Extensions.Service.Infrastructure.TokenProviders;
using AuthenticationService.Web.Extensions.Service.Infrastructure.Loggers;
using AuthenticationService.Web.Extensions.Service.Infrastructure.PasswordHashers;
using AuthenticationService.Web.Extensions.Service.Infrastructure.UnitsOfWork;

namespace AuthenticationService.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCoreServices();

            // services.AddDapperPostgresRepositories(Configuration);
            // services.AddDapperPostgresUnitOfWork();
            // services.AddMongoRepositories(Configuration);
            // services.AddMongoUnitOfWork();
            services.AddEFCorePostgresConfiguration(Configuration);
            services.AddEFCoreUnitOfWork();

            services.AddRedisTokenBlacklistStorage(Configuration);

            services.AddCoreFluentValidators();
            services.AddApiFluentValidators();

            services.AddSerilogLogger();

            services.AddJwtAuthentication(Configuration);
            services.AddJwtTokenImplementation(Configuration);
            
            services.AddBCryptPasswordHasher();

            services.AddApiModelMapper();
            services.AddApiControllers();
            services.AddApiAuthorizationPolicies();

            services.AddWebCorsPolicies(Configuration);
            services.AddWebResponseCaching(Configuration);
            services.AddWebHstsWithHttpsRedirection(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {   
            bool envIsProductionOrStaging = env.IsProduction() || env.IsStaging();
            
            if (env.IsDevelopment())      app.UseDeveloperExceptionPage();

            if (envIsProductionOrStaging) app.ConfigureAndUseExceptionHandler();

            if (envIsProductionOrStaging) app.UseHsts();

            if (envIsProductionOrStaging) app.UseHttpsRedirection();

            app.ConfigureAndUseSerilogRequestLogging();

            app.UseMiddleware<TokenBlacklistMiddleware>();

            app.UseRouting();
            
            if (envIsProductionOrStaging) app.ConfigureAndUseCors();
            
            if (envIsProductionOrStaging) app.UseResponseCaching();

            app.UseAuthentication();

            app.UseAuthorization();

            app.ConfigureAndUseEndpoints();
        }
    }
}

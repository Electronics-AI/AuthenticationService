using AuthenticationService.API.Models.Requests.Authentication;
using AuthenticationService.API.Models.Requests.Token;
using AuthenticationService.API.Models.Requests.User;
using AuthenticationService.API.Models.Responses.Token;
using AuthenticationService.Core.Domain.User;
using AuthenticationService.Infrastructure.Validators.FluentValidation;
using AuthenticationService.Infrastructure.Validators.FluentValidation.Domain.User;
using AuthenticationService.Infrastructure.Validators.FluentValidation.Model.Requests.Authentication;
using AuthenticationService.Infrastructure.Validators.FluentValidation.Model.Requests.Token;
using AuthenticationService.Infrastructure.Validators.FluentValidation.Model.Requests.User;
using AuthenticationService.Infrastructure.Validators.FluentValidation.Model.Responses.Token;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Web.Extensions.Service.Library.Validators
{
    public static class ValidatorsServiceExtension
    {
        public static IServiceCollection AddApiFluentValidators(
            this IServiceCollection services
        ) 
        {
            services.AddTransient<FluentValidation.IValidator<LoginUserRequestDto>,
                                  LoginUserRequestDtoValidator>();
            services.AddTransient<FluentValidation.IValidator<LogoutUserRequestDto>,
                                  LogoutUserRequestDtoValidator>();
            services.AddTransient<FluentValidation.IValidator<RefreshTokensRequestDto>,
                                  RefreshTokensRequestDtoValidator>();
            services.AddTransient<FluentValidation.IValidator<RegisterUserRequestDto>,
                                  RegisterUserRequestDtoValidator>();
            services.AddTransient<FluentValidation.IValidator<UpdateUserRequestDto>, 
                                  UpdateUserRequestDtoValidator>();
            services.AddTransient<FluentValidation.IValidator<RefreshedTokenSetResponseDto>, 
                                  RefreshedTokenSetResponseDtoValidator>();

            return services;
        }
        
        public static IServiceCollection AddCoreFluentValidators(
            this IServiceCollection services
        )
        {
            services.AddTransient<AbstractValidator<UserEntity>, UserEntityFluentValidator>();
            services.AddTransient<AbstractValidator<UserClaims>, UserClaimsFluentValidator>();
            
            services.AddTransient<AuthenticationService.Core.Interfaces.Infrastructure.Validators.IValidator<UserEntity>,
                                  GenericFluentValidator<UserEntity>>();
            services.AddTransient<AuthenticationService.Core.Interfaces.Infrastructure.Validators.IValidator<UserClaims>,
                                  GenericFluentValidator<UserClaims>>();

            return services;
        }
    }
}
using System;
using AuthenticationService.Core.Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AuthenticationService.API.Extensions
{
    public static class AuthorizationContextExtensions
    {
        public static Guid GetResourceId(
            this AuthorizationHandlerContext context
            )
        {      
            var httpContext = context.Resource as HttpContext;

            return Guid.Parse(httpContext.GetRouteValue("id").ToString());
        }

        public static UserClaims GetUserClaims(
            this AuthorizationHandlerContext context
        )
        {
            return new UserClaims(context.User.Claims);
        }
    }
}
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AuthenticationService.Core.Interfaces.Services;
using AuthenticationService.Core.Services.Token;
using Microsoft.AspNetCore.Http;

namespace AuthenticationService.Web.Middleware.Custom
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenBlacklistMiddleware(
            RequestDelegate next

        )
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITokenService tokenService)
        {
            string accessToken = context.Request.Headers["Authorization"].ToString();
            string refreshToken = context.Request.Query["RefreshToken"].ToString();

            bool accessTokenInBlacklist = false;
            bool refreshTokenInBlacklist = false;

            if (!string.IsNullOrEmpty(accessToken)) {
                accessTokenInBlacklist = await tokenService.CheckTokenInBlacklistAsync(accessToken);
            }
            if (!string.IsNullOrEmpty(refreshToken)) {
                refreshTokenInBlacklist = await tokenService.CheckTokenInBlacklistAsync(refreshToken);
            }

            if (accessTokenInBlacklist || refreshTokenInBlacklist) {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Invalid token");
            } 
            else {
                await _next(context);
            }
        }
    }
}
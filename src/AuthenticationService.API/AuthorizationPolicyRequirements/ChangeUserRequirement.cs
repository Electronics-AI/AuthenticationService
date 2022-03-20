using System;
using System.Threading.Tasks;
using AuthenticationService.API.Extensions;
using AuthenticationService.Core.Domain.User;
using AuthenticationService.Core.Interfaces.Services;
using AuthenticationService.Core.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AuthenticationService.API.AuthorizationPolicyRequirements
{
    public class ChangeUserRequirement : IAuthorizationRequirement
    {
    }

    public class ChangeUserRequirementHandler : AuthorizationHandler<ChangeUserRequirement>
    {   
        private readonly IUserService _userService;
        private readonly ILogger<ChangeUserRequirementHandler> _logger;
        
        public ChangeUserRequirementHandler(
            IUserService userService,
            ILogger<ChangeUserRequirementHandler> logger
        )
        {
            _userService = userService ?? 
                throw new ArgumentNullException(nameof(userService));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ChangeUserRequirement requirement
            )
        {
            UserClaims userClaims = context.GetUserClaims();     
            Guid userToChangeId = context.GetResourceId();

            _logger.LogInformation("Trying to check if a user with the claims: {@userClaims}; " +
                                   "is allowed to change a user with the id: {userToChangeId}",
                                   userClaims, userToChangeId);

            if (await _userService.CheckUserIsAllowedToChangeUserAsync(userClaims, userToChangeId)) {
                 _logger.LogInformation("User with the claims: {@userClaims}; is allowed to change " + 
                                       "a user with the id: {userToChangeId}",
                                       userClaims, userToChangeId);

                context.Succeed(requirement);
                return;
            }

            _logger.LogInformation("User with the claims: {@userClaims}; is not allowed to change " + 
                                   "a user with the id: {userToChangeId}",
                                    userClaims, userToChangeId);
            context.Fail();
        }
    }
}
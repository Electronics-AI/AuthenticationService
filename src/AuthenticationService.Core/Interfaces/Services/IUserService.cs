using System;
using System.Threading.Tasks;
using AuthenticationService.Core.Domain.User;

namespace AuthenticationService.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task RegisterUserAsync(UserEntity registrationUser);
        Task UpdateUserAsync(UserEntity updatedUser);
        Task DeleteUserByIdAsync(Guid id);
        Task<UserEntity> GetUserByIdAsync(Guid id);
        Task<bool> CheckUserIsAllowedToChangeUserAsync(UserClaims userClaims, Guid userToChangeId);
    }
}
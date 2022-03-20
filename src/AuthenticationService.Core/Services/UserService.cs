using System;
using System.Threading.Tasks;
using AuthenticationService.Core.Domain.User;
using AuthenticationService.Core.Exceptions;
using AuthenticationService.Core.Interfaces.Infrastructure.PasswordHashers;
using AuthenticationService.Core.Interfaces.Infrastructure.Repositories;
using AuthenticationService.Core.Interfaces.Services;

namespace AuthenticationService.Core.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        
        public UserService(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher
        )
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _passwordHasher = passwordHasher ??
                throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<bool> CheckUserIsAllowedToChangeUserAsync(UserClaims userClaims, Guid userToChangeId)
        {
            if (userClaims.Id == userToChangeId) {
                return true;
            }
            else if (userClaims.Role == RoleTypes.Admin) {
                UserEntity userToChange = await _unitOfWork.Users.GetByIdAsync(userToChangeId) ??
                    throw new UserDoesNotExistException("User you are trying to change does not exist.");

                bool canBeChangedByAdminRole = !(userToChange.Role == RoleTypes.Admin);
                return canBeChangedByAdminRole;
            }
            return false;
        }

        public async Task RegisterUserAsync(UserEntity registrationUser)
        {   
            if (await _unitOfWork.Users.GetByUserNameAsync(registrationUser.UserName) != null)
                throw new UserAlreadyExistsException($"User with the username '{registrationUser.UserName}' already exists.");
 
            if (await _unitOfWork.Users.GetByEmailAsync(registrationUser.Email) != null)
                throw new UserAlreadyExistsException($"User with the email '{registrationUser.Email}' already exists.");

            registrationUser.Password.Hash(_passwordHasher);

            registrationUser.CreationDate = DateTime.Now;

            await _unitOfWork.Users.AddAsync(registrationUser);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteUserByIdAsync(Guid id)
        {
            UserEntity userToDelete = await _unitOfWork.Users.GetByIdAsync(id) ??
                throw new UserDoesNotExistException("User you are trying to delete does not exist.");

            await _unitOfWork.Users.RemoveAsync(userToDelete);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<UserEntity> GetUserByIdAsync(Guid id)
        {
            return await _unitOfWork.Users.GetByIdAsync(id) ??
                throw new UserDoesNotExistException("User you are trying to get does not exist.");
        }

        public async Task UpdateUserAsync(UserEntity updatedUser)
        {
            UserEntity userToUpdate = await _unitOfWork.Users.GetByIdAsync(updatedUser.Id) ??
                throw new UserDoesNotExistException("User you are trying to update does not exist.");
            
            userToUpdate = updatedUser;

            await _unitOfWork.Users.UpdateAsync(userToUpdate = updatedUser);
            await _unitOfWork.CompleteAsync();
        }
    }
}
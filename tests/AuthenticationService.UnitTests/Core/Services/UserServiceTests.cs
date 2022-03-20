using System;
using System.Threading.Tasks;
using AuthenticationService.Core.Domain.User;
using AuthenticationService.Core.Exceptions;
using AuthenticationService.Core.Interfaces.Infrastructure.PasswordHashers;
using AuthenticationService.Core.Interfaces.Infrastructure.Repositories;
using AuthenticationService.Core.Services.User;
using Moq;
using Xunit;

namespace AuthenticationService.UnitTests.Core.Services.User
{
    public class UserServiceTests
    {
        private readonly UserService _userServiceUT;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();

        public UserServiceTests()
        {
            _userServiceUT = new UserService(_unitOfWorkMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task RegisterUserAsync_WhenUserWithRegistrationUserEmailAlreadyExists_ShouldThrowUserAlreadyExistsException()
        {   
            string existingUserName = "ExistingUsername";

            UserEntity registrationUser = sampleValidRegistrationUser;
            UserEntity registeredUser = sampleValidRegisteredUser;

            registrationUser.UserName = registeredUser.UserName = existingUserName;
            
            _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.Users.GetByUserNameAsync(registeredUser.UserName))
                .ReturnsAsync(registeredUser)
                .Verifiable();
            
            Func<Task> act = () => _userServiceUT.RegisterUserAsync(registrationUser);

            await Assert.ThrowsAsync<UserAlreadyExistsException>(act);

            _unitOfWorkMock.VerifyAll();
        }

        [Fact]
        public async Task RegisterUserAsync_WhenUserWithRegistrationUserNameAlreadyExists_ShouldThrowUserAlreadyExistsException()
        {
            string existingUserEmail = "emailexistsindb@gmail.com";

            UserEntity registrationUser = sampleValidRegistrationUser;
            UserEntity registeredUser = sampleValidRegisteredUser;

            registrationUser.Email = registeredUser.Email = existingUserEmail;

            _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.Users.GetByEmailAsync(registeredUser.Email))
                .ReturnsAsync(registeredUser)
                .Verifiable();
            
            Func<Task> act = () => _userServiceUT.RegisterUserAsync(registrationUser);
            
            await Assert.ThrowsAsync<UserAlreadyExistsException>(act);

            _unitOfWorkMock.VerifyAll();
        }
        
        [Fact]
        public async Task RegisterUserAsync_WhenRegistrationUserIsValid_ShouldAddUserToUserRepository()
        {
            UserEntity registrationUser = sampleValidRegistrationUser;

            _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.Users.GetByUserNameAsync(registrationUser.UserName))
                .ReturnsAsync(() => null)
                .Verifiable();
            
            _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.Users.GetByEmailAsync(registrationUser.Email))
                .ReturnsAsync(() => null)
                .Verifiable();

            await _userServiceUT.RegisterUserAsync(registrationUser);

            _unitOfWorkMock.VerifyAll();
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.Users.AddAsync(registrationUser), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_WhenUserExists_ShouldReturnCorrectUser()
        {
            UserEntity registeredUser = sampleValidRegisteredUser;

            _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.Users.GetByIdAsync(registeredUser.Id))
                .ReturnsAsync(registeredUser)
                .Verifiable();

            UserEntity receivedUser = await _userServiceUT.GetUserByIdAsync(registeredUser.Id);

            _unitOfWorkMock.VerifyAll();

            Assert.Equal(registeredUser.Id, receivedUser.Id);

        }

        [Fact]
        public async Task GetUserByIdAsync_WhenUserDoesNotExist_ShouldThrowUserDoesNotExistException(
        )
        {   
            Guid nonExistingUserId = Guid.NewGuid();

            _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.Users.GetByIdAsync(nonExistingUserId))
                .ReturnsAsync(() => null)
                .Verifiable();

            Func<Task> act = () => _userServiceUT.GetUserByIdAsync(nonExistingUserId);

            await Assert.ThrowsAsync<UserDoesNotExistException>(act);
            
            _unitOfWorkMock.VerifyAll();
        }

        [Theory]
        [InlineData(RoleTypes.User)]
        [InlineData(RoleTypes.Admin)]
        public async Task CheckUserIsAllowedToChangeUserAsync_WhenUserHasAnyRole_ShouldBeAbleToDeleteItself(
            RoleTypes changingUserRole
        )
        {   
            Guid userId = Guid.NewGuid();

            UserEntity changingUser = sampleValidRegisteredUser;
            changingUser.Id = userId;
            
            UserClaims changingUserClaims = new UserClaims() {
                Id = userId,
            };

            changingUserClaims = new UserClaims() {
                Id = userId,
                Role = changingUserRole
            };

            bool userIsAllowedToChange = await _userServiceUT.CheckUserIsAllowedToChangeUserAsync(
                changingUserClaims, userId
                );

            Assert.True(userIsAllowedToChange);
        }

        [Theory]
        [InlineData(RoleTypes.User)]
        [InlineData(RoleTypes.Admin)]
        public async Task CheckUserIsAllowedToChangeUserAsync_WhenUserHasUserRole_ShouldNotBeAbleToDeleteOtherUsers(
            RoleTypes userToChangeRole
        )
        {   
            Guid changingUserId = Guid.NewGuid();
            Guid userToChangeId = Guid.NewGuid();

            UserEntity changingUser = sampleValidRegisteredUser;
            changingUser.Id = changingUserId;
            changingUser.Role = RoleTypes.User;

            UserEntity userToChange = sampleValidRegisteredUser;
            userToChange.Id = userToChangeId;
            userToChange.Role = userToChangeRole;

            UserClaims changingUserClaims = new UserClaims(changingUser);


            _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.Users.GetByIdAsync(userToChange.Id))
                .ReturnsAsync(userToChange);

            bool userIsAllowedToChange = await _userServiceUT.CheckUserIsAllowedToChangeUserAsync(
                changingUserClaims, userToChangeId
                );

            Assert.False(userIsAllowedToChange);
        }

        [Fact]
        public async Task CheckUserIsAllowedToChangeUserAsync_WhenUserHasAdminRole_ShouldBeAbleToDeleteOtherUsersWithUserRole()
        {
            Guid changingUserId = Guid.NewGuid();
            Guid userToChangeId = Guid.NewGuid();

            UserEntity changingUser = sampleValidRegisteredUser;
            changingUser.Id = changingUserId;
            changingUser.Role = RoleTypes.Admin;

            UserEntity userToChange = sampleValidRegisteredUser;
            userToChange.Id = userToChangeId;
            userToChange.Role = RoleTypes.User;

            UserClaims changingUserClaims = new UserClaims(changingUser);

            _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.Users.GetByIdAsync(userToChange.Id))
                .ReturnsAsync(userToChange)
                .Verifiable();

            bool userIsAllowedToChange = await _userServiceUT.CheckUserIsAllowedToChangeUserAsync(
                    changingUserClaims, userToChange.Id
                    );

            _unitOfWorkMock.VerifyAll();

            Assert.True(userIsAllowedToChange);
        }

        [Fact]
        public async Task CheckUserIsAllowedToChangeUserAsync_WhenUserHasAdminRole_ShouldNotBeAbleToDeleteOtherUsersWithAdminRole()
        {
            Guid changingUserId = Guid.NewGuid();
            Guid userToChangeId = Guid.NewGuid();

            UserEntity changingUser = sampleValidRegisteredUser;
            changingUser.Id = changingUserId;
            changingUser.Role = RoleTypes.Admin;

            UserEntity userToChange = sampleValidRegisteredUser;
            userToChange.Id = userToChangeId;
            userToChange.Role = RoleTypes.Admin;

            UserClaims changingUserClaims = new UserClaims(changingUser);

            _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.Users.GetByIdAsync(userToChange.Id))
                .ReturnsAsync(userToChange)
                .Verifiable();

            bool userIsAllowedToChange = await _userServiceUT.CheckUserIsAllowedToChangeUserAsync(
                    changingUserClaims, userToChange.Id
                    );
            
            _unitOfWorkMock.VerifyAll();

            Assert.False(userIsAllowedToChange);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenUserToUpdateDoesNotExist_ShouldThrowUserDoesNotExistException()
        {
            UserEntity userToUpdate = sampleValidRegisteredUser;

            _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.Users.GetByIdAsync(userToUpdate.Id))
                .ReturnsAsync(() => null)
                .Verifiable();

            Func<Task> act = () => _userServiceUT.UpdateUserAsync(userToUpdate);
            
            await Assert.ThrowsAsync<UserDoesNotExistException>(act);

            _unitOfWorkMock.VerifyAll();
        }

        [Fact]
        public async Task UpdateUserAsync_WhenUserToUpdateExists_ShouldUpdateUserInRepository()
        {
            UserEntity userToUpdate = sampleValidRegisteredUser;

            _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.Users.GetByIdAsync(userToUpdate.Id))
                .ReturnsAsync(userToUpdate)
                .Verifiable();

            await _userServiceUT.UpdateUserAsync(userToUpdate);

            _unitOfWorkMock.VerifyAll();
           _unitOfWorkMock.Verify(unitOfWork => unitOfWork.Users.UpdateAsync(userToUpdate), Times.Once);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_WhenUserToDeleteDoesNotExist_ShouldThrowUserDoesNotExistException()
        {
            UserEntity userToDelete = sampleValidRegisteredUser;

            _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.Users.GetByIdAsync(userToDelete.Id))
                .ReturnsAsync(() => null)
                .Verifiable();

            Func<Task> act = () => _userServiceUT.DeleteUserByIdAsync(userToDelete.Id);
            
            await Assert.ThrowsAsync<UserDoesNotExistException>(act);

            _unitOfWorkMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteUserByIdAsync_WhenUserToDeleteExists_ShouldDeleteInRepository()
        {
            UserEntity userToDelete = sampleValidRegisteredUser;

            _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.Users.GetByIdAsync(userToDelete.Id))
                .ReturnsAsync(userToDelete)
                .Verifiable();

            await _userServiceUT.DeleteUserByIdAsync(userToDelete.Id);

            _unitOfWorkMock.VerifyAll();
        
           _unitOfWorkMock.Verify(unitOfWork => unitOfWork.Users.RemoveAsync(userToDelete), Times.Once);

        }

        private UserEntity sampleValidRegisteredUser => new UserEntity() {
            Id = Guid.NewGuid(),
            UserName = "TestCreatedUser",
            Email = "testuser@gmail.com",
            Gender = GenderTypes.Male,
            Role = RoleTypes.User,
            DateOfBirth = new DateTime(1986, 11, 27),
            Password = new Password() {Type = PasswordTypes.Hashed, Value = "$K479WMDL224L$W20"},
            LastLoginDate = new DateTime(2007, 3, 4),
            CreationDate = new DateTime(2001, 1, 15)
        };

        private UserEntity sampleValidRegistrationUser = new UserEntity() {
            UserName = "TestRegistrationUser",
            Email = "testregistrationuser@gmail.com",
            Gender = GenderTypes.Male,
            Role = RoleTypes.User,
            DateOfBirth = new DateTime(1986, 11, 27),
            Password = new Password() {Type = PasswordTypes.PlainText, Value = "plainTextPassword"},
            LastLoginDate = null,
            CreationDate = null
        };
    }
}
using System;
using System.Threading.Tasks;
using AuthenticationService.API.Controllers;
using AuthenticationService.API.Models.Requests.User;
using AuthenticationService.Core.Domain.User;
using AuthenticationService.Core.Interfaces.Infrastructure.Loggers;
using AuthenticationService.Core.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AuthenticationService.UnitTests.API.Controllers
{
    public class UsersControllerTests
    {
        private readonly UsersController _usersControllerUT;
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<UsersController>> _loggerMock = new();

        public UsersControllerTests()
        {
            _usersControllerUT = new UsersController(
                _userServiceMock.Object, _mapperMock.Object, _loggerMock.Object
            );
        }

        [Fact]
        public async Task RegisterUserAsync_WhenRegisterUserDtoIsValid_ShouldRegisterUser()
        {
            RegisterUserRequestDto registerUserRequestDto = new RegisterUserRequestDto() {
                UserName = "registrationTestUser",
                Email = "reguser@gmail.com",
                Gender = GenderTypes.Male,
                DateOfBirth = new DateTime(2001, 11, 6),
                Password = "3571tjt2slq0",
                ConfirmPassword = "3571tjt2slq0"
            };

            UserEntity registrationUser = new UserEntity() {
                UserName = registerUserRequestDto.UserName,
                Email = registerUserRequestDto.Email, 
                Gender = registerUserRequestDto.Gender,
                DateOfBirth = registerUserRequestDto.DateOfBirth,
                Password = new Password() {Type = PasswordTypes.PlainText, Value = registerUserRequestDto.Password},
            };

            _mapperMock.Setup(mapper =>
                mapper.Map<UserEntity>(registerUserRequestDto))
                .Returns(registrationUser)
                .Verifiable();

            _userServiceMock.Setup(userService =>
                userService.RegisterUserAsync(registrationUser))
                .Verifiable();
            
            var registerUserResult = await _usersControllerUT.RegisterUserAsync(registerUserRequestDto);

            _userServiceMock.VerifyAll();
            _mapperMock.VerifyAll();          

            Assert.IsType<NoContentResult>(registerUserResult);
        }

        [Fact]
        public async Task DeleteUserAsync_WhenDeleteUserRequestDtoContainsValidUserToDeleteId_ShouldDeleteUser()
        {
            DeleteUserByIdRequestDto deleteUserByIdRequestDto = new DeleteUserByIdRequestDto() {
                Id = Guid.NewGuid()
            };

            _userServiceMock.Setup(userService =>
                userService.DeleteUserByIdAsync(deleteUserByIdRequestDto.Id))
                .Verifiable();
            
            var deleteUserResult = await _usersControllerUT.DeleteUserAsync(deleteUserByIdRequestDto);

            _userServiceMock.VerifyAll();
            
            Assert.IsType<NoContentResult>(deleteUserResult);

        }
    }
}
using System;
using System.Net.Mime;
using System.Threading.Tasks;
using AuthenticationService.API.Models.Requests.User;
using AuthenticationService.Core.Domain.User;
using AuthenticationService.Core.Interfaces.Infrastructure.Loggers;
using AuthenticationService.Core.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            ILogger<UsersController> logger
            )
        {
            _userService = userService ??
                throw new ArgumentNullException(nameof(userService));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost()]
        [ProducesDefaultResponseType()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> RegisterUserAsync(
            [FromBody]RegisterUserRequestDto registerUserRequestDto
            )
        {   
            UserEntity registrationUser = _mapper.Map<UserEntity>(registerUserRequestDto);
            
            _logger.Log(LogLevelTypes.Information,
                        "Trying to register a new user by the given email: {userRegistrationEmail}; and " +
                        "the username: {userRegistrationUserName};",
                        args: new string[]{registrationUser.Email, registrationUser.UserName});

            await _userService.RegisterUserAsync(registrationUser);
            
            _logger.Log(LogLevelTypes.Information, 
                        "User with the email: {userRegistrationEmail};" + 
                        "and the username: {userRegistrationUserName};" + 
                        "has successfully been registered.",
                        args: new string[]{registrationUser.Email, registrationUser.UserName});

            return NoContent();
        }

        [Authorize("ChangeUserAccess")]
        [HttpDelete("{id}")]
        [ProducesDefaultResponseType()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteUserAsync(
            [FromRoute]DeleteUserByIdRequestDto deleteUserByIdRequestDto
        )
        {
            _logger.Log(LogLevelTypes.Information, 
                        "Trying to delete a user by the given id: {userToDeleteId}.",
                        args: deleteUserByIdRequestDto.Id);

            await _userService.DeleteUserByIdAsync(deleteUserByIdRequestDto.Id);

            _logger.Log(LogLevelTypes.Information,
                        "User with id: {userToDeleteId} has successfully been deleted.",
                        args: deleteUserByIdRequestDto.Id);
                        
            return NoContent();
        }

        [Authorize("ChangeUserAccess")]
        [HttpPatch("{id}")]
        [ProducesDefaultResponseType()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> UpdateUserAsync(
            Guid userToUpdateId, 
            JsonPatchDocument<UpdateUserRequestDto> updateUserPatchDocument 
        )
        {            
            UserEntity userToUpdate = await _userService.GetUserByIdAsync(userToUpdateId);

            var userToUpdateDto = _mapper.Map<UpdateUserRequestDto>(userToUpdate);

            updateUserPatchDocument.ApplyTo(userToUpdateDto);

            if (!TryValidateModel(userToUpdateDto)) {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(userToUpdateDto, userToUpdate);

            _logger.Log(LogLevelTypes.Information, 
                        "Trying to update a user by the given id: {userToUpdateId}.",
                        args: userToUpdateId);

            await _userService.UpdateUserAsync(userToUpdate);

            _logger.Log(LogLevelTypes.Information,
                        "User with the id: {userToUpdateId}, has successfully been updated.",
                        args: userToUpdateId);

            return Ok();
        }

    } 
}
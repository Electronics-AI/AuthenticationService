using System;

namespace AuthenticationService.API.Models.Requests.User
{
    public class DeleteUserByIdRequestDto
    {
        public Guid Id { get; set; }
    }
}
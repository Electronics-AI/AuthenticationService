using AuthenticationService.API.Models.Requests.User;
using AuthenticationService.Core.Domain.User;
using AutoMapper;

namespace AuthenticationService.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserRequestDto, UserEntity>()
            .ForMember(user => user.UserName, opt => 
                       opt.MapFrom(registerDto => registerDto.UserName))
            .ForMember(user => user.Email, opt => 
                       opt.MapFrom(registerDto => registerDto.Email))
            .ForMember(user => user.Gender, opt => 
                       opt.MapFrom(registerDto => registerDto.Gender))
            .ForMember(user => user.DateOfBirth, opt => 
                       opt.MapFrom(registerDto => registerDto.DateOfBirth))
            .ForMember(user => user.Password, opt => 
                       opt.MapFrom(registerDto => new Password(){Type = PasswordTypes.PlainText, 
                                                                 Value = registerDto.Password}));

            CreateMap<UserEntity, UpdateUserRequestDto>()
            .ForMember(updateDto => updateDto.UserName, opt => 
                       opt.MapFrom(user => user.UserName))
            .ForMember(updateDto => updateDto.Email, opt => 
                       opt.MapFrom(user => user.Email))
            .ForMember(updateDto => updateDto.DateOfBirth, opt => 
                       opt.MapFrom(user => user.DateOfBirth))
            .ForMember(updateDto => updateDto.Gender, opt => 
                       opt.MapFrom(user => user.Gender))
            .ForMember(updateDto => updateDto.Role, opt =>
                       opt.MapFrom(user => user.Role));

            CreateMap<UpdateUserRequestDto, UserEntity>()
            .ForMember(user => user.UserName, opt => 
                       opt.MapFrom(updateDto => updateDto.UserName))
            .ForMember(user => user.Email, opt => 
                       opt.MapFrom(updateDto => updateDto.Email))
            .ForMember(user => user.DateOfBirth, opt => 
                       opt.MapFrom(updateDto => updateDto.DateOfBirth))
            .ForMember(user => user.Gender, opt => 
                       opt.MapFrom(updateDto => updateDto.Gender));

        }

    }
}
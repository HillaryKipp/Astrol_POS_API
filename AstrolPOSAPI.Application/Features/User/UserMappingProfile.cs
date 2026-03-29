using AstrolPOSAPI.Application.Features.User.DTOs;
using AstrolPOSAPI.Domain.Entities.Identity;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.User
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<AppUser, UserDto>();
        }
    }
}

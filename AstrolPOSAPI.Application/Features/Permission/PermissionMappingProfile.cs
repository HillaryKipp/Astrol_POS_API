using AstrolPOSAPI.Application.Features.Permission.DTOs;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.Permission
{
    public class PermissionMappingProfile : Profile
    {
        public PermissionMappingProfile()
        {
            CreateMap<AstrolPOSAPI.Domain.Entities.Identity.Permission, PermissionDto>();
            CreateMap<CreatePermissionDto, AstrolPOSAPI.Domain.Entities.Identity.Permission>();
            CreateMap<UpdatePermissionDto, AstrolPOSAPI.Domain.Entities.Identity.Permission>();
        }
    }
}

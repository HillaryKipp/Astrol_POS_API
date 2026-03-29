using AstrolPOSAPI.Application.Features.POS.DrawerGroup.Commands.CreateDrawerGroup;
using AstrolPOSAPI.Application.Features.POS.DrawerGroup.Commands.UpdateDrawerGroup;
using AstrolPOSAPI.Application.Features.POS.DrawerGroup.DTOs;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.POS.DrawerGroup
{
    public class DrawerGroupMappingProfile : Profile
    {
        public DrawerGroupMappingProfile()
        {
            CreateMap<Domain.Entities.POS.DrawerGroup, DrawerGroupDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : null))
                .ForMember(dest => dest.StoreOfOperationName, opt => opt.MapFrom(src => src.StoreOfOperation != null ? src.StoreOfOperation.Name : null));
            CreateMap<CreateDrawerGroupDto, Domain.Entities.POS.DrawerGroup>();
            CreateMap<UpdateDrawerGroupDto, Domain.Entities.POS.DrawerGroup>();
            CreateMap<CreateDrawerGroupCommand, Domain.Entities.POS.DrawerGroup>();
            CreateMap<UpdateDrawerGroupCommand, Domain.Entities.POS.DrawerGroup>();
        }
    }
}

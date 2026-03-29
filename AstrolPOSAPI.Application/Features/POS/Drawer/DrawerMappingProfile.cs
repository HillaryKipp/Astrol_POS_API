using AstrolPOSAPI.Application.Features.POS.Drawer.Commands.CreateDrawer;
using AstrolPOSAPI.Application.Features.POS.Drawer.DTOs;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.POS.Drawer
{
    public class DrawerMappingProfile : Profile
    {
        public DrawerMappingProfile()
        {
            CreateMap<AstrolPOSAPI.Domain.Entities.POS.Drawer, DrawerDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : null))
                .ForMember(dest => dest.StoreOfOperationName, opt => opt.MapFrom(src => src.StoreOfOperation != null ? src.StoreOfOperation.Name : null))
                .ForMember(dest => dest.DrawerGroupName, opt => opt.MapFrom(src => src.DrawerGroup != null ? src.DrawerGroup.Name : null))
                .ForMember(dest => dest.TerminalName, opt => opt.MapFrom(src => src.Terminal != null ? src.Terminal.Description : null))
                .ForMember(dest => dest.DefaultScreenName, opt => opt.MapFrom(src => src.DefaultScreen != null ? src.DefaultScreen.Description : null))
                .ReverseMap();
            CreateMap<AstrolPOSAPI.Application.Features.POS.Drawer.Commands.CreateDrawer.CreateDrawerCommand, AstrolPOSAPI.Domain.Entities.POS.Drawer>();
            CreateMap<AstrolPOSAPI.Application.Features.POS.Drawer.Commands.UpdateDrawer.UpdateDrawerCommand, AstrolPOSAPI.Domain.Entities.POS.Drawer>();
        }
    }
}

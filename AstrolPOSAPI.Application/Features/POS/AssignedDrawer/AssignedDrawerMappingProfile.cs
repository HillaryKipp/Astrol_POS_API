using AstrolPOSAPI.Application.Features.POS.AssignedDrawer.Commands.CreateAssignedDrawer;
using AstrolPOSAPI.Application.Features.POS.AssignedDrawer.Commands.UpdateAssignedDrawer;
using AstrolPOSAPI.Application.Features.POS.AssignedDrawer.DTOs;
using AutoMapper;
using AssignedDrawerEntity = AstrolPOSAPI.Domain.Entities.POS.AssignedDrawer;

namespace AstrolPOSAPI.Application.Features.POS.AssignedDrawer
{
    public class AssignedDrawerMappingProfile : Profile
    {
        public AssignedDrawerMappingProfile()
        {
            CreateMap<AssignedDrawerEntity, AssignedDrawerDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : null))
                .ForMember(dest => dest.StoreOfOperationName, opt => opt.MapFrom(src => src.StoreOfOperation != null ? src.StoreOfOperation.Name : null))
                .ForMember(dest => dest.DrawerName, opt => opt.MapFrom(src => src.Drawer != null ? src.Drawer.Name : null))
                .ForMember(dest => dest.DrawerStatus, opt => opt.MapFrom(src => src.Drawer != null ? (AstrolPOSAPI.Domain.Entities.POS.DrawerStatus?)src.Drawer.Status : null))
                .ForMember(dest => dest.DefaultScreenName, opt => opt.MapFrom(src => src.DefaultScreen != null ? src.DefaultScreen.Name : null))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null));
            CreateMap<CreateAssignedDrawerCommand, AssignedDrawerEntity>();
            CreateMap<UpdateAssignedDrawerCommand, AssignedDrawerEntity>();

            CreateMap<CreateAssignedDrawerDto, AssignedDrawerEntity>();
            CreateMap<UpdateAssignedDrawerDto, AssignedDrawerEntity>();
        }
    }
}

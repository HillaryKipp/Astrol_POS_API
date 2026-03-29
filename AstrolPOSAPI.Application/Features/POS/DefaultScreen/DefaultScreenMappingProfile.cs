using AstrolPOSAPI.Application.Features.POS.DefaultScreen.DTOs;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.POS.DefaultScreen
{
    public class DefaultScreenMappingProfile : Profile
    {
        public DefaultScreenMappingProfile()
        {
            CreateMap<AstrolPOSAPI.Domain.Entities.POS.DefaultScreen, DefaultScreenDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : null))
                .ForMember(dest => dest.StoreOfOperationName, opt => opt.MapFrom(src => src.StoreOfOperation != null ? src.StoreOfOperation.Name : null))
                .ReverseMap();
            CreateMap<CreateDefaultScreenDto, AstrolPOSAPI.Domain.Entities.POS.DefaultScreen>();
            CreateMap<UpdateDefaultScreenDto, AstrolPOSAPI.Domain.Entities.POS.DefaultScreen>();
            
            CreateMap<AstrolPOSAPI.Application.Features.POS.DefaultScreen.Commands.CreateDefaultScreen.CreateDefaultScreenCommand, AstrolPOSAPI.Domain.Entities.POS.DefaultScreen>();
            CreateMap<AstrolPOSAPI.Application.Features.POS.DefaultScreen.Commands.UpdateDefaultScreen.UpdateDefaultScreenCommand, AstrolPOSAPI.Domain.Entities.POS.DefaultScreen>();
        }
    }
}

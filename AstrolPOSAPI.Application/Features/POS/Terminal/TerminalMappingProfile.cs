using AstrolPOSAPI.Application.Features.POS.Terminal.DTOs;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.POS.Terminal
{
    public class TerminalMappingProfile : Profile
    {
        public TerminalMappingProfile()
        {
            CreateMap<AstrolPOSAPI.Domain.Entities.POS.Terminal, TerminalDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : null))
                .ForMember(dest => dest.StoreOfOperationName, opt => opt.MapFrom(src => src.StoreOfOperation != null ? src.StoreOfOperation.Name : null))
                .ReverseMap();
            CreateMap<AstrolPOSAPI.Application.Features.POS.Terminal.Commands.CreateTerminal.CreateTerminalCommand, AstrolPOSAPI.Domain.Entities.POS.Terminal>();
            CreateMap<AstrolPOSAPI.Application.Features.POS.Terminal.Commands.UpdateTerminal.UpdateTerminalCommand, AstrolPOSAPI.Domain.Entities.POS.Terminal>();
        }
    }
}

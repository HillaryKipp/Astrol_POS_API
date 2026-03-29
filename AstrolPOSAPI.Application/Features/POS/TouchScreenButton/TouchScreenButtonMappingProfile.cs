using AstrolPOSAPI.Application.Features.POS.TouchScreenButton.Commands.UpdateTouchScreenButton;
using AstrolPOSAPI.Application.Features.POS.TouchScreenButton.DTOs;
using AstrolPOSAPI.Domain.Entities.POS;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.POS.TouchScreenButton
{
    public class TouchScreenButtonMappingProfile : Profile
    {
        public TouchScreenButtonMappingProfile()
        {
            CreateMap<AstrolPOSAPI.Domain.Entities.POS.TouchScreenButton, TouchScreenButtonDto>().ReverseMap();
            CreateMap<AstrolPOSAPI.Application.Features.POS.TouchScreenButton.Commands.CreateTouchScreenButton.CreateTouchScreenButtonCommand, AstrolPOSAPI.Domain.Entities.POS.TouchScreenButton>();
            CreateMap<UpdateTouchScreenButtonCommand, AstrolPOSAPI.Domain.Entities.POS.TouchScreenButton>();
        }
    }
}

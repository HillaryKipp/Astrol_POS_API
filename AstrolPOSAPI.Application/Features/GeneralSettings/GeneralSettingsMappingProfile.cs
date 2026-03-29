using AstrolPOSAPI.Application.Features.GeneralSettings.DTOs;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.GeneralSettings
{
    public class GeneralSettingsMappingProfile : Profile
    {
        public GeneralSettingsMappingProfile()
        {
            CreateMap<Domain.Entities.Core.GeneralSettings, GeneralSettingsDto>();
            CreateMap<CreateGeneralSettingsDto, Domain.Entities.Core.GeneralSettings>();
            CreateMap<UpdateGeneralSettingsDto, Domain.Entities.Core.GeneralSettings>();
        }
    }
}

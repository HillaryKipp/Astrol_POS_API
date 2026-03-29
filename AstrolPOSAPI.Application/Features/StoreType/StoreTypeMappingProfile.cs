using AstrolPOSAPI.Application.Features.StoreType.DTOs;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.StoreType
{
    public class StoreTypeMappingProfile : Profile
    {
        public StoreTypeMappingProfile()
        {
            CreateMap<AstrolPOSAPI.Domain.Entities.Core.StoreType, StoreTypeDto>();
            CreateMap<CreateStoreTypeDto, AstrolPOSAPI.Domain.Entities.Core.StoreType>();
            CreateMap<UpdateStoreTypeDto, AstrolPOSAPI.Domain.Entities.Core.StoreType>();
        }
    }
}

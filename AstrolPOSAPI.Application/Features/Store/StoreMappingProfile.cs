using AstrolPOSAPI.Application.Features.Store.DTOs;
using AstrolPOSAPI.Domain.Entities.Core;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.Store
{
    public class StoreMappingProfile : Profile
    {
        public StoreMappingProfile()
        {
            CreateMap<AstrolPOSAPI.Domain.Entities.Core.Store, StoreDto>().ReverseMap();
        }
    }
}

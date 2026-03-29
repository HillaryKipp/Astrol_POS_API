using AstrolPOSAPI.Application.Features.Purchasing.Vendor.DTOs;
using AstrolPOSAPI.Domain.Entities.Purchasing;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.Purchasing.Vendor
{
    public class VendorMappingProfile : Profile
    {
        public VendorMappingProfile()
        {
            CreateMap<AstrolPOSAPI.Domain.Entities.Purchasing.Vendor, VendorDto>()
                .ForMember(dest => dest.VendorPostingGroupCode, opt => opt.MapFrom(src => src.VendorPostingGroup != null ? src.VendorPostingGroup.Code : null))
                .ForMember(dest => dest.GenBusPostingGroupCode, opt => opt.MapFrom(src => src.GenBusPostingGroup != null ? src.GenBusPostingGroup.Code : null));

            CreateMap<CreateVendorDto, AstrolPOSAPI.Domain.Entities.Purchasing.Vendor>();
            CreateMap<UpdateVendorDto, AstrolPOSAPI.Domain.Entities.Purchasing.Vendor>();
        }
    }
}

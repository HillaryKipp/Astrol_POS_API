using AstrolPOSAPI.Application.Features.Purchasing.Invoice.DTOs;
using AstrolPOSAPI.Domain.Entities.Purchasing;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.Purchasing.Invoice
{
    public class PurchaseMappingProfile : Profile
    {
        public PurchaseMappingProfile()
        {
            CreateMap<PurchaseHeader, PurchaseHeaderDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<PurchaseLine, PurchaseLineDto>();

            CreateMap<CreatePurchaseHeaderDto, PurchaseHeader>();
            CreateMap<CreatePurchaseLineDto, PurchaseLine>();

            // Posted Variants
            CreateMap<PurchInvHeader, PurchaseHeaderDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "Posted"));
            CreateMap<PurchInvLine, PurchaseLineDto>();
        }
    }
}

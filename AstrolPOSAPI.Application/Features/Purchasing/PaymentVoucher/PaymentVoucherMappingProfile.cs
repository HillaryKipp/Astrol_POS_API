using AutoMapper;
using AstrolPOSAPI.Application.Features.Purchasing.PaymentVoucher.DTOs;
using AstrolPOSAPI.Application.Features.Purchasing.Invoice.DTOs;
using AstrolPOSAPI.Domain.Entities.Purchasing;

namespace AstrolPOSAPI.Application.Features.Purchasing.PaymentVoucher
{
    public class PaymentVoucherMappingProfile : Profile
    {
        public PaymentVoucherMappingProfile()
        {
            CreateMap<Domain.Entities.Purchasing.PaymentVoucher, PaymentVoucherDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));
            CreateMap<CreatePaymentVoucherDto, Domain.Entities.Purchasing.PaymentVoucher>();

            // Posted Purchase Invoice mappings
            CreateMap<PurchInvHeader, PostedPurchInvHeaderDto>();
            CreateMap<PurchInvLine, PostedPurchInvLineDto>();
        }
    }
}

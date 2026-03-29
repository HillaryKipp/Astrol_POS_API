using AstrolPOSAPI.Application.Features.POS.Sales.Commands.CreateSalesOrder;
using AstrolPOSAPI.Application.Features.POS.Sales.DTOs;
using AstrolPOSAPI.Domain.Entities.POS;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.POS.Sales
{
    public class SalesMappingProfile : Profile
    {
        public SalesMappingProfile()
        {
            CreateMap<SalesOrder, SalesOrderDto>()
                .ForMember(dest => dest.CashierName, opt => opt.MapFrom(src => src.Cashier != null ? src.Cashier.Name : null))
                .ForMember(dest => dest.DrawerName, opt => opt.MapFrom(src => src.Drawer != null ? src.Drawer.Name : null));

            CreateMap<SalesOrderLine, SalesOrderLineDto>();
            CreateMap<Payment, PaymentDto>();
            CreateMap<Receipt, ReceiptDto>();

            CreateMap<CreateSalesOrderCommand, SalesOrder>();
            CreateMap<CreateSalesOrderDto, SalesOrder>();
        }
    }
}

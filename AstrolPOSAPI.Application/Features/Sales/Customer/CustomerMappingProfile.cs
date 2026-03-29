using AstrolPOSAPI.Application.Features.Sales.Customer.DTOs;
using AstrolPOSAPI.Domain.Entities.Accounting;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.Sales.Customer
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<AstrolPOSAPI.Domain.Entities.Accounting.Customer, CustomerDto>()
                .ForMember(dest => dest.CustomerPostingGroupCode, opt => opt.MapFrom(src => src.CustomerPostingGroup != null ? src.CustomerPostingGroup.Code : null))
                .ForMember(dest => dest.GenBusPostingGroupCode, opt => opt.MapFrom(src => src.GenBusPostingGroup != null ? src.GenBusPostingGroup.Code : null));

            CreateMap<CreateCustomerDto, AstrolPOSAPI.Domain.Entities.Accounting.Customer>();
            CreateMap<UpdateCustomerDto, AstrolPOSAPI.Domain.Entities.Accounting.Customer>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}

using AstrolPOSAPI.Application.Features.Accounting.PostingGroups.DTOs;
using AstrolPOSAPI.Domain.Entities.Accounting;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.Accounting.PostingGroups
{
    public class PostingGroupMappingProfile : Profile
    {
        public PostingGroupMappingProfile()
        {
            CreateMap<VendorPostingGroup, VendorPostingGroupDto>();
            CreateMap<CreateVendorPostingGroupDto, VendorPostingGroup>();
            CreateMap<UpdateVendorPostingGroupDto, VendorPostingGroup>();

            CreateMap<GenBusPostingGroup, GenBusPostingGroupDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Description));
            CreateMap<CreateGenBusPostingGroupDto, GenBusPostingGroup>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Name));
            CreateMap<UpdateGenBusPostingGroupDto, GenBusPostingGroup>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Name));

            CreateMap<CustomerPostingGroup, CustomerPostingGroupDto>().ReverseMap();
            CreateMap<CreateCustomerPostingGroupDto, CustomerPostingGroup>();
            CreateMap<UpdateCustomerPostingGroupDto, CustomerPostingGroup>();

            CreateMap<GenProdPostingGroup, GenProdPostingGroupDto>().ReverseMap();
            CreateMap<CreateGenProdPostingGroupDto, GenProdPostingGroup>();
            CreateMap<UpdateGenProdPostingGroupDto, GenProdPostingGroup>();

            CreateMap<GeneralPostingSetup, GeneralPostingSetupDto>().ReverseMap();
            CreateMap<CreateGeneralPostingSetupDto, GeneralPostingSetup>();
            CreateMap<UpdateGeneralPostingSetupDto, GeneralPostingSetup>();

            CreateMap<BankAccount, BankAccountDto>().ReverseMap();
            CreateMap<CreateBankAccountDto, BankAccount>();
            CreateMap<UpdateBankAccountDto, BankAccount>();
        }
    }
}

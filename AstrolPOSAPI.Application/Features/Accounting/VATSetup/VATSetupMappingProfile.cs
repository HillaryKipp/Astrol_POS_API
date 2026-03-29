using AutoMapper;
using AstrolPOSAPI.Application.Features.Accounting.VATSetup.DTOs;
using AstrolPOSAPI.Domain.Entities.Accounting;

namespace AstrolPOSAPI.Application.Features.Accounting.VATSetup
{
    public class VATSetupMappingProfile : Profile
    {
        public VATSetupMappingProfile()
        {
            CreateMap<VATPostingSetup, VATSetupDto>();
            CreateMap<CreateVATSetupDto, VATPostingSetup>();
        }
    }
}

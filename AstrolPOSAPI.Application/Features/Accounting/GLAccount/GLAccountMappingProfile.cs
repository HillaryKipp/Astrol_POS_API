using AstrolPOSAPI.Application.Features.Accounting.GLAccount.DTOs;
using AstrolPOSAPI.Domain.Entities.Accounting;
using AutoMapper;

namespace AstrolPOSAPI.Application.Features.Accounting.GLAccount
{
    public class GLAccountMappingProfile : Profile
    {
        public GLAccountMappingProfile()
        {
            CreateMap<Domain.Entities.Accounting.GLAccount, GLAccountDto>().ReverseMap();
            CreateMap<CreateGLAccountDto, Domain.Entities.Accounting.GLAccount>();
            CreateMap<UpdateGLAccountDto, Domain.Entities.Accounting.GLAccount>();
        }
    }
}

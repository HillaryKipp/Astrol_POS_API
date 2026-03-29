using AutoMapper;
using AstrolPOSAPI.Application.Features.Company.DTOs;

namespace AstrolPOSAPI.Application.Features.Company
{
    public class CompanyMappingProfile : Profile
    {
        public CompanyMappingProfile()
        {
            CreateMap<Domain.Entities.Core.Company, CompanyDto>();
            CreateMap<CreateCompanyDto, Domain.Entities.Core.Company>();
            CreateMap<UpdateCompanyDto, Domain.Entities.Core.Company>();
            CreateMap<Commands.CreateCompany.CreateCompanyCommand, Domain.Entities.Core.Company>();
            CreateMap<Commands.UpdateCompany.UpdateCompanyCommand, Domain.Entities.Core.Company>();
        }
    }
}

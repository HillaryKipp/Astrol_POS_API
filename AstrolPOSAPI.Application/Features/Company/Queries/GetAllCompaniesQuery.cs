using AutoMapper;
using AstrolPOSAPI.Application.Features.Company.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Company.Queries
{
    public class GetAllCompaniesQuery : IRequest<List<CompanyDto>>
    {
    }

    public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, List<CompanyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCompaniesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CompanyDto>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
        {
            var companies = await _unitOfWork.Repository<Domain.Entities.Core.Company>()
                .Entities
                .Where(c => c.DeletedDate == null)
                .OrderBy(c => c.Code)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<CompanyDto>>(companies);
        }
    }
}

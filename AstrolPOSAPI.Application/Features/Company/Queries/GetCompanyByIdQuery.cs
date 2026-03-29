using AutoMapper;
using AstrolPOSAPI.Application.Features.Company.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Company.Queries
{
    public class GetCompanyByIdQuery : IRequest<CompanyDto>
    {
        public string Id { get; set; } = default!;
    }

    public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCompanyByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.Company>()
                .Entities
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.DeletedDate == null, cancellationToken);

            if (company == null)
            {
                throw new KeyNotFoundException($"Company with ID {request.Id} not found");
            }

            return _mapper.Map<CompanyDto>(company);
        }
    }
}

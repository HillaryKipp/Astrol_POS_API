using AutoMapper;
using AstrolPOSAPI.Application.Features.Company.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Company.Commands.UpdateCompany
{
    public class UpdateCompanyCommand : IRequest<CompanyDto>
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }

    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, CompanyDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCompanyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CompanyDto> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.Company>()
                .Entities
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.DeletedDate == null, cancellationToken);

            if (company == null)
            {
                throw new KeyNotFoundException($"Company with ID {request.Id} not found");
            }

            company.Code = request.Code;
            company.Name = request.Name;
            company.Description = request.Description;

            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.Company>().UpdateAsync(company);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<CompanyDto>(company);
        }
    }
}

using AstrolPOSAPI.Application.Features.Company.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Core;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AstrolPOSAPI.Application.Features.Company.Commands.CreateCompany
{
    public class CreateCompanyCommand : IRequest<CompanyDto>
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }

    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CompanyDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCompanyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = _mapper.Map<AstrolPOSAPI.Domain.Entities.Core.Company>(request);
            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.Company>().AddAsync(company);
            await _unitOfWork.Save(cancellationToken);
            return _mapper.Map<CompanyDto>(company);
        }
    }
}

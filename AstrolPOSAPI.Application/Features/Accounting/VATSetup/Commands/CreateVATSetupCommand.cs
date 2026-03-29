using AstrolPOSAPI.Application.Features.Accounting.VATSetup.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Accounting;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Accounting.VATSetup.Commands
{
    public class CreateVATSetupCommand : IRequest<VATSetupDto>
    {
        public CreateVATSetupDto VATSetup { get; set; } = default!;
    }

    public class CreateVATSetupCommandHandler : IRequestHandler<CreateVATSetupCommand, VATSetupDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateVATSetupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<VATSetupDto> Handle(CreateVATSetupCommand request, CancellationToken cancellationToken)
        {
            // Check for duplicate code
            var exists = await _unitOfWork.Repository<VATPostingSetup>().Entities
                .AnyAsync(v => v.Code == request.VATSetup.Code && v.CompanyId == request.VATSetup.CompanyId && v.DeletedDate == null, cancellationToken);

            if (exists)
                throw new InvalidOperationException($"VAT Setup with code '{request.VATSetup.Code}' already exists.");

            var entity = _mapper.Map<VATPostingSetup>(request.VATSetup);
            entity.Id = Guid.NewGuid().ToString();

            await _unitOfWork.Repository<VATPostingSetup>().AddAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<VATSetupDto>(entity);
        }
    }
}

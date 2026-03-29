using AstrolPOSAPI.Application.Features.Accounting.VATSetup.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Accounting;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Accounting.VATSetup.Commands
{
    public class UpdateVATSetupCommand : IRequest<VATSetupDto>
    {
        public string Id { get; set; } = string.Empty;
        public UpdateVATSetupDto VATSetup { get; set; } = default!;
    }

    public class UpdateVATSetupCommandHandler : IRequestHandler<UpdateVATSetupCommand, VATSetupDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateVATSetupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<VATSetupDto> Handle(UpdateVATSetupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<VATPostingSetup>().Entities
                .FirstOrDefaultAsync(v => v.Id == request.Id && v.DeletedDate == null, cancellationToken);

            if (entity == null)
                throw new KeyNotFoundException($"VAT Setup with ID '{request.Id}' not found.");

            entity.Code = request.VATSetup.Code;
            entity.Description = request.VATSetup.Description;
            entity.VATPercentage = request.VATSetup.VATPercentage;
            entity.VATAccountCode = request.VATSetup.VATAccountCode;

            await _unitOfWork.Repository<VATPostingSetup>().UpdateAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<VATSetupDto>(entity);
        }
    }
}

using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Accounting;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Accounting.VATSetup.Commands
{
    public class DeleteVATSetupCommand : IRequest<Unit>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class DeleteVATSetupCommandHandler : IRequestHandler<DeleteVATSetupCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteVATSetupCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteVATSetupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<VATPostingSetup>().Entities
                .FirstOrDefaultAsync(v => v.Id == request.Id && v.DeletedDate == null, cancellationToken);

            if (entity == null)
                throw new KeyNotFoundException($"VAT Setup with ID '{request.Id}' not found.");

            await _unitOfWork.Repository<VATPostingSetup>().DeleteAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return Unit.Value;
        }
    }
}

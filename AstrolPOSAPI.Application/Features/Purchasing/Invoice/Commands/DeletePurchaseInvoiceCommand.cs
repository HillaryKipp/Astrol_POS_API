using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Purchasing;
using MediatR;

namespace AstrolPOSAPI.Application.Features.Purchasing.Invoice.Commands
{
    public class DeletePurchaseInvoiceCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class DeletePurchaseInvoiceCommandHandler : IRequestHandler<DeletePurchaseInvoiceCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePurchaseInvoiceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeletePurchaseInvoiceCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<PurchaseHeader>();
            var header = await repo.GetByIdAsync(request.Id);

            if (header == null)
                return false;

            if (header.Status == PurchaseStatus.Posted)
                throw new InvalidOperationException("Cannot delete a posted purchase invoice.");

            await repo.DeleteAsync(header);
            await _unitOfWork.Save(cancellationToken);

            return true;
        }
    }
}


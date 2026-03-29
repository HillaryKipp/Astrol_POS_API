using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.POS;
using FluentValidation;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.Sales.Commands.VoidSale
{
    public class VoidSaleCommand : IRequest<bool>
    {
        public string SalesOrderId { get; set; } = default!;
        public string? Reason { get; set; }
    }

    public class VoidSaleCommandValidator : AbstractValidator<VoidSaleCommand>
    {
        public VoidSaleCommandValidator()
        {
            RuleFor(p => p.SalesOrderId).NotEmpty();
        }
    }

    public class VoidSaleCommandHandler : IRequestHandler<VoidSaleCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public VoidSaleCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(VoidSaleCommand request, CancellationToken cancellationToken)
        {
            // Get the sales order
            var salesOrder = await _unitOfWork.Repository<SalesOrder>().GetByIdAsync(request.SalesOrderId);
            if (salesOrder == null)
                throw new KeyNotFoundException($"Sales order with ID {request.SalesOrderId} not found");

            if (salesOrder.Status == SalesOrderStatus.Voided)
                throw new InvalidOperationException("Sales order is already voided");

            if (salesOrder.Status == SalesOrderStatus.Completed)
                throw new InvalidOperationException("Cannot void a completed sale. Use refund instead.");

            // Update status to voided
            salesOrder.Status = SalesOrderStatus.Voided;
            await _unitOfWork.Repository<SalesOrder>().UpdateAsync(salesOrder);

            // Cancel any pending payments
            var allPayments = await _unitOfWork.Repository<Payment>().GetAllAsync();
            var pendingPayments = allPayments.Where(p =>
                p.SalesOrderId == request.SalesOrderId &&
                p.Status == PaymentStatus.Pending).ToList();

            foreach (var payment in pendingPayments)
            {
                payment.Status = PaymentStatus.Cancelled;
                payment.ResponseMessage = $"Sale voided. Reason: {request.Reason ?? "No reason provided"}";
                await _unitOfWork.Repository<Payment>().UpdateAsync(payment);
            }

            await _unitOfWork.Save(cancellationToken);

            return true;
        }
    }
}

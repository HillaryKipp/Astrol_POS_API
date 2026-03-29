using AstrolPOSAPI.Application.Features.POS.Sales.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.POS;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.Sales.Commands.RemoveItemFromSale
{
    public class RemoveItemFromSaleCommand : IRequest<SalesOrderDto>
    {
        public string SalesOrderId { get; set; } = default!;
        public string LineId { get; set; } = default!;
    }

    public class RemoveItemFromSaleCommandValidator : AbstractValidator<RemoveItemFromSaleCommand>
    {
        public RemoveItemFromSaleCommandValidator()
        {
            RuleFor(p => p.SalesOrderId).NotEmpty();
            RuleFor(p => p.LineId).NotEmpty();
        }
    }

    public class RemoveItemFromSaleCommandHandler : IRequestHandler<RemoveItemFromSaleCommand, SalesOrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RemoveItemFromSaleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SalesOrderDto> Handle(RemoveItemFromSaleCommand request, CancellationToken cancellationToken)
        {
            // Get the sales order
            var salesOrder = await _unitOfWork.Repository<SalesOrder>().GetByIdAsync(request.SalesOrderId);
            if (salesOrder == null || salesOrder.Status != SalesOrderStatus.Pending)
                throw new InvalidOperationException("Sales order not found or not in pending status");

            // Get the line
            var line = await _unitOfWork.Repository<SalesOrderLine>().GetByIdAsync(request.LineId);
            if (line == null || line.SalesOrderId != request.SalesOrderId)
                throw new KeyNotFoundException($"Line with ID {request.LineId} not found in this order");

            // Soft delete the line
            line.DeletedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<SalesOrderLine>().UpdateAsync(line);

            // Recalculate order totals
            await RecalculateOrderTotals(salesOrder);

            await _unitOfWork.Save(cancellationToken);

            return await GetSalesOrderWithDetails(salesOrder.Id);
        }

        private async Task RecalculateOrderTotals(SalesOrder order)
        {
            var allLines = await _unitOfWork.Repository<SalesOrderLine>().GetAllAsync();
            var orderLines = allLines.Where(l => l.SalesOrderId == order.Id && l.DeletedDate == null).ToList();

            order.Subtotal = orderLines.Sum(l => l.Quantity * l.UnitPrice);
            order.DiscountAmount = orderLines.Sum(l => l.DiscountAmount);
            order.TaxAmount = orderLines.Sum(l => l.TaxAmount);
            order.TotalAmount = order.Subtotal - order.DiscountAmount + order.TaxAmount;

            await _unitOfWork.Repository<SalesOrder>().UpdateAsync(order);
        }

        private async Task<SalesOrderDto> GetSalesOrderWithDetails(string orderId)
        {
            var order = await _unitOfWork.Repository<SalesOrder>().GetByIdAsync(orderId);
            var allLines = await _unitOfWork.Repository<SalesOrderLine>().GetAllAsync();
            var allPayments = await _unitOfWork.Repository<Payment>().GetAllAsync();

            var dto = _mapper.Map<SalesOrderDto>(order);
            dto.Lines = _mapper.Map<List<SalesOrderLineDto>>(
                allLines.Where(l => l.SalesOrderId == orderId && l.DeletedDate == null).ToList());
            dto.Payments = _mapper.Map<List<PaymentDto>>(
                allPayments.Where(p => p.SalesOrderId == orderId && p.DeletedDate == null).ToList());

            return dto;
        }
    }
}

using AstrolPOSAPI.Application.Features.POS.Sales.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.POS;
using AutoMapper;
using FluentValidation;
using MediatR;
using ItemEntity = AstrolPOSAPI.Domain.Entities.POS.Item;

namespace AstrolPOSAPI.Application.Features.POS.Sales.Commands.AddItemToSale
{
    public class AddItemToSaleCommand : IRequest<SalesOrderDto>
    {
        public string SalesOrderId { get; set; } = default!;
        public string ItemId { get; set; } = default!;
        public decimal Quantity { get; set; }
        public decimal DiscountAmount { get; set; }
    }

    public class AddItemToSaleCommandValidator : AbstractValidator<AddItemToSaleCommand>
    {
        public AddItemToSaleCommandValidator()
        {
            RuleFor(p => p.SalesOrderId).NotEmpty();
            RuleFor(p => p.ItemId).NotEmpty();
            RuleFor(p => p.Quantity).GreaterThan(0);
        }
    }

    public class AddItemToSaleCommandHandler : IRequestHandler<AddItemToSaleCommand, SalesOrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddItemToSaleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SalesOrderDto> Handle(AddItemToSaleCommand request, CancellationToken cancellationToken)
        {
            // Get the sales order
            var salesOrder = await _unitOfWork.Repository<SalesOrder>().GetByIdAsync(request.SalesOrderId);
            if (salesOrder == null || salesOrder.Status != SalesOrderStatus.Pending)
                throw new InvalidOperationException("Sales order not found or not in pending status");

            // Get the item
            var item = await _unitOfWork.Repository<ItemEntity>().GetByIdAsync(request.ItemId);
            if (item == null || item.DeletedDate != null || !item.IsActive)
                throw new KeyNotFoundException($"Item with ID {request.ItemId} not found or inactive");

            // Check stock availability
            if (item.QuantityOnHand < request.Quantity)
                throw new InvalidOperationException($"Insufficient stock. Available: {item.QuantityOnHand}, Requested: {request.Quantity}");

            // Calculate line totals
            var lineSubtotal = request.Quantity * item.UnitPrice;
            var taxAmount = (lineSubtotal - request.DiscountAmount) * (item.TaxRate / 100);
            var lineTotal = lineSubtotal - request.DiscountAmount + taxAmount;

            // Check if item already exists in order
            var existingLines = await _unitOfWork.Repository<SalesOrderLine>().GetAllAsync();
            var existingLine = existingLines.FirstOrDefault(l =>
                l.SalesOrderId == request.SalesOrderId &&
                l.ItemId == request.ItemId &&
                l.DeletedDate == null);

            if (existingLine != null)
            {
                // Update existing line
                existingLine.Quantity += request.Quantity;
                existingLine.DiscountAmount += request.DiscountAmount;
                existingLine.TaxAmount = (existingLine.Quantity * existingLine.UnitPrice - existingLine.DiscountAmount) * (existingLine.TaxRate / 100);
                existingLine.LineTotal = existingLine.Quantity * existingLine.UnitPrice - existingLine.DiscountAmount + existingLine.TaxAmount;
                await _unitOfWork.Repository<SalesOrderLine>().UpdateAsync(existingLine);
            }
            else
            {
                // Create new line
                var line = new SalesOrderLine
                {
                    SalesOrderId = request.SalesOrderId,
                    ItemId = item.Id,
                    ItemCode = item.Code,
                    ItemName = item.Name,
                    UnitOfMeasure = item.UnitOfMeasure,
                    Quantity = request.Quantity,
                    UnitPrice = item.UnitPrice,
                    DiscountAmount = request.DiscountAmount,
                    TaxRate = item.TaxRate,
                    TaxAmount = taxAmount,
                    LineTotal = lineTotal
                };
                await _unitOfWork.Repository<SalesOrderLine>().AddAsync(line);
            }

            // Recalculate order totals
            await RecalculateOrderTotals(salesOrder, cancellationToken);

            await _unitOfWork.Save(cancellationToken);

            // Reload order with lines
            return await GetSalesOrderWithDetails(salesOrder.Id);
        }

        private async Task RecalculateOrderTotals(SalesOrder order, CancellationToken cancellationToken)
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

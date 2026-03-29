using AstrolPOSAPI.Application.Features.POS.Sales.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.POS;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.Sales.Queries
{
    public class GetOpenSalesByDrawerQuery : IRequest<SalesOrderDto?>
    {
        public string DrawerId { get; set; } = default!;
        public string? CashierId { get; set; }
    }

    public class GetOpenSalesByDrawerQueryHandler : IRequestHandler<GetOpenSalesByDrawerQuery, SalesOrderDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOpenSalesByDrawerQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SalesOrderDto?> Handle(GetOpenSalesByDrawerQuery request, CancellationToken cancellationToken)
        {
            var allOrders = await _unitOfWork.Repository<SalesOrder>().GetAllAsync();
            var query = allOrders.AsQueryable()
                .Where(o => o.DrawerId == request.DrawerId &&
                           o.Status == SalesOrderStatus.Pending &&
                           o.DeletedDate == null);

            if (!string.IsNullOrEmpty(request.CashierId))
                query = query.Where(o => o.CashierId == request.CashierId);

            var order = query.OrderByDescending(o => o.OrderDate).FirstOrDefault();

            if (order == null)
                return null;

            var allLines = await _unitOfWork.Repository<SalesOrderLine>().GetAllAsync();
            var allPayments = await _unitOfWork.Repository<Payment>().GetAllAsync();

            var dto = _mapper.Map<SalesOrderDto>(order);
            dto.Lines = _mapper.Map<List<SalesOrderLineDto>>(
                allLines.Where(l => l.SalesOrderId == order.Id && l.DeletedDate == null).ToList());
            dto.Payments = _mapper.Map<List<PaymentDto>>(
                allPayments.Where(p => p.SalesOrderId == order.Id && p.DeletedDate == null).ToList());

            return dto;
        }
    }
}

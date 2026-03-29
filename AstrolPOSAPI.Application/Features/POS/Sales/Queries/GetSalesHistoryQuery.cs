using AstrolPOSAPI.Application.Features.POS.Sales.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.POS;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.POS.Sales.Queries
{
    public class GetSalesHistoryQuery : IRequest<List<SalesOrderDto>>
    {
        public string? CompanyId { get; set; }
        public string? StoreOfOperationId { get; set; }
        public string? DrawerId { get; set; }
        public string? CashierId { get; set; }
        public SalesOrderStatus? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetSalesHistoryQueryHandler : IRequestHandler<GetSalesHistoryQuery, List<SalesOrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSalesHistoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<SalesOrderDto>> Handle(GetSalesHistoryQuery request, CancellationToken cancellationToken)
        {
            // Use IQueryable with .Include() to eagerly load related data
            var query = _unitOfWork.Repository<SalesOrder>().Entities
                .Include(o => o.Lines)
                .Include(o => o.Payments)
                .Include(o => o.Cashier)
                .Include(o => o.Drawer)
                .Where(o => o.DeletedDate == null);

            if (!string.IsNullOrEmpty(request.CompanyId))
                query = query.Where(o => o.CompanyId == request.CompanyId);

            if (!string.IsNullOrEmpty(request.StoreOfOperationId))
                query = query.Where(o => o.StoreOfOperationId == request.StoreOfOperationId);

            if (!string.IsNullOrEmpty(request.DrawerId))
                query = query.Where(o => o.DrawerId == request.DrawerId);

            if (!string.IsNullOrEmpty(request.CashierId))
                query = query.Where(o => o.CashierId == request.CashierId);

            if (request.Status.HasValue)
                query = query.Where(o => o.Status == request.Status.Value);

            if (request.FromDate.HasValue)
                query = query.Where(o => o.OrderDate >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                query = query.Where(o => o.OrderDate <= request.ToDate.Value);

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<SalesOrderDto>>(orders);
        }
    }
}


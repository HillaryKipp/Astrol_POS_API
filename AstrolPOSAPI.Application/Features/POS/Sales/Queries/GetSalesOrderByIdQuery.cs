using AstrolPOSAPI.Application.Features.POS.Sales.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.POS;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.POS.Sales.Queries
{
    public class GetSalesOrderByIdQuery : IRequest<SalesOrderDto>
    {
        public string Id { get; set; } = default!;
    }

    public class GetSalesOrderByIdQueryHandler : IRequestHandler<GetSalesOrderByIdQuery, SalesOrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSalesOrderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SalesOrderDto> Handle(GetSalesOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Repository<SalesOrder>().Entities
                .Include(o => o.Lines)
                .Include(o => o.Payments)
                .Include(o => o.Cashier)
                .Include(o => o.Drawer)
                .FirstOrDefaultAsync(o => o.Id == request.Id && o.DeletedDate == null, cancellationToken);

            if (order == null)
                throw new KeyNotFoundException($"Sales order with ID {request.Id} not found");

            return _mapper.Map<SalesOrderDto>(order);
        }
    }
}


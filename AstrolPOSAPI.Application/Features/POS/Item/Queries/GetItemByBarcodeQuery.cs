using AstrolPOSAPI.Application.Features.POS.Item.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.POS.Item.Queries
{
    public class GetItemByBarcodeQuery : IRequest<ItemDto>
    {
        public string Barcode { get; set; } = default!;
        public string? CompanyId { get; set; }
        public string? StoreOfOperationId { get; set; }
    }

    public class GetItemByBarcodeQueryHandler : IRequestHandler<GetItemByBarcodeQuery, ItemDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetItemByBarcodeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ItemDto> Handle(GetItemByBarcodeQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.Item>().Entities
                .Where(i => i.Barcode == request.Barcode && i.DeletedDate == null && i.IsActive);

            if (!string.IsNullOrEmpty(request.CompanyId))
                query = query.Where(i => i.CompanyId == request.CompanyId);

            if (!string.IsNullOrEmpty(request.StoreOfOperationId))
                query = query.Where(i => i.StoreOfOperationId == request.StoreOfOperationId);

            var item = await query
                .Include(i => i.Category)
                .FirstOrDefaultAsync(cancellationToken);

            if (item == null)
                throw new KeyNotFoundException($"No active item found with barcode '{request.Barcode}'");

            return _mapper.Map<ItemDto>(item);
        }
    }
}

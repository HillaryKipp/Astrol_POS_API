using AstrolPOSAPI.Application.Features.POS.Item.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ItemEntity = AstrolPOSAPI.Domain.Entities.POS.Item;

namespace AstrolPOSAPI.Application.Features.POS.Item.Queries
{
    public class GetAllItemsQuery : IRequest<List<ItemDto>>
    {
        public string? CompanyId { get; set; }
        public string? StoreOfOperationId { get; set; }
        public string? CategoryId { get; set; }
        public bool? IsActive { get; set; }
        public string? SearchTerm { get; set; }
    }

    public class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, List<ItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllItemsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ItemDto>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.Repository<ItemEntity>().GetAllAsync();
            var query = items.AsQueryable().Where(x => x.DeletedDate == null);

            if (!string.IsNullOrEmpty(request.CompanyId))
                query = query.Where(x => x.CompanyId == request.CompanyId);

            if (!string.IsNullOrEmpty(request.StoreOfOperationId))
                query = query.Where(x => x.StoreOfOperationId == request.StoreOfOperationId);

            if (!string.IsNullOrEmpty(request.CategoryId))
                query = query.Where(x => x.CategoryId == request.CategoryId);

            if (request.IsActive.HasValue)
                query = query.Where(x => x.IsActive == request.IsActive.Value);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchLower = request.SearchTerm.ToLower();
                query = query.Where(x =>
                    x.Code.ToLower().Contains(searchLower) ||
                    x.Name.ToLower().Contains(searchLower) ||
                    (x.Barcode != null && x.Barcode.ToLower().Contains(searchLower)));
            }

            return _mapper.Map<List<ItemDto>>(query.OrderBy(x => x.Name).ToList());
        }
    }
}

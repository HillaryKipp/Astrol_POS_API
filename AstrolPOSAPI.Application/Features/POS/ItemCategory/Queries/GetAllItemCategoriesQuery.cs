using AstrolPOSAPI.Application.Features.POS.ItemCategory.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.POS;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.POS.ItemCategory.Queries
{
    public class GetAllItemCategoriesQuery : IRequest<IEnumerable<ItemCategoryDto>>
    {
        public string? CompanyId { get; set; }
        public string? StoreOfOperationId { get; set; }
    }

    public class GetAllItemCategoriesQueryHandler : IRequestHandler<GetAllItemCategoriesQuery, IEnumerable<ItemCategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllItemCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemCategoryDto>> Handle(GetAllItemCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.ItemCategory>().Entities
                .Include(x => x.Company)
                .Include(x => x.StoreOfOperation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.CompanyId))
                query = query.Where(x => x.CompanyId == request.CompanyId);

            if (!string.IsNullOrEmpty(request.StoreOfOperationId))
                query = query.Where(x => x.StoreOfOperationId == request.StoreOfOperationId);


            var list = await query.ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ItemCategoryDto>>(list);
        }
    }
}

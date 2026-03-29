using AstrolPOSAPI.Application.Features.POS.ItemCategory.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.POS.ItemCategory.Queries
{
    public class GetItemCategoryByIdQuery : IRequest<ItemCategoryDto>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class GetItemCategoryByIdQueryHandler : IRequestHandler<GetItemCategoryByIdQuery, ItemCategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetItemCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ItemCategoryDto> Handle(GetItemCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Repository<Domain.Entities.POS.ItemCategory>().Entities
                .Include(x => x.Company)
                .Include(x => x.StoreOfOperation)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (category == null)
            {
                throw new KeyNotFoundException($"Item Category with ID {request.Id} not found.");
            }

            return _mapper.Map<ItemCategoryDto>(category);
        }
    }
}

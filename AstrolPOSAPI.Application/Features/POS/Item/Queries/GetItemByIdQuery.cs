using AstrolPOSAPI.Application.Features.POS.Item.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using ItemEntity = AstrolPOSAPI.Domain.Entities.POS.Item;

namespace AstrolPOSAPI.Application.Features.POS.Item.Queries
{
    public class GetItemByIdQuery : IRequest<ItemDto>
    {
        public string Id { get; set; } = default!;
    }

    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetItemByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ItemDto> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.Repository<ItemEntity>().GetByIdAsync(request.Id);

            if (item == null || item.DeletedDate != null)
                throw new KeyNotFoundException($"Item with ID {request.Id} not found");

            return _mapper.Map<ItemDto>(item);
        }
    }
}

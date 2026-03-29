using AstrolPOSAPI.Application.Features.POS.ItemCategory.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.POS;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.ItemCategory.Commands.CreateItemCategory
{
    public class CreateItemCategoryCommand : CreateItemCategoryDto, IRequest<ItemCategoryDto>
    {
    }

    public class CreateItemCategoryCommandHandler : IRequestHandler<CreateItemCategoryCommand, ItemCategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateItemCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ItemCategoryDto> Handle(CreateItemCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<AstrolPOSAPI.Domain.Entities.POS.ItemCategory>(request);
            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.ItemCategory>().AddAsync(category);
            await _unitOfWork.Save(cancellationToken);
            return _mapper.Map<ItemCategoryDto>(category);
        }
    }
}

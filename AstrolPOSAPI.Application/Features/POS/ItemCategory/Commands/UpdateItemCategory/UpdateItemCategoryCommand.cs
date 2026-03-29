using AstrolPOSAPI.Application.Features.POS.ItemCategory.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.POS;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.ItemCategory.Commands.UpdateItemCategory
{
    public class UpdateItemCategoryCommand : UpdateItemCategoryDto, IRequest<ItemCategoryDto>
    {
    }

    public class UpdateItemCategoryCommandHandler : IRequestHandler<UpdateItemCategoryCommand, ItemCategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateItemCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ItemCategoryDto> Handle(UpdateItemCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.ItemCategory>().GetByIdAsync(request.Id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Item Category with ID {request.Id} not found.");
            }

            _mapper.Map(request, category);
            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.ItemCategory>().UpdateAsync(category);
            await _unitOfWork.Save(cancellationToken);
            return _mapper.Map<ItemCategoryDto>(category);
        }
    }
}

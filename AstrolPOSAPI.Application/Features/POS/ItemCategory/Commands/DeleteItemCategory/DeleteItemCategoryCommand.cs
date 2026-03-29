using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.POS;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.ItemCategory.Commands.DeleteItemCategory
{
    public class DeleteItemCategoryCommand : IRequest
    {
        public string Id { get; set; } = default!;
    }

    public class DeleteItemCategoryCommandHandler : IRequestHandler<DeleteItemCategoryCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteItemCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteItemCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.ItemCategory>().GetByIdAsync(request.Id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Item Category with ID {request.Id} not found.");
            }

            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.ItemCategory>().DeleteAsync(category);
            await _unitOfWork.Save(cancellationToken);
        }
    }
}

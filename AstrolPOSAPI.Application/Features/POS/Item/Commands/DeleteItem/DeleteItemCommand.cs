using AstrolPOSAPI.Application.Interfaces.Repositories;
using FluentValidation;
using MediatR;
using ItemEntity = AstrolPOSAPI.Domain.Entities.POS.Item;

namespace AstrolPOSAPI.Application.Features.POS.Item.Commands.DeleteItem
{
    public class DeleteItemCommand : IRequest<bool>
    {
        public string Id { get; set; } = default!;
    }

    public class DeleteItemCommandValidator : AbstractValidator<DeleteItemCommand>
    {
        public DeleteItemCommandValidator()
        {
            RuleFor(p => p.Id).NotEmpty();
        }
    }

    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteItemCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.Repository<ItemEntity>().GetByIdAsync(request.Id);

            if (item == null || item.DeletedDate != null)
                throw new KeyNotFoundException($"Item with ID {request.Id} not found");

            // Soft delete
            item.DeletedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<ItemEntity>().UpdateAsync(item);
            await _unitOfWork.Save(cancellationToken);

            return true;
        }
    }
}

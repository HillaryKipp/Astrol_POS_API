using AstrolPOSAPI.Application.Interfaces.Repositories;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.TouchScreenButton.Commands.DeleteTouchScreenButton
{
    public class DeleteTouchScreenButtonCommand : IRequest<bool>
    {
        public string Id { get; set; } = default!;
    }

    public class DeleteTouchScreenButtonCommandHandler : IRequestHandler<DeleteTouchScreenButtonCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTouchScreenButtonCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteTouchScreenButtonCommand request, CancellationToken cancellationToken)
        {
            var button = await _unitOfWork.Repository<Domain.Entities.POS.TouchScreenButton>().GetByIdAsync(request.Id);

            if (button == null)
                throw new KeyNotFoundException($"TouchScreenButton with ID {request.Id} not found");

            button.DeletedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<Domain.Entities.POS.TouchScreenButton>().UpdateAsync(button);
            await _unitOfWork.Save(cancellationToken);

            return true;
        }
    }
}

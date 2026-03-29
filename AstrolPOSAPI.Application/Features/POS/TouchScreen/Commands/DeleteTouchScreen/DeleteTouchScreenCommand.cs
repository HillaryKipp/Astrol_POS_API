using AstrolPOSAPI.Application.Interfaces.Repositories;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.TouchScreen.Commands.DeleteTouchScreen
{
    public class DeleteTouchScreenCommand : IRequest<bool>
    {
        public string Id { get; set; } = default!;
    }

    public class DeleteTouchScreenCommandHandler : IRequestHandler<DeleteTouchScreenCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTouchScreenCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteTouchScreenCommand request, CancellationToken cancellationToken)
        {
            var touchScreen = await _unitOfWork.Repository<Domain.Entities.POS.TouchScreen>().GetByIdAsync(request.Id);

            if (touchScreen == null)
                throw new KeyNotFoundException($"TouchScreen with ID {request.Id} not found");

            touchScreen.DeletedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<Domain.Entities.POS.TouchScreen>().UpdateAsync(touchScreen);
            await _unitOfWork.Save(cancellationToken);

            return true;
        }
    }
}

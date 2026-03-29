using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.POS;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.DefaultScreen.Commands.DeleteDefaultScreen
{
    public class DeleteDefaultScreenCommand : IRequest<bool>
    {
        public string Id { get; set; } = default!;
    }

    public class DeleteDefaultScreenCommandHandler : IRequestHandler<DeleteDefaultScreenCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDefaultScreenCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteDefaultScreenCommand request, CancellationToken cancellationToken)
        {
            var defaultScreen = await _unitOfWork.Repository<Domain.Entities.POS.DefaultScreen>().GetByIdAsync(request.Id);

            if (defaultScreen == null)
                throw new KeyNotFoundException($"DefaultScreen with ID {request.Id} not found");

            defaultScreen.DeletedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.DefaultScreen>().UpdateAsync(defaultScreen);
            await _unitOfWork.Save(cancellationToken);

            return true;
        }
    }
}

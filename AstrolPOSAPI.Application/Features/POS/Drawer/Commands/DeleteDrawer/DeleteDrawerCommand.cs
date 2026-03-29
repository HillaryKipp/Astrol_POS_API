using AstrolPOSAPI.Application.Interfaces.Repositories;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.Drawer.Commands.DeleteDrawer
{
    public class DeleteDrawerCommand : IRequest<bool>
    {
        public string Id { get; set; } = default!;
    }

    public class DeleteDrawerCommandHandler : IRequestHandler<DeleteDrawerCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDrawerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteDrawerCommand request, CancellationToken cancellationToken)
        {
            var drawer = await _unitOfWork.Repository<Domain.Entities.POS.Drawer>().GetByIdAsync(request.Id);

            if (drawer == null)
                throw new KeyNotFoundException($"Drawer with ID {request.Id} not found");

            drawer.DeletedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<Domain.Entities.POS.Drawer>().UpdateAsync(drawer);
            await _unitOfWork.Save(cancellationToken);

            return true;
        }
    }
}

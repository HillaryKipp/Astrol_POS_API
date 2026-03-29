using AstrolPOSAPI.Application.Interfaces.Repositories;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.DrawerGroup.Commands.DeleteDrawerGroup
{
    public class DeleteDrawerGroupCommand : IRequest<bool>
    {
        public string Id { get; set; } = default!;
    }

    public class DeleteDrawerGroupCommandHandler : IRequestHandler<DeleteDrawerGroupCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDrawerGroupCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteDrawerGroupCommand request, CancellationToken cancellationToken)
        {
            var drawerGroup = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.DrawerGroup>().GetByIdAsync(request.Id);

            if (drawerGroup == null)
                throw new KeyNotFoundException($"DrawerGroup with ID {request.Id} not found");

            // Soft delete
            drawerGroup.DeletedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.DrawerGroup>().UpdateAsync(drawerGroup);
            await _unitOfWork.Save(cancellationToken);

            return true;
        }
    }
}

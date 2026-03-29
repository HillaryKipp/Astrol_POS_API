using AstrolPOSAPI.Application.Interfaces.Repositories;
using MediatR;

namespace AstrolPOSAPI.Application.Features.Permission.Commands
{
    public class DeletePermissionCommand : IRequest<Unit>
    {
        public string Id { get; set; } = default!;
    }

    public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePermissionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Identity.Permission>()
                .GetByIdAsync(request.Id);

            if (permission == null)
                throw new KeyNotFoundException($"Permission with ID {request.Id} not found");

            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Identity.Permission>().DeleteAsync(permission);
            await _unitOfWork.Save(cancellationToken);

            return Unit.Value;
        }
    }
}

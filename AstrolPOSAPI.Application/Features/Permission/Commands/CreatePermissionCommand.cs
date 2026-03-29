using AstrolPOSAPI.Application.Features.Permission.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.Permission.Commands
{
    public class CreatePermissionCommand : IRequest<PermissionDto>
    {
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
        public string ResourceName { get; set; } = default!;
        public bool CanWrite { get; set; }
        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanEdit { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
    }

    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, PermissionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePermissionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PermissionDto> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            // Validate: Must have either UserId or RoleId
            if (string.IsNullOrEmpty(request.UserId) && string.IsNullOrEmpty(request.RoleId))
                throw new InvalidOperationException("Permission must be assigned to either a User or a Role");

            var permission = new AstrolPOSAPI.Domain.Entities.Identity.Permission
            {
                UserId = request.UserId,
                RoleId = request.RoleId,
                ResourceName = request.ResourceName,
                CanWrite = request.CanWrite,
                CanRead = request.CanRead,
                CanEdit = request.CanEdit,
                CanDelete = request.CanDelete
            };

            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Identity.Permission>().AddAsync(permission);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<PermissionDto>(permission);
        }
    }
}

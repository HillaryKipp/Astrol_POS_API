using AstrolPOSAPI.Application.Features.Permission.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.Permission.Queries
{
    public class GetPermissionsByUserIdQuery : IRequest<List<PermissionDto>>
    {
        public string UserId { get; set; } = default!;
    }

    public class GetPermissionsByUserIdQueryHandler : IRequestHandler<GetPermissionsByUserIdQuery, List<PermissionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPermissionsByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<PermissionDto>> Handle(GetPermissionsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var allPermissions = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Identity.Permission>().GetAllAsync();
            var permissions = allPermissions.Where(p => p.UserId == request.UserId).ToList();

            return _mapper.Map<List<PermissionDto>>(permissions);
        }
    }
}

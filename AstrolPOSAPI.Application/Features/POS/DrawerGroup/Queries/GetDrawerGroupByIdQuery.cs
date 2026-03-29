using AstrolPOSAPI.Application.Features.POS.DrawerGroup.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.DrawerGroup.Queries
{
    public class GetDrawerGroupByIdQuery : IRequest<DrawerGroupDto>
    {
        public string Id { get; set; } = default!;
    }

    public class GetDrawerGroupByIdQueryHandler : IRequestHandler<GetDrawerGroupByIdQuery, DrawerGroupDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDrawerGroupByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DrawerGroupDto> Handle(GetDrawerGroupByIdQuery request, CancellationToken cancellationToken)
        {
            var drawerGroup = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.DrawerGroup>().GetByIdAsync(request.Id);

            if (drawerGroup == null || drawerGroup.DeletedDate != null)
                throw new KeyNotFoundException($"DrawerGroup with ID {request.Id} not found");

            return _mapper.Map<DrawerGroupDto>(drawerGroup);
        }
    }
}

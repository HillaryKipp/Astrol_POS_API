using AstrolPOSAPI.Application.Features.POS.AssignedDrawer.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.POS.AssignedDrawer.Queries
{
    public class GetAllAssignedDrawersQuery : IRequest<List<AssignedDrawerDto>>
    {
        public string? CompanyId { get; set; }
        public string? StoreOfOperationId { get; set; }
        public string? UserId { get; set; }
        public string? DrawerId { get; set; }
    }

    public class GetAllAssignedDrawersQueryHandler : IRequestHandler<GetAllAssignedDrawersQuery, List<AssignedDrawerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAssignedDrawersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<AssignedDrawerDto>> Handle(GetAllAssignedDrawersQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Domain.Entities.POS.AssignedDrawer>().Entities
                .Include(x => x.Company)
                .Include(x => x.StoreOfOperation)
                .Include(x => x.Drawer)
                .Include(x => x.DefaultScreen)
                .Include(x => x.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.CompanyId))
                query = query.Where(x => x.CompanyId == request.CompanyId);

            if (!string.IsNullOrEmpty(request.StoreOfOperationId))
                query = query.Where(x => x.StoreOfOperationId == request.StoreOfOperationId);

            if (!string.IsNullOrEmpty(request.UserId))
                query = query.Where(x => x.UserId == request.UserId);

            if (!string.IsNullOrEmpty(request.DrawerId))
                query = query.Where(x => x.DrawerId == request.DrawerId);

            var list = await query.ToListAsync(cancellationToken);
            return _mapper.Map<List<AssignedDrawerDto>>(list);
        }
    }
}

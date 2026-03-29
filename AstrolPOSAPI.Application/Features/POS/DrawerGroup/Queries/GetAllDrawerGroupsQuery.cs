using AstrolPOSAPI.Application.Features.POS.DrawerGroup.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.POS.DrawerGroup.Queries
{
    public class GetAllDrawerGroupsQuery : IRequest<List<DrawerGroupDto>>
    {
        public string? CompanyId { get; set; }
        public string? StoreOfOperationId { get; set; }
    }

    public class GetAllDrawerGroupsQueryHandler : IRequestHandler<GetAllDrawerGroupsQuery, List<DrawerGroupDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllDrawerGroupsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<DrawerGroupDto>> Handle(GetAllDrawerGroupsQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.DrawerGroup>().Entities
                .Include(x => x.Company)
                .Include(x => x.StoreOfOperation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.CompanyId))
            {
                query = query.Where(dg => dg.CompanyId == request.CompanyId);
            }

            if (!string.IsNullOrEmpty(request.StoreOfOperationId))
            {
                query = query.Where(dg => dg.StoreOfOperationId == request.StoreOfOperationId);
            }

            var list = await query.ToListAsync(cancellationToken);
            return _mapper.Map<List<DrawerGroupDto>>(list);
        }
    }
}

using AstrolPOSAPI.Application.Features.POS.TouchScreen.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;    

namespace AstrolPOSAPI.Application.Features.POS.TouchScreen.Queries
{
    public class GetAllTouchScreensQuery : IRequest<List<TouchScreenDto>>
    {
        public string? CompanyId { get; set; }
        public string? StoreOfOperationId { get; set; }
        public bool IncludeButtons { get; set; } = false;
    }

    public class GetAllTouchScreensQueryHandler : IRequestHandler<GetAllTouchScreensQuery, List<TouchScreenDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllTouchScreensQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<TouchScreenDto>> Handle(GetAllTouchScreensQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Domain.Entities.POS.TouchScreen>().Entities
                .Include(x => x.Company)
                .Include(x => x.StoreOfOperation)
                .AsQueryable();

            if (request.IncludeButtons)
                query = query.Include(x => x.Buttons);

            if (!string.IsNullOrEmpty(request.CompanyId))
                query = query.Where(x => x.CompanyId == request.CompanyId);

            if (!string.IsNullOrEmpty(request.StoreOfOperationId))
                query = query.Where(x => x.StoreOfOperationId == request.StoreOfOperationId);

            var list = await query.ToListAsync(cancellationToken);
            return _mapper.Map<List<TouchScreenDto>>(list);
        }
    }
}

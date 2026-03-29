using AstrolPOSAPI.Application.Features.POS.TouchScreenButton.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.TouchScreenButton.Queries
{
    public class GetAllTouchScreenButtonsQuery : IRequest<List<TouchScreenButtonDto>>
    {
        public string? TouchScreenId { get; set; }
        public string? CompanyId { get; set; }
        public string? StoreOfOperationId { get; set; }
    }

    public class GetAllTouchScreenButtonsQueryHandler : IRequestHandler<GetAllTouchScreenButtonsQuery, List<TouchScreenButtonDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllTouchScreenButtonsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<TouchScreenButtonDto>> Handle(GetAllTouchScreenButtonsQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.TouchScreenButton>().GetAllAsync();
            var query = items.AsQueryable();

            if (!string.IsNullOrEmpty(request.TouchScreenId))
                query = query.Where(x => x.TouchScreenId == request.TouchScreenId);

            if (!string.IsNullOrEmpty(request.CompanyId))
                query = query.Where(x => x.CompanyId == request.CompanyId);

            if (!string.IsNullOrEmpty(request.StoreOfOperationId))
                query = query.Where(x => x.StoreOfOperationId == request.StoreOfOperationId);

            return _mapper.Map<List<TouchScreenButtonDto>>(query.OrderBy(x => x.Row).ThenBy(x => x.Column).ThenBy(x => x.SortOrder).ToList());
        }
    }
}

using AstrolPOSAPI.Application.Features.Store.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Store.Queries
{
    public class GetAllStoresQuery : IRequest<List<StoreDto>>
    {
        public string? CompanyId { get; set; }
    }

    public class GetAllStoresQueryHandler : IRequestHandler<GetAllStoresQuery, List<StoreDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllStoresQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<StoreDto>> Handle(GetAllStoresQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.Store>().GetAllAsync();

            var stores = await query;

            // Filter by CompanyId if provided
            var filteredStores = stores;
            if (!string.IsNullOrEmpty(request.CompanyId))
            {
                filteredStores = stores.Where(s => s.CompanyId == request.CompanyId).ToList();
            }

            return _mapper.Map<List<StoreDto>>(filteredStores);
        }
    }
}

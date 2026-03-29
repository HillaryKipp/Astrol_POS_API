using AstrolPOSAPI.Application.Features.Purchasing.Vendor.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Purchasing.Vendor.Queries
{
    public class GetAllVendorsQuery : IRequest<List<VendorDto>>
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetAllVendorsQueryHandler : IRequestHandler<GetAllVendorsQuery, List<VendorDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllVendorsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<VendorDto>> Handle(GetAllVendorsQuery request, CancellationToken cancellationToken)
        {
            var vendors = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Purchasing.Vendor>().Entities
                .Where(v => v.CompanyId == request.CompanyId && v.DeletedDate == null)
                .Include(v => v.VendorPostingGroup)
                .Include(v => v.GenBusPostingGroup)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<VendorDto>>(vendors);
        }
    }
}

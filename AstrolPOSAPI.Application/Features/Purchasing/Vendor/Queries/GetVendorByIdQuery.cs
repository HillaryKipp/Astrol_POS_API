using AstrolPOSAPI.Application.Features.Purchasing.Vendor.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Purchasing.Vendor.Queries
{
    public class GetVendorByIdQuery : IRequest<VendorDto>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class GetVendorByIdQueryHandler : IRequestHandler<GetVendorByIdQuery, VendorDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetVendorByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<VendorDto> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
        {
            var vendor = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Purchasing.Vendor>().Entities
                .Include(v => v.VendorPostingGroup)
                .Include(v => v.GenBusPostingGroup)
                .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

            if (vendor == null)
                throw new KeyNotFoundException($"Vendor with ID {request.Id} not found");

            return _mapper.Map<VendorDto>(vendor);
        }
    }
}

using System;
using System.Threading.Tasks;
using AstrolPOSAPI.Application.Features.Purchasing.Vendor.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Application.Interfaces.Services;
using AstrolPOSAPI.Domain.Entities.Purchasing;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Purchasing.Vendor.Commands
{
    public class CreateVendorCommand : IRequest<VendorDto>
    {
        public CreateVendorDto Vendor { get; set; } = default!;
    }

    public class CreateVendorCommandHandler : IRequestHandler<CreateVendorCommand, VendorDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INoSeriesService _noSeriesService;

        public CreateVendorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, INoSeriesService noSeriesService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _noSeriesService = noSeriesService;
        }

        public async Task<VendorDto> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
        {
            // Auto-generate Vendor Number
            var vendorNo = await _noSeriesService.GenerateNextNumberAsync("VENDOR", cancellationToken);

            var vendor = _mapper.Map<AstrolPOSAPI.Domain.Entities.Purchasing.Vendor>(request.Vendor);
            vendor.No = vendorNo;
            vendor.Id = Guid.NewGuid().ToString();

            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Purchasing.Vendor>().AddAsync(vendor);
            await _unitOfWork.Save(cancellationToken);

            // Fetch again to include navigation properties for the DTO
            var result = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Purchasing.Vendor>().Entities
                .Include(v => v.VendorPostingGroup)
                .Include(v => v.GenBusPostingGroup)
                .FirstOrDefaultAsync(v => v.Id == vendor.Id, cancellationToken);

            return _mapper.Map<VendorDto>(result);
        }
    }
}

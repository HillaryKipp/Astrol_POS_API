using AstrolPOSAPI.Application.Features.Purchasing.Vendor.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Purchasing.Vendor.Commands
{
    public class UpdateVendorCommand : IRequest<VendorDto>
    {
        public UpdateVendorDto Vendor { get; set; } = default!;
    }

    public class UpdateVendorCommandHandler : IRequestHandler<UpdateVendorCommand, VendorDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateVendorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<VendorDto> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
        {
            var vendor = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Purchasing.Vendor>().GetByIdAsync(request.Vendor.Id);
            if (vendor == null)
                throw new KeyNotFoundException($"Vendor with ID {request.Vendor.Id} not found");

            _mapper.Map(request.Vendor, vendor);
            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Purchasing.Vendor>().UpdateAsync(vendor);
            await _unitOfWork.Save(cancellationToken);

            var result = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Purchasing.Vendor>().Entities
                .Include(v => v.VendorPostingGroup)
                .Include(v => v.GenBusPostingGroup)
                .FirstOrDefaultAsync(v => v.Id == vendor.Id, cancellationToken);

            return _mapper.Map<VendorDto>(result);
        }
    }
}

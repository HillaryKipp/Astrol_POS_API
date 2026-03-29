using AstrolPOSAPI.Application.Features.Purchasing.Invoice.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Application.Interfaces.Services;
using AstrolPOSAPI.Domain.Entities.Purchasing;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Purchasing.Invoice.Commands
{
    public class CreatePurchaseInvoiceCommand : IRequest<PurchaseHeaderDto>
    {
        public CreatePurchaseHeaderDto Invoice { get; set; } = default!;
    }

    public class CreatePurchaseInvoiceCommandHandler : IRequestHandler<CreatePurchaseInvoiceCommand, PurchaseHeaderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INoSeriesService _noSeriesService;

        public CreatePurchaseInvoiceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, INoSeriesService noSeriesService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _noSeriesService = noSeriesService;
        }

        public async Task<PurchaseHeaderDto> Handle(CreatePurchaseInvoiceCommand request, CancellationToken cancellationToken)
        {
            // Fetch Vendor name
            var vendor = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Purchasing.Vendor>().Entities
                .FirstOrDefaultAsync(v => v.No == request.Invoice.BuyFromVendorNo && v.CompanyId == request.Invoice.CompanyId, cancellationToken);

            if (vendor == null)
                throw new InvalidOperationException($"Vendor {request.Invoice.BuyFromVendorNo} not found.");

            // Check for unique VendorInvoiceNo
            if (!string.IsNullOrEmpty(request.Invoice.VendorInvoiceNo))
            {
                var exists = await _unitOfWork.Repository<PurchaseHeader>().Entities
                    .AnyAsync(h => h.VendorInvoiceNo == request.Invoice.VendorInvoiceNo && h.CompanyId == request.Invoice.CompanyId && h.DeletedDate == null, cancellationToken);
                
                if (exists)
                    throw new InvalidOperationException($"Vendor Invoice No {request.Invoice.VendorInvoiceNo} already exists for this company.");
            }

            // Generate Invoice No
            var invoiceNo = await _noSeriesService.GenerateNextNumberAsync("PURCH_INV", cancellationToken);

            var header = _mapper.Map<PurchaseHeader>(request.Invoice);
            header.No = invoiceNo;
            header.BuyFromVendorName = vendor.Name;
            header.Id = Guid.NewGuid().ToString();
            header.Status = PurchaseStatus.Open;

            // Calculate line amounts
            foreach (var line in header.Lines)
            {
                line.Id = Guid.NewGuid().ToString();
                line.LineAmount = line.Quantity * line.DirectUnitCost;
            }

            await _unitOfWork.Repository<PurchaseHeader>().AddAsync(header);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<PurchaseHeaderDto>(header);
        }
    }
}

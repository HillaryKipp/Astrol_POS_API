using AstrolPOSAPI.Application.Features.Purchasing.Invoice.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Purchasing;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Purchasing.Invoice.Commands
{
    public class UpdatePurchaseInvoiceCommand : IRequest<PurchaseHeaderDto>
    {
        public string Id { get; set; } = string.Empty;
        public CreatePurchaseHeaderDto Invoice { get; set; } = default!;
    }

    public class UpdatePurchaseInvoiceCommandHandler : IRequestHandler<UpdatePurchaseInvoiceCommand, PurchaseHeaderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdatePurchaseInvoiceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PurchaseHeaderDto> Handle(UpdatePurchaseInvoiceCommand request, CancellationToken cancellationToken)
        {
            var headerRepo = _unitOfWork.Repository<PurchaseHeader>();

            var header = await headerRepo.Entities
                .Include(h => h.Lines)
                .FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);

            if (header == null)
                throw new KeyNotFoundException($"Purchase invoice with ID {request.Id} not found.");

            if (header.Status == PurchaseStatus.Posted)
                throw new InvalidOperationException("Cannot update a posted purchase invoice.");

            // Fetch Vendor to keep name in sync
            var vendor = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Purchasing.Vendor>().Entities
                .FirstOrDefaultAsync(v => v.No == request.Invoice.BuyFromVendorNo && v.CompanyId == request.Invoice.CompanyId, cancellationToken);    

            if (vendor == null)
                throw new InvalidOperationException($"Vendor {request.Invoice.BuyFromVendorNo} not found.");

            // Update header fields
            header.BuyFromVendorNo = request.Invoice.BuyFromVendorNo;
            header.BuyFromVendorName = vendor.Name;
            header.PostingDate = request.Invoice.PostingDate;
            header.DocumentDate = request.Invoice.DocumentDate;
            header.VendorInvoiceNo = request.Invoice.VendorInvoiceNo;
            header.CompanyId = request.Invoice.CompanyId;

            // Replace lines with the incoming ones
            header.Lines.Clear();
            foreach (var lineDto in request.Invoice.Lines)
            {
                var line = _mapper.Map<PurchaseLine>(lineDto);
                line.Id = Guid.NewGuid().ToString();
                line.LineAmount = line.Quantity * line.DirectUnitCost;
                header.Lines.Add(line);
            }

            await headerRepo.UpdateAsync(header);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<PurchaseHeaderDto>(header);
        }
    }
}


using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Accounting;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Accounting.Queries
{
    public class GetVendorLedgerEntriesQuery : IRequest<List<VendorLedgerEntry>>
    {
        public string CompanyId { get; set; } = string.Empty;
        public string? VendorNo { get; set; }
        public bool? OpenOnly { get; set; }
    }

    public class GetVendorLedgerEntriesQueryHandler : IRequestHandler<GetVendorLedgerEntriesQuery, List<VendorLedgerEntry>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetVendorLedgerEntriesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<VendorLedgerEntry>> Handle(GetVendorLedgerEntriesQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<VendorLedgerEntry>().Entities
                .Where(e => e.CompanyId == request.CompanyId && e.DeletedDate == null);

            if (!string.IsNullOrEmpty(request.VendorNo))
                query = query.Where(e => e.VendorNo == request.VendorNo);

            if (request.OpenOnly.HasValue)
                query = query.Where(e => e.Open == request.OpenOnly.Value);

            return await query.OrderByDescending(e => e.PostingDate).ToListAsync(cancellationToken);
        }
    }
}

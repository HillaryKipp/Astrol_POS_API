using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AstrolPOSAPI.Application.Features.Purchasing.Invoice.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Purchasing;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Purchasing.Invoice.Queries
{
    public class GetAllPurchaseInvoicesQuery : IRequest<List<PurchaseHeaderDto>>
    {
        public string CompanyId { get; set; } = string.Empty;
        public bool IncludePosted { get; set; }
    }

    public class GetAllPurchaseInvoicesQueryHandler : IRequestHandler<GetAllPurchaseInvoicesQuery, List<PurchaseHeaderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllPurchaseInvoicesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<PurchaseHeaderDto>> Handle(GetAllPurchaseInvoicesQuery request, CancellationToken cancellationToken)
        {
            var results = new List<PurchaseHeaderDto>();

            // Unposted
            var unposted = await _unitOfWork.Repository<PurchaseHeader>().Entities
                .Where(h => h.CompanyId == request.CompanyId && h.DeletedDate == null)
                .Include(h => h.Lines)
                .ToListAsync(cancellationToken);

            results.AddRange(_mapper.Map<List<PurchaseHeaderDto>>(unposted));

            if (request.IncludePosted)
            {
                var posted = await _unitOfWork.Repository<PurchInvHeader>().Entities
                    .Where(h => h.CompanyId == request.CompanyId && h.DeletedDate == null)
                    .Include(h => h.Lines)
                    .ToListAsync(cancellationToken);

                results.AddRange(_mapper.Map<List<PurchaseHeaderDto>>(posted));
            }

            return results.OrderByDescending(h => h.PostingDate).ToList();
        }
    }
}

using System.Threading.Tasks;
using AstrolPOSAPI.Application.Features.Purchasing.Invoice.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Purchasing;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Purchasing.Invoice.Queries
{
    public class GetPurchaseInvoiceQuery : IRequest<PurchaseHeaderDto>
    {
        public string Id { get; set; } = string.Empty;
        public bool IsPosted { get; set; }
    }

    public class GetPurchaseInvoiceQueryHandler : IRequestHandler<GetPurchaseInvoiceQuery, PurchaseHeaderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPurchaseInvoiceQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PurchaseHeaderDto> Handle(GetPurchaseInvoiceQuery request, CancellationToken cancellationToken)
        {
            if (request.IsPosted)
            {
                var posted = await _unitOfWork.Repository<PurchInvHeader>().Entities
                    .Include(h => h.Lines)
                    .FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);

                return _mapper.Map<PurchaseHeaderDto>(posted);
            }
            else
            {
                var unposted = await _unitOfWork.Repository<PurchaseHeader>().Entities
                    .Include(h => h.Lines)
                    .FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);

                return _mapper.Map<PurchaseHeaderDto>(unposted);
            }
        }
    }
}

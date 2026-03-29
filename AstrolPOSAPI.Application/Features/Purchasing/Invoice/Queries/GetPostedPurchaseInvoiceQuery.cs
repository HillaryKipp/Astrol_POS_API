using AstrolPOSAPI.Application.Features.Purchasing.Invoice.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Purchasing;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Purchasing.Invoice.Queries
{
    public class GetPostedPurchaseInvoiceQuery : IRequest<PostedPurchInvHeaderDto>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class GetPostedPurchaseInvoiceQueryHandler : IRequestHandler<GetPostedPurchaseInvoiceQuery, PostedPurchInvHeaderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPostedPurchaseInvoiceQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PostedPurchInvHeaderDto> Handle(GetPostedPurchaseInvoiceQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<PurchInvHeader>().Entities
                .Include(h => h.Lines)
                .FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);

            if (entity == null)
                throw new KeyNotFoundException($"Posted Purchase Invoice with ID '{request.Id}' not found.");

            var dto = _mapper.Map<PostedPurchInvHeaderDto>(entity);
            dto.TotalAmount = entity.Lines.Sum(l => l.LineAmount);

            return dto;
        }
    }
}

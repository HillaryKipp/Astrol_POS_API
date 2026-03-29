using AstrolPOSAPI.Application.Features.Purchasing.PaymentVoucher.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Purchasing.PaymentVoucher.Queries
{
    public class GetAllPaymentVouchersQuery : IRequest<List<PaymentVoucherDto>>
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetAllPaymentVouchersQueryHandler : IRequestHandler<GetAllPaymentVouchersQuery, List<PaymentVoucherDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllPaymentVouchersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<PaymentVoucherDto>> Handle(GetAllPaymentVouchersQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.Repository<Domain.Entities.Purchasing.PaymentVoucher>().Entities
                .Where(p => p.CompanyId == request.CompanyId && p.DeletedDate == null)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<PaymentVoucherDto>>(entities);
        }
    }
}

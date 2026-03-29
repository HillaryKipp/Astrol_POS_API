using AstrolPOSAPI.Application.Features.Purchasing.PaymentVoucher.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Purchasing.PaymentVoucher.Queries
{
    public class GetPaymentVoucherQuery : IRequest<PaymentVoucherDto>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class GetPaymentVoucherQueryHandler : IRequestHandler<GetPaymentVoucherQuery, PaymentVoucherDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPaymentVoucherQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaymentVoucherDto> Handle(GetPaymentVoucherQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<Domain.Entities.Purchasing.PaymentVoucher>().Entities
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.DeletedDate == null, cancellationToken);

            if (entity == null)
                throw new KeyNotFoundException($"Payment Voucher with ID '{request.Id}' not found.");

            return _mapper.Map<PaymentVoucherDto>(entity);
        }
    }
}

using AstrolPOSAPI.Application.Features.Purchasing.PaymentVoucher.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Application.Interfaces.Services;
using AstrolPOSAPI.Domain.Entities.Accounting;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Purchasing.PaymentVoucher.Commands
{
    public class CreatePaymentVoucherCommand : IRequest<PaymentVoucherDto>
    {
        public CreatePaymentVoucherDto PaymentVoucher { get; set; } = default!;
    }

    public class CreatePaymentVoucherCommandHandler : IRequestHandler<CreatePaymentVoucherCommand, PaymentVoucherDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INoSeriesService _noSeriesService;

        public CreatePaymentVoucherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, INoSeriesService noSeriesService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _noSeriesService = noSeriesService;
        }

        public async Task<PaymentVoucherDto> Handle(CreatePaymentVoucherCommand request, CancellationToken cancellationToken)
        {
            var dto = request.PaymentVoucher;

            // Validate Vendor
            var vendor = await _unitOfWork.Repository<Domain.Entities.Purchasing.Vendor>().Entities
                .FirstOrDefaultAsync(v => v.No == dto.VendorNo && v.CompanyId == dto.CompanyId, cancellationToken);
            if (vendor == null)
                throw new InvalidOperationException($"Vendor '{dto.VendorNo}' not found.");

            if (dto.Amount <= 0)
                throw new InvalidOperationException("Payment Amount must be greater than zero.");

            // Validate Bank Account
            var bank = await _unitOfWork.Repository<BankAccount>().Entities
                .FirstOrDefaultAsync(b => b.Code == dto.BankAccountCode && b.CompanyId == dto.CompanyId, cancellationToken);
            if (bank == null)
                throw new InvalidOperationException($"Bank Account '{dto.BankAccountCode}' not found.");

            // Generate PV Number
            var pvNo = await _noSeriesService.GenerateNextNumberAsync("PV", cancellationToken);

            var entity = _mapper.Map<Domain.Entities.Purchasing.PaymentVoucher>(dto);
            entity.Id = Guid.NewGuid().ToString();
            entity.No = pvNo;
            entity.VendorName = vendor.Name;
            entity.BankAccountName = bank.Name;
            entity.Status = Domain.Entities.Purchasing.PaymentVoucherStatus.Open;

            await _unitOfWork.Repository<Domain.Entities.Purchasing.PaymentVoucher>().AddAsync(entity);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<PaymentVoucherDto>(entity);
        }
    }
}

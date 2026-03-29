using AstrolPOSAPI.Application.Features.POS.Sales.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Application.Interfaces.Services;
using AstrolPOSAPI.Domain.Entities.POS;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.Sales.Commands.ProcessPayment
{
    public class ProcessPaymentCommand : IRequest<PaymentDto>
    {
        public string SalesOrderId { get; set; } = default!;
        public PaymentMethod PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ReferenceNo { get; set; }
        public string? CardLastFour { get; set; }

        /// <summary>
        /// If true, initiates M-Pesa STK Push. If false, assumes manual reference entry.
        /// </summary>
        public bool InitiateStkPush { get; set; } = true;
    }

    public class ProcessPaymentCommandValidator : AbstractValidator<ProcessPaymentCommand>
    {
        public ProcessPaymentCommandValidator()
        {
            RuleFor(p => p.SalesOrderId).NotEmpty();
            RuleFor(p => p.Amount).GreaterThan(0);

            // M-Pesa requires phone number
            When(p => p.PaymentMethod == PaymentMethod.Mpesa, () =>
            {
                RuleFor(p => p.PhoneNumber).NotEmpty().WithMessage("Phone number is required for M-Pesa payments");
            });
        }
    }

    public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, PaymentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMpesaService? _mpesaService;

        public ProcessPaymentCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMpesaService? mpesaService = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mpesaService = mpesaService;
        }

        public async Task<PaymentDto> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            // Get the sales order
            var salesOrder = await _unitOfWork.Repository<SalesOrder>().GetByIdAsync(request.SalesOrderId);
            if (salesOrder == null || salesOrder.Status != SalesOrderStatus.Pending)
                throw new InvalidOperationException("Sales order not found or not in pending status");

            // Calculate remaining amount
            var allPayments = await _unitOfWork.Repository<Payment>().GetAllAsync();
            var existingPayments = allPayments
                .Where(p => p.SalesOrderId == request.SalesOrderId &&
                           p.DeletedDate == null &&
                           p.Status == PaymentStatus.Completed)
                .Sum(p => p.Amount);

            var remainingAmount = salesOrder.TotalAmount - existingPayments;

            //if (request.Amount > remainingAmount && request.PaymentMethod != PaymentMethod.Cash)
            //{
            //    // Only cash can be overpaid (for change)
            //    throw new InvalidOperationException($"Payment amount exceeds remaining balance. Remaining: {remainingAmount:C}");
            //}

            // Process payment based on method
            var payment = new Payment
            {
                SalesOrderId = request.SalesOrderId,
                PaymentMethod = request.PaymentMethod,
                Amount = request.Amount,
                PhoneNumber = request.PhoneNumber,
                ReferenceNo = request.ReferenceNo,
                CardLastFour = request.CardLastFour,
                TransactionDate = DateTime.UtcNow,
                Status = PaymentStatus.Pending
            };

            switch (request.PaymentMethod)
            {
                case PaymentMethod.Cash:
                    // Cash payments are immediately completed
                    payment.Status = PaymentStatus.Completed;
                    payment.ResponseMessage = "Cash payment received";
                    break;

                case PaymentMethod.Mpesa:
                    await ProcessMpesaPayment(payment, salesOrder, request);
                    break;

                case PaymentMethod.Visa:
                case PaymentMethod.Mastercard:
                    // Card payments - require manual reference entry
                    if (!string.IsNullOrEmpty(request.ReferenceNo))
                    {
                        payment.Status = PaymentStatus.Completed;
                        payment.ResponseMessage = $"Card payment authorized. Auth: {request.ReferenceNo}";
                    }
                    else
                    {
                        payment.Status = PaymentStatus.Pending;
                        payment.ResponseMessage = "Awaiting card authorization";
                    }
                    break;

                case PaymentMethod.BankTransfer:
                    payment.Status = PaymentStatus.Completed;
                    payment.ResponseMessage = "Bank transfer recorded";
                    break;

                case PaymentMethod.CreditAccount:
                    payment.Status = PaymentStatus.Completed;
                    payment.ResponseMessage = "Amount charged to customer account";
                    break;
            }

            await _unitOfWork.Repository<Payment>().AddAsync(payment);

            // Update sales order amounts
            if (payment.Status == PaymentStatus.Completed)
            {
                salesOrder.AmountPaid += payment.Amount;
                if (request.PaymentMethod == PaymentMethod.Cash && payment.Amount > remainingAmount)
                {
                    salesOrder.ChangeGiven = payment.Amount - remainingAmount;
                }
                await _unitOfWork.Repository<SalesOrder>().UpdateAsync(salesOrder);
            }

            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<PaymentDto>(payment);
        }

        private async Task ProcessMpesaPayment(Payment payment, SalesOrder salesOrder, ProcessPaymentCommand request)
        {
            // If manual reference provided, complete immediately
            if (!string.IsNullOrEmpty(request.ReferenceNo))
            {
                payment.Status = PaymentStatus.Completed;
                payment.ResponseMessage = $"M-Pesa payment confirmed. Reference: {request.ReferenceNo}";
                return;
            }

            // Check if M-Pesa service is available
            if (_mpesaService == null || !request.InitiateStkPush)
            {
                payment.Status = PaymentStatus.Pending;
                payment.ResponseMessage = "Awaiting M-Pesa confirmation (manual entry required)";
                return;
            }

            // Initiate real STK Push
            var result = await _mpesaService.InitiateStkPushAsync(
                request.PhoneNumber!,
                request.Amount,
                salesOrder.OrderNo,
                $"Payment for {salesOrder.OrderNo}");

            if (result.Success)
            {
                // Store CheckoutRequestID for callback matching
                payment.ReferenceNo = result.CheckoutRequestId;
                payment.Status = PaymentStatus.Pending;
                payment.ResponseMessage = result.Message ?? "STK Push sent to customer phone";
            }
            else
            {
                payment.Status = PaymentStatus.Failed;
                payment.ResponseMessage = result.Message ?? "Failed to initiate M-Pesa STK Push";
                throw new InvalidOperationException(payment.ResponseMessage);
            }
        }
    }
}


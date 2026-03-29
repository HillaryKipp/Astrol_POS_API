using AstrolPOSAPI.Application.Features.OTP.DTOs;
using AstrolPOSAPI.Application.Interfaces.Infrastructure;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.OTP.Commands
{
    public class SendOTPCommand : IRequest<OTPDto>
    {
        public string PhoneNumber { get; set; } = default!;
        public string Purpose { get; set; } = default!;
    }

    public class SendOTPCommandHandler : IRequestHandler<SendOTPCommand, OTPDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISmsSender _smsSender;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;

        public SendOTPCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ISmsSender smsSender, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _smsSender = smsSender;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OTPDto> Handle(SendOTPCommand request, CancellationToken cancellationToken)
        {
            // Get current user ID
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                         ?? _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

            // Generate 6-digit OTP using a cryptographically secure RNG
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            var value = BitConverter.ToUInt32(bytes, 0);
            var otpCode = (value % 900000 + 100000).ToString();

            var otp = new AstrolPOSAPI.Domain.Entities.Identity.OTP
            {
                PhoneNumber = request.PhoneNumber,
                OTPCode = otpCode,
                Purpose = request.Purpose,
                CreatedDate = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5), // OTP expires in 5 minutes
                IsVerified = false,
                VerificationAttempts = 0,
                UserId = userId ?? string.Empty,
                CreatedBy = userId // Populate CreatedBy with userId

            };

            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Identity.OTP>().AddAsync(otp);
            await _unitOfWork.Save(cancellationToken);

            // Send SMS
            var message = $"Your OTP code for Astrol POS is: {otpCode}. It expires in 5 minutes.";
            await _smsSender.SendSmsAsync(request.PhoneNumber, message);

            return _mapper.Map<OTPDto>(otp);
        }
    }
}

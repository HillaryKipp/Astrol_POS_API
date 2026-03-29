using AstrolPOSAPI.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace AstrolPOSAPI.Application.Features.OTP.Commands
{
    public class VerifyOTPCommand : IRequest<bool>
    {
        public string PhoneNumber { get; set; } = default!;
        public string OTPCode { get; set; } = default!;
        public string Purpose { get; set; } = default!;
    }

    public class VerifyOTPCommandHandler : IRequestHandler<VerifyOTPCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public VerifyOTPCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(VerifyOTPCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Identity.OTP>();
            var otp = await repo.Entities
                .Where(o => o.PhoneNumber == request.PhoneNumber && o.Purpose == request.Purpose && !o.IsVerified)
                .OrderByDescending(o => o.CreatedDate)
                .FirstOrDefaultAsync(cancellationToken);

            if (otp == null)
                return false;

            const int maxAttempts = 5;
            if (otp.VerificationAttempts >= maxAttempts)
                return false;

            if (DateTime.UtcNow > otp.ExpiresAt)
                return false;

            var storedBytes = Encoding.UTF8.GetBytes(otp.OTPCode ?? string.Empty);
            var providedBytes = Encoding.UTF8.GetBytes(request.OTPCode ?? string.Empty);
            bool codesMatch = CryptographicOperations.FixedTimeEquals(storedBytes, providedBytes);

            if (!codesMatch)
            {
                otp.VerificationAttempts++;
                await repo.UpdateAsync(otp);
                await _unitOfWork.Save(cancellationToken);
                return false;
            }

            otp.IsVerified = true;
            otp.VerifiedAt = DateTime.UtcNow;
            await repo.UpdateAsync(otp);
            await _unitOfWork.Save(cancellationToken);

            return true;
        }
    }
}

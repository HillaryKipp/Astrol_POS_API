using AstrolPOSAPI.Application.Features.GeneralSettings.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.GeneralSettings.Commands.CreateGeneralSettings
{
    public class CreateGeneralSettingsCommand : IRequest<GeneralSettingsDto>
    {
        public string CompanyId { get; set; } = default!;
        public string? LogoUrl { get; set; }
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? TertiaryColor { get; set; }
        public bool HasOtp { get; set; }
        public string? Currency { get; set; }
        public string? Timezone { get; set; }
    }

    public class CreateGeneralSettingsCommandHandler : IRequestHandler<CreateGeneralSettingsCommand, GeneralSettingsDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateGeneralSettingsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GeneralSettingsDto> Handle(CreateGeneralSettingsCommand request, CancellationToken cancellationToken)
        {
            // Validate Company exists
            var company = await _unitOfWork.Repository<Domain.Entities.Core.Company>().GetByIdAsync(request.CompanyId);
            if (company == null)
                throw new InvalidOperationException($"Company with ID {request.CompanyId} not found");

            var settings = new Domain.Entities.Core.GeneralSettings
            {
                CompanyId = request.CompanyId,
                LogoUrl = request.LogoUrl,
                PrimaryColor = request.PrimaryColor ?? "#007bff",
                SecondaryColor = request.SecondaryColor ?? "#6c757d",
                TertiaryColor = request.TertiaryColor ?? "#28a745",
                HasOtp = request.HasOtp,
                Currency = (request.Currency ?? "KES").ToUpperInvariant(),
                CurrencySymbol = ((request.Currency ?? "KES").ToUpperInvariant()) switch
                {
                    "KES" => "KES",
                    "USD" => "$",
                    "EUR" => "€",
                    _ => (request.Currency ?? "KES").ToUpperInvariant()
                },
                Timezone = request.Timezone ?? "UTC",
                EnableInventory = true,
                EnablePOS = true,
                EnableReporting = true
            };

            await _unitOfWork.Repository<Domain.Entities.Core.GeneralSettings>().AddAsync(settings);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<GeneralSettingsDto>(settings);
        }
    }
}

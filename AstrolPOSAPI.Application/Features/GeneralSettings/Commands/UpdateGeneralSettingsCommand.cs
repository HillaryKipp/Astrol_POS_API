using AstrolPOSAPI.Application.Features.GeneralSettings.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.GeneralSettings.Commands.UpdateGeneralSettings
{
    public class UpdateGeneralSettingsCommand : IRequest<GeneralSettingsDto>
    {
        public string Id { get; set; } = default!;
        public string? LogoUrl { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanySlogan { get; set; }
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? TertiaryColor { get; set; }
        public string? AccentColor { get; set; }
        public string? BackgroundColor { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public bool HasOtp { get; set; }
        public bool EnableInventory { get; set; }
        public bool EnablePOS { get; set; }
        public bool EnableReporting { get; set; }
        public string? Currency { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? DateFormat { get; set; }
        public string? TimeFormat { get; set; }
        public string? Timezone { get; set; }
        public string? TaxNumber { get; set; }
        public decimal? DefaultTaxRate { get; set; }
        public string? ReceiptFooter { get; set; }
        public string? SupportEmail { get; set; }
        public string? SupportPhone { get; set; }
    }

    public class UpdateGeneralSettingsCommandHandler : IRequestHandler<UpdateGeneralSettingsCommand, GeneralSettingsDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateGeneralSettingsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GeneralSettingsDto> Handle(UpdateGeneralSettingsCommand request, CancellationToken cancellationToken)
        {
            var settings = await _unitOfWork.Repository<Domain.Entities.Core.GeneralSettings>()
                .GetByIdAsync(request.Id);

            if (settings == null)
                throw new KeyNotFoundException($"GeneralSettings with ID {request.Id} not found");

            // Update properties
            settings.LogoUrl = request.LogoUrl ?? settings.LogoUrl;
            settings.CompanyName = request.CompanyName ?? settings.CompanyName;
            settings.CompanySlogan = request.CompanySlogan ?? settings.CompanySlogan;
            settings.PrimaryColor = request.PrimaryColor ?? settings.PrimaryColor;
            settings.SecondaryColor = request.SecondaryColor ?? settings.SecondaryColor;
            settings.TertiaryColor = request.TertiaryColor ?? settings.TertiaryColor;
            settings.AccentColor = request.AccentColor ?? settings.AccentColor;
            settings.BackgroundColor = request.BackgroundColor ?? settings.BackgroundColor;
            settings.BackgroundImageUrl = request.BackgroundImageUrl ?? settings.BackgroundImageUrl;
            settings.HasOtp = request.HasOtp;
            settings.EnableInventory = request.EnableInventory;
            settings.EnablePOS = request.EnablePOS;
            settings.EnableReporting = request.EnableReporting;
            settings.Currency = (request.Currency ?? settings.Currency)?.ToUpperInvariant();
            settings.CurrencySymbol = string.IsNullOrWhiteSpace(request.CurrencySymbol)
                ? settings.CurrencySymbol
                : request.CurrencySymbol;
            settings.DateFormat = request.DateFormat ?? settings.DateFormat;
            settings.TimeFormat = request.TimeFormat ?? settings.TimeFormat;
            settings.Timezone = request.Timezone ?? settings.Timezone;
            settings.TaxNumber = request.TaxNumber ?? settings.TaxNumber;
            settings.DefaultTaxRate = request.DefaultTaxRate ?? settings.DefaultTaxRate;
            settings.ReceiptFooter = request.ReceiptFooter ?? settings.ReceiptFooter;
            settings.SupportEmail = request.SupportEmail ?? settings.SupportEmail;
            settings.SupportPhone = request.SupportPhone ?? settings.SupportPhone;

            await _unitOfWork.Repository<Domain.Entities.Core.GeneralSettings>().UpdateAsync(settings);
            
            // Sync Company Name if changed
            if (!string.IsNullOrWhiteSpace(request.CompanyName))
            {
                var company = await _unitOfWork.Repository<Domain.Entities.Core.Company>()
                    .GetByIdAsync(settings.CompanyId);
                
                if (company != null && company.Name != request.CompanyName)
                {
                    company.Name = request.CompanyName;
                    await _unitOfWork.Repository<Domain.Entities.Core.Company>().UpdateAsync(company);
                }
            }

            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<GeneralSettingsDto>(settings);
        }
    }
}

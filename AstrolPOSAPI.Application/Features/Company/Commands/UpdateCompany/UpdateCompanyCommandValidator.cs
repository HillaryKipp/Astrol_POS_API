using FluentValidation;

namespace AstrolPOSAPI.Application.Features.Company.Commands.UpdateCompany
{
    public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
    {
        public UpdateCompanyCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Valid company ID is required");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Company code is required")
                .MaximumLength(32).WithMessage("Company code must not exceed 32 characters");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Company name is required")
                .MaximumLength(256).WithMessage("Company name must not exceed 256 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}

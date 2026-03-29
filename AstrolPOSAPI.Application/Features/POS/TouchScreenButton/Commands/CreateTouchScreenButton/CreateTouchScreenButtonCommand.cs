using AstrolPOSAPI.Application.Features.POS.TouchScreen.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.POS;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.TouchScreenButton.Commands.CreateTouchScreenButton
{
    public class CreateTouchScreenButtonCommand : IRequest<TouchScreenButtonDto>
    {
        public string TouchScreenId { get; set; } = default!;
        public ButtonType ButtonType { get; set; }
        public string? ItemId { get; set; }
        public string ItemName { get; set; } = default!;
        public ButtonShape Shape { get; set; } = ButtonShape.Rectangle;
        public string BackgroundColor { get; set; } = "#FFFFFF";
        public string TextColor { get; set; } = "#000000";
        public int? FontSize { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int RowSpan { get; set; } = 1;
        public int ColumnSpan { get; set; } = 1;
        public bool ShowImage { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsDefaultImage { get; set; }
        public int SortOrder { get; set; }
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class CreateTouchScreenButtonCommandValidator : AbstractValidator<CreateTouchScreenButtonCommand>
    {
        public CreateTouchScreenButtonCommandValidator()
        {
            RuleFor(p => p.TouchScreenId).NotEmpty();
            RuleFor(p => p.ItemName).NotEmpty().MaximumLength(100);
            RuleFor(p => p.BackgroundColor).Matches(@"^#[0-9A-Fa-f]{6}$");
            RuleFor(p => p.TextColor).Matches(@"^#[0-9A-Fa-f]{6}$");
            RuleFor(p => p.FontSize).InclusiveBetween(8, 72).When(p => p.FontSize.HasValue);
            RuleFor(p => p.Row).GreaterThan(0);
            RuleFor(p => p.Column).GreaterThan(0);
            RuleFor(p => p.RowSpan).InclusiveBetween(1, 10);
            RuleFor(p => p.ColumnSpan).InclusiveBetween(1, 10);
            RuleFor(p => p.CompanyId).NotEmpty();
            RuleFor(p => p.StoreOfOperationId).NotEmpty();
        }
    }

    public class CreateTouchScreenButtonCommandHandler : IRequestHandler<CreateTouchScreenButtonCommand, TouchScreenButtonDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateTouchScreenButtonCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TouchScreenButtonDto> Handle(CreateTouchScreenButtonCommand request, CancellationToken cancellationToken)
        {
            var button = _mapper.Map<Domain.Entities.POS.TouchScreenButton>(request);
            await _unitOfWork.Repository<Domain.Entities.POS.TouchScreenButton>().AddAsync(button);
            await _unitOfWork.Save(cancellationToken);
            return _mapper.Map<TouchScreenButtonDto>(button);
        }
    }
}

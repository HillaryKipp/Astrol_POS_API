using AstrolPOSAPI.Application.Features.POS.TouchScreen.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.TouchScreen.Commands.CreateTouchScreen
{
    public class CreateTouchScreenCommand : IRequest<TouchScreenDto>
    {
        public string Code { get; set; } = default!;
        public string ScreenName { get; set; } = default!;
        public string? Description { get; set; }
        public int GridRows { get; set; } = 2;
        public int GridColumns { get; set; } = 2;
        public int DefaultFontSize { get; set; } = 12;
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class CreateTouchScreenCommandValidator : AbstractValidator<CreateTouchScreenCommand>
    {
        public CreateTouchScreenCommandValidator()
        {
            RuleFor(p => p.Code).NotEmpty().MaximumLength(32);
            RuleFor(p => p.ScreenName).NotEmpty().MaximumLength(100);
            RuleFor(p => p.Description).MaximumLength(500);
            RuleFor(p => p.GridRows).InclusiveBetween(1, 10);
            RuleFor(p => p.GridColumns).InclusiveBetween(1, 10);
            RuleFor(p => p.DefaultFontSize).InclusiveBetween(8, 72);
            RuleFor(p => p.CompanyId).NotEmpty();
            RuleFor(p => p.StoreOfOperationId).NotEmpty();
        }
    }

    public class CreateTouchScreenCommandHandler : IRequestHandler<CreateTouchScreenCommand, TouchScreenDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateTouchScreenCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TouchScreenDto> Handle(CreateTouchScreenCommand request, CancellationToken cancellationToken)
        {
            var touchScreen = _mapper.Map<Domain.Entities.POS.TouchScreen>(request);
            await _unitOfWork.Repository<Domain.Entities.POS.TouchScreen>().AddAsync(touchScreen);
            await _unitOfWork.Save(cancellationToken);
            return _mapper.Map<TouchScreenDto>(touchScreen);
        }
    }
}

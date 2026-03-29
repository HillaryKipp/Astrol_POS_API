using AstrolPOSAPI.Application.Features.POS.TouchScreen.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.TouchScreen.Commands.UpdateTouchScreen
{
    public class UpdateTouchScreenCommand : IRequest<TouchScreenDto>
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string ScreenName { get; set; } = default!;
        public string? Description { get; set; }
        public int GridRows { get; set; }
        public int GridColumns { get; set; }
        public int DefaultFontSize { get; set; }
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class UpdateTouchScreenCommandValidator : AbstractValidator<UpdateTouchScreenCommand>
    {
        public UpdateTouchScreenCommandValidator()
        {
            RuleFor(p => p.Id).NotEmpty();
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

    public class UpdateTouchScreenCommandHandler : IRequestHandler<UpdateTouchScreenCommand, TouchScreenDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateTouchScreenCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TouchScreenDto> Handle(UpdateTouchScreenCommand request, CancellationToken cancellationToken)
        {
            var touchScreen = await _unitOfWork.Repository<Domain.Entities.POS.TouchScreen>().GetByIdAsync(request.Id);

            if (touchScreen == null || touchScreen.DeletedDate != null)
                throw new KeyNotFoundException($"TouchScreen with ID {request.Id} not found");

            _mapper.Map(request, touchScreen);
            await _unitOfWork.Repository<Domain.Entities.POS.TouchScreen>().UpdateAsync(touchScreen);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<TouchScreenDto>(touchScreen);
        }
    }
}

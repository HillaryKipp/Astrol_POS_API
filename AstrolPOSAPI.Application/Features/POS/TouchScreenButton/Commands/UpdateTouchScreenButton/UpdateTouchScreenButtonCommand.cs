using AstrolPOSAPI.Application.Features.POS.TouchScreenButton.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.POS;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.TouchScreenButton.Commands.UpdateTouchScreenButton
{
    public class UpdateTouchScreenButtonCommand : IRequest<TouchScreenButtonDto>
    {
        public string Id { get; set; } = default!;
        public string ItemName { get; set; } = default!;
        public string BackgroundColor { get; set; } = default!;
        public string TextColor { get; set; } = default!;
        public string? ImageUrl { get; set; }
        public ButtonType ButtonType { get; set; }
        public ButtonShape Shape { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }

    public class UpdateTouchScreenButtonCommandValidator : AbstractValidator<UpdateTouchScreenButtonCommand>
    {
        public UpdateTouchScreenButtonCommandValidator()
        {
            RuleFor(v => v.Id).NotEmpty();
            RuleFor(v => v.ItemName).NotEmpty().MaximumLength(100);
        }
    }

    public class UpdateTouchScreenButtonCommandHandler : IRequestHandler<UpdateTouchScreenButtonCommand, TouchScreenButtonDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateTouchScreenButtonCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TouchScreenButtonDto> Handle(UpdateTouchScreenButtonCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.TouchScreenButton>().GetByIdAsync(request.Id);
            if (entity == null) return null!;

            _mapper.Map(request, entity);
            await _unitOfWork.Save(cancellationToken);
            return _mapper.Map<TouchScreenButtonDto>(entity);
        }
    }
}

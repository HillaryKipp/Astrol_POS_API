using AstrolPOSAPI.Application.Features.POS.DefaultScreen.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.DefaultScreen.Commands.CreateDefaultScreen
{
    public class CreateDefaultScreenCommand : IRequest<DefaultScreenDto>
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class CreateDefaultScreenCommandValidator : AbstractValidator<CreateDefaultScreenCommand>
    {
        public CreateDefaultScreenCommandValidator()
        {
            RuleFor(p => p.Code).NotEmpty().MaximumLength(32);
            RuleFor(p => p.Name).NotEmpty().MaximumLength(128);
            RuleFor(p => p.Description).NotEmpty().MaximumLength(128);
            RuleFor(p => p.CompanyId).NotEmpty();
            RuleFor(p => p.StoreOfOperationId).NotEmpty();
        }
    }

    public class CreateDefaultScreenCommandHandler : IRequestHandler<CreateDefaultScreenCommand, DefaultScreenDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDefaultScreenCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DefaultScreenDto> Handle(CreateDefaultScreenCommand request, CancellationToken cancellationToken)
        {
            var defaultScreen = _mapper.Map<AstrolPOSAPI.Domain.Entities.POS.DefaultScreen>(request);
            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.DefaultScreen>().AddAsync(defaultScreen);
            await _unitOfWork.Save(cancellationToken);
            return _mapper.Map<DefaultScreenDto>(defaultScreen);
        }
    }
}

using AstrolPOSAPI.Application.Features.POS.DrawerGroup.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.DrawerGroup.Commands.CreateDrawerGroup
{
    public class CreateDrawerGroupCommand : IRequest<DrawerGroupDto>
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class CreateDrawerGroupCommandValidator : AbstractValidator<CreateDrawerGroupCommand>
    {
        public CreateDrawerGroupCommandValidator()
        {
            RuleFor(p => p.Code)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(32).WithMessage("{PropertyName} must not exceed 32 characters.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(64).WithMessage("{PropertyName} must not exceed 64 characters.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(128).WithMessage("{PropertyName} must not exceed 128 characters.");

            RuleFor(p => p.CompanyId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.StoreOfOperationId)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }

    public class CreateDrawerGroupCommandHandler : IRequestHandler<CreateDrawerGroupCommand, DrawerGroupDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDrawerGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DrawerGroupDto> Handle(CreateDrawerGroupCommand request, CancellationToken cancellationToken)
        {
            var drawerGroup = _mapper.Map<Domain.Entities.POS.DrawerGroup>(request);
            await _unitOfWork.Repository<Domain.Entities.POS.DrawerGroup>().AddAsync(drawerGroup);
            await _unitOfWork.Save(cancellationToken);
            return _mapper.Map<DrawerGroupDto>(drawerGroup);
        }
    }
}

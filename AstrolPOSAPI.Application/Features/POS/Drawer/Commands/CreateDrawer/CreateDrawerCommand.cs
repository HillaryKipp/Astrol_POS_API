using AstrolPOSAPI.Application.Features.POS.Drawer.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.POS;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.Drawer.Commands.CreateDrawer
{
    public class CreateDrawerCommand : IRequest<DrawerDto>
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string DrawerGroupId { get; set; } = default!;
        public string? DefaultScreenId { get; set; }
        public string TerminalId { get; set; } = default!;
        public DrawerStatus Status { get; set; }
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class CreateDrawerCommandValidator : AbstractValidator<CreateDrawerCommand>
    {
        public CreateDrawerCommandValidator()
        {
            RuleFor(p => p.Code).NotEmpty().MaximumLength(32);
            RuleFor(p => p.Name).NotEmpty().MaximumLength(128);
            RuleFor(p => p.DrawerGroupId).NotEmpty();
            RuleFor(p => p.TerminalId).NotEmpty();
            RuleFor(p => p.CompanyId).NotEmpty();
            RuleFor(p => p.StoreOfOperationId).NotEmpty();
            RuleFor(p => p.Status).IsInEnum();
        }
    }

    public class CreateDrawerCommandHandler : IRequestHandler<CreateDrawerCommand, DrawerDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDrawerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DrawerDto> Handle(CreateDrawerCommand request, CancellationToken cancellationToken)
        {
            var drawer = _mapper.Map<AstrolPOSAPI.Domain.Entities.POS.Drawer>(request);
            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.Drawer>().AddAsync(drawer);
            await _unitOfWork.Save(cancellationToken);
            return _mapper.Map<DrawerDto>(drawer);
        }
    }
}

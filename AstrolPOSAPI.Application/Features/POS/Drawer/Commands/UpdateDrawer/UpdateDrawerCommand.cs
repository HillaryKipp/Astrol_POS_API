using AstrolPOSAPI.Application.Features.POS.Drawer.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.POS;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.Drawer.Commands.UpdateDrawer
{
    public class UpdateDrawerCommand : IRequest<DrawerDto>
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string DrawerGroupId { get; set; } = default!;
        public string? DefaultScreenId { get; set; }
        public string TerminalId { get; set; } = default!;
        public DrawerStatus Status { get; set; }
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class UpdateDrawerCommandValidator : AbstractValidator<UpdateDrawerCommand>
    {
        public UpdateDrawerCommandValidator()
        {
            RuleFor(p => p.Id).NotEmpty();
            RuleFor(p => p.Code).NotEmpty().MaximumLength(32);
            RuleFor(p => p.Name).NotEmpty().MaximumLength(128);
            RuleFor(p => p.DrawerGroupId).NotEmpty();
            RuleFor(p => p.TerminalId).NotEmpty();
            RuleFor(p => p.CompanyId).NotEmpty();
            RuleFor(p => p.StoreOfOperationId).NotEmpty();
            RuleFor(p => p.Status).IsInEnum();
        }
    }

    public class UpdateDrawerCommandHandler : IRequestHandler<UpdateDrawerCommand, DrawerDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDrawerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DrawerDto> Handle(UpdateDrawerCommand request, CancellationToken cancellationToken)
        {
            var drawer = await _unitOfWork.Repository<Domain.Entities.POS.Drawer>().GetByIdAsync(request.Id);

            if (drawer == null || drawer.DeletedDate != null)
                throw new KeyNotFoundException($"Drawer with ID {request.Id} not found");

            _mapper.Map(request, drawer);
            await _unitOfWork.Repository<Domain.Entities.POS.Drawer>().UpdateAsync(drawer);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<DrawerDto>(drawer);
        }
    }
}

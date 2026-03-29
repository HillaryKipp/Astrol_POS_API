using AstrolPOSAPI.Application.Features.POS.Terminal.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.Terminal.Commands.UpdateTerminal
{
    public class UpdateTerminalCommand : IRequest<TerminalDto>
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class UpdateTerminalCommandValidator : AbstractValidator<UpdateTerminalCommand>
    {
        public UpdateTerminalCommandValidator()
        {
            RuleFor(p => p.Id).NotEmpty();
            RuleFor(p => p.Code).NotEmpty().MaximumLength(32);
            RuleFor(p => p.Name).NotEmpty().MaximumLength(64);
            RuleFor(p => p.Description).NotEmpty().MaximumLength(128);
            RuleFor(p => p.CompanyId).NotEmpty();
            RuleFor(p => p.StoreOfOperationId).NotEmpty();
        }
    }

    public class UpdateTerminalCommandHandler : IRequestHandler<UpdateTerminalCommand, TerminalDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateTerminalCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TerminalDto> Handle(UpdateTerminalCommand request, CancellationToken cancellationToken)
        {
            var terminal = await _unitOfWork.Repository<Domain.Entities.POS.Terminal>().GetByIdAsync(request.Id);

            if (terminal == null || terminal.DeletedDate != null)
                throw new KeyNotFoundException($"Terminal with ID {request.Id} not found");

            _mapper.Map(request, terminal);
            await _unitOfWork.Repository<Domain.Entities.POS.Terminal>().UpdateAsync(terminal);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<TerminalDto>(terminal);
        }
    }
}

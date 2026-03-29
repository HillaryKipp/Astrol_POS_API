using AstrolPOSAPI.Application.Features.POS.Terminal.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.Terminal.Commands.CreateTerminal
{
    public class CreateTerminalCommand : IRequest<TerminalDto>
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class CreateTerminalCommandValidator : AbstractValidator<CreateTerminalCommand>
    {
        public CreateTerminalCommandValidator()
        {
            RuleFor(p => p.Code).NotEmpty().MaximumLength(32);
            RuleFor(p => p.Name).NotEmpty().MaximumLength(64);
            RuleFor(p => p.Description).NotEmpty().MaximumLength(128);
            RuleFor(p => p.CompanyId).NotEmpty();
            RuleFor(p => p.StoreOfOperationId).NotEmpty();
        }
    }

    public class CreateTerminalCommandHandler : IRequestHandler<CreateTerminalCommand, TerminalDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateTerminalCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TerminalDto> Handle(CreateTerminalCommand request, CancellationToken cancellationToken)
        {
            var terminal = _mapper.Map<Domain.Entities.POS.Terminal>(request);
            await _unitOfWork.Repository<Domain.Entities.POS.Terminal>().AddAsync(terminal);
            await _unitOfWork.Save(cancellationToken);
            return _mapper.Map<TerminalDto>(terminal);
        }
    }
}

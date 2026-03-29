using AstrolPOSAPI.Application.Interfaces.Repositories;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.Terminal.Commands.DeleteTerminal
{
    public class DeleteTerminalCommand : IRequest<bool>
    {
        public string Id { get; set; } = default!;
    }

    public class DeleteTerminalCommandHandler : IRequestHandler<DeleteTerminalCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTerminalCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteTerminalCommand request, CancellationToken cancellationToken)
        {
            var terminal = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.Terminal>().GetByIdAsync(request.Id);

            if (terminal == null)
                throw new KeyNotFoundException($"Terminal with ID {request.Id} not found");

            terminal.DeletedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.Terminal>().UpdateAsync(terminal);
            await _unitOfWork.Save(cancellationToken);

            return true;
        }
    }
}

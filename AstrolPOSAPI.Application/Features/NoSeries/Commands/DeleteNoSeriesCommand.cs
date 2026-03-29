using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Core;
using MediatR;

namespace AstrolPOSAPI.Application.Features.NoSeries.Commands.DeleteNoSeries
{
    public class DeleteNoSeriesCommand : IRequest
    {
        public string Id { get; set; } = default!;
    }

    public class DeleteNoSeriesCommandHandler : IRequestHandler<DeleteNoSeriesCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteNoSeriesCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteNoSeriesCommand request, CancellationToken cancellationToken)
        {
            var noSeries = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.NoSeries>()
                .GetByIdAsync(request.Id);

            if (noSeries == null)
                throw new KeyNotFoundException($"NoSeries with ID {request.Id} not found");

            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.NoSeries>().DeleteAsync(noSeries);
            await _unitOfWork.Save(cancellationToken);
        }
    }
}

using AstrolPOSAPI.Application.Features.NoSeries.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Core;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.NoSeries.Queries
{
    public class GetNoSeriesByIdQuery : IRequest<NoSeriesDto>
    {
        public string Id { get; set; } = default!;
    }

    public class GetNoSeriesByIdQueryHandler : IRequestHandler<GetNoSeriesByIdQuery, NoSeriesDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetNoSeriesByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<NoSeriesDto> Handle(GetNoSeriesByIdQuery request, CancellationToken cancellationToken)
        {
            var noSeries = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.NoSeries>()
                .GetByIdAsync(request.Id);

            if (noSeries == null)
                throw new KeyNotFoundException($"NoSeries with ID {request.Id} not found");

            return _mapper.Map<NoSeriesDto>(noSeries);
        }
    }
}

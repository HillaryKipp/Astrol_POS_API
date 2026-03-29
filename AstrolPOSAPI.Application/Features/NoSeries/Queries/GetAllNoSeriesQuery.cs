using AstrolPOSAPI.Application.Features.NoSeries.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.NoSeries.Queries
{
    public class GetAllNoSeriesQuery : IRequest<List<NoSeriesDto>>
    {
    }

    public class GetAllNoSeriesQueryHandler : IRequestHandler<GetAllNoSeriesQuery, List<NoSeriesDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllNoSeriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<NoSeriesDto>> Handle(GetAllNoSeriesQuery request, CancellationToken cancellationToken)
        {
            var noSeriesList = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.NoSeries>()
                .GetAllAsync();

            return _mapper.Map<List<NoSeriesDto>>(noSeriesList);
        }
    }
}

using AstrolPOSAPI.Application.Features.NoSeries.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Core;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.NoSeries.Commands.UpdateNoSeries
{
    public class UpdateNoSeriesCommand : IRequest<NoSeriesDto>
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }
        public string? CurrentNo { get; set; }
    }

    public class UpdateNoSeriesCommandHandler : IRequestHandler<UpdateNoSeriesCommand, NoSeriesDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateNoSeriesCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<NoSeriesDto> Handle(UpdateNoSeriesCommand request, CancellationToken cancellationToken)
        {
            var noSeries = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.NoSeries>()
                .GetByIdAsync(request.Id);

            if (noSeries == null)
                throw new KeyNotFoundException($"NoSeries with ID {request.Id} not found");

            noSeries.Code = request.Code;
            noSeries.Description = request.Description;
            noSeries.Prefix = request.Prefix;
            noSeries.Suffix = request.Suffix;
            noSeries.CurrentNo = request.CurrentNo;

            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.NoSeries>().UpdateAsync(noSeries);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<NoSeriesDto>(noSeries);
        }
    }
}

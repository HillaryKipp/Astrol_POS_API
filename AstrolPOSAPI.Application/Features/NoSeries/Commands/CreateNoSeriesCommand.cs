using AstrolPOSAPI.Application.Features.NoSeries.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.NoSeries.Commands.CreateNoSeries
{
    public class CreateNoSeriesCommand : IRequest<NoSeriesDto>
    {
        public string Code { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }
        public string? CurrentNo { get; set; }
    }

    public class CreateNoSeriesCommandHandler : IRequestHandler<CreateNoSeriesCommand, NoSeriesDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateNoSeriesCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<NoSeriesDto> Handle(CreateNoSeriesCommand request, CancellationToken cancellationToken)
        {
            var noSeries = new Domain.Entities.Core.NoSeries
            {
                Code = request.Code,
                Description = request.Description,
                Prefix = request.Prefix,
                Suffix = request.Suffix,
                CurrentNo = request.CurrentNo ?? "0"
            };

            await _unitOfWork.Repository<Domain.Entities.Core.NoSeries>().AddAsync(noSeries);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<NoSeriesDto>(noSeries);
        }
    }
}

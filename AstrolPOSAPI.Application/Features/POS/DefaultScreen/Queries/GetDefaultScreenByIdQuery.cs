using AstrolPOSAPI.Application.Features.POS.DefaultScreen.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.DefaultScreen.Queries
{
    public class GetDefaultScreenByIdQuery : IRequest<DefaultScreenDto>
    {
        public string Id { get; set; } = default!;
    }

    public class GetDefaultScreenByIdQueryHandler : IRequestHandler<GetDefaultScreenByIdQuery, DefaultScreenDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDefaultScreenByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DefaultScreenDto> Handle(GetDefaultScreenByIdQuery request, CancellationToken cancellationToken)
        {
            var defaultScreen = await _unitOfWork.Repository<Domain.Entities.POS.DefaultScreen>().GetByIdAsync(request.Id);

            if (defaultScreen == null || defaultScreen.DeletedDate != null)
                throw new KeyNotFoundException($"DefaultScreen with ID {request.Id} not found");

            return _mapper.Map<DefaultScreenDto>(defaultScreen);
        }
    }
}

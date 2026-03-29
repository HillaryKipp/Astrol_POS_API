using AstrolPOSAPI.Application.Features.POS.TouchScreen.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.TouchScreen.Queries
{
    public class GetTouchScreenByIdQuery : IRequest<TouchScreenDto>
    {
        public string Id { get; set; } = default!;
        public bool IncludeButtons { get; set; } = true;
    }

    public class GetTouchScreenByIdQueryHandler : IRequestHandler<GetTouchScreenByIdQuery, TouchScreenDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTouchScreenByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TouchScreenDto> Handle(GetTouchScreenByIdQuery request, CancellationToken cancellationToken)
        {
            var touchScreen = await _unitOfWork.Repository<Domain.Entities.POS.TouchScreen>().GetByIdAsync(request.Id);

            if (touchScreen == null || touchScreen.DeletedDate != null)
                throw new KeyNotFoundException($"TouchScreen with ID {request.Id} not found");

            var dto = _mapper.Map<TouchScreenDto>(touchScreen);

            // Load buttons if requested
            if (request.IncludeButtons)
            {
                var buttons = await _unitOfWork.Repository<Domain.Entities.POS.TouchScreenButton>().GetAllAsync();
                var screenButtons = buttons.Where(b => b.TouchScreenId == request.Id && b.DeletedDate == null).ToList();
                dto.Buttons = _mapper.Map<List<TouchScreenButtonDto>>(screenButtons);
            }

            return dto;
        }
    }
}

using AstrolPOSAPI.Application.Features.POS.TouchScreen.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.TouchScreenButton.Queries
{
    public class GetTouchScreenButtonByIdQuery : IRequest<TouchScreenButtonDto>
    {
        public string Id { get; set; } = default!;
    }

    public class GetTouchScreenButtonByIdQueryHandler : IRequestHandler<GetTouchScreenButtonByIdQuery, TouchScreenButtonDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTouchScreenButtonByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TouchScreenButtonDto> Handle(GetTouchScreenButtonByIdQuery request, CancellationToken cancellationToken)
        {
            var button = await _unitOfWork.Repository<Domain.Entities.POS.TouchScreenButton>().GetByIdAsync(request.Id);

            if (button == null || button.DeletedDate != null)
                throw new KeyNotFoundException($"TouchScreenButton with ID {request.Id} not found");

            return _mapper.Map<TouchScreenButtonDto>(button);
        }
    }
}

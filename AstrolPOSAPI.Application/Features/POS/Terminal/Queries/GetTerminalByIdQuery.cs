using AstrolPOSAPI.Application.Features.POS.Terminal.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace AstrolPOSAPI.Application.Features.POS.Terminal.Queries
{
    public class GetTerminalByIdQuery : IRequest<TerminalDto>
    {
        public string Id { get; set; } = default!;
    }

    public class GetTerminalByIdQueryHandler : IRequestHandler<GetTerminalByIdQuery, TerminalDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTerminalByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TerminalDto> Handle(GetTerminalByIdQuery request, CancellationToken cancellationToken)
        {
            var terminal = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.Terminal>().GetByIdAsync(request.Id);

            if (terminal == null || terminal.DeletedDate != null)
                throw new KeyNotFoundException($"Terminal with ID {request.Id} not found");

            return _mapper.Map<TerminalDto>(terminal);
        }
    }
}

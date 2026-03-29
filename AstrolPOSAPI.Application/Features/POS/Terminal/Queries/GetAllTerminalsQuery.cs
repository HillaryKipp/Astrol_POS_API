using AstrolPOSAPI.Application.Features.POS.Terminal.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.POS.Terminal.Queries
{
    public class GetAllTerminalsQuery : IRequest<List<TerminalDto>>
    {
        public string? CompanyId { get; set; }
        public string? StoreOfOperationId { get; set; }
    }

    public class GetAllTerminalsQueryHandler : IRequestHandler<GetAllTerminalsQuery, List<TerminalDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllTerminalsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<TerminalDto>> Handle(GetAllTerminalsQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Domain.Entities.POS.Terminal>().Entities
                .Include(x => x.Company)
                .Include(x => x.StoreOfOperation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.CompanyId))
                query = query.Where(x => x.CompanyId == request.CompanyId);

            if (!string.IsNullOrEmpty(request.StoreOfOperationId))
                query = query.Where(x => x.StoreOfOperationId == request.StoreOfOperationId);

            var list = await query.ToListAsync(cancellationToken);
            return _mapper.Map<List<TerminalDto>>(list);
        }
    }
}

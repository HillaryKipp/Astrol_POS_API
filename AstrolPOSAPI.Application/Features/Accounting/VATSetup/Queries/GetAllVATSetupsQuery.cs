using AstrolPOSAPI.Application.Features.Accounting.VATSetup.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Accounting;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Accounting.VATSetup.Queries
{
    public class GetAllVATSetupsQuery : IRequest<List<VATSetupDto>>
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetAllVATSetupsQueryHandler : IRequestHandler<GetAllVATSetupsQuery, List<VATSetupDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllVATSetupsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<VATSetupDto>> Handle(GetAllVATSetupsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.Repository<VATPostingSetup>().Entities
                .Where(v => v.CompanyId == request.CompanyId && v.DeletedDate == null)
                .OrderBy(v => v.Code)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<VATSetupDto>>(entities);
        }
    }
}

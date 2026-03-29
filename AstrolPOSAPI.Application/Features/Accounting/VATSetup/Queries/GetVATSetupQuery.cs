using AstrolPOSAPI.Application.Features.Accounting.VATSetup.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Accounting;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Accounting.VATSetup.Queries
{
    public class GetVATSetupQuery : IRequest<VATSetupDto>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class GetVATSetupQueryHandler : IRequestHandler<GetVATSetupQuery, VATSetupDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetVATSetupQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<VATSetupDto> Handle(GetVATSetupQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<VATPostingSetup>().Entities
                .FirstOrDefaultAsync(v => v.Id == request.Id && v.DeletedDate == null, cancellationToken);

            if (entity == null)
                throw new KeyNotFoundException($"VAT Setup with ID '{request.Id}' not found.");

            return _mapper.Map<VATSetupDto>(entity);
        }
    }
}

using AstrolPOSAPI.Application.Features.Sales.Customer.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Sales.Customer.Queries
{
    public class GetAllCustomersQuery : IRequest<List<CustomerDto>>
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, List<CustomerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCustomersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Accounting.Customer>().Entities
                .Where(c => c.CompanyId == request.CompanyId && c.DeletedDate == null)
                .Include(c => c.CustomerPostingGroup)
                .Include(c => c.GenBusPostingGroup)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<CustomerDto>>(customers);
        }
    }
}

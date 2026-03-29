using AstrolPOSAPI.Application.Features.Sales.Customer.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Sales.Customer.Queries
{
    public class GetCustomerByIdQuery : IRequest<CustomerDto>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Accounting.Customer>().Entities
                .Include(c => c.CustomerPostingGroup)
                .Include(c => c.GenBusPostingGroup)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {request.Id} not found");

            return _mapper.Map<CustomerDto>(customer);
        }
    }
}

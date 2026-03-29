using AstrolPOSAPI.Application.Features.Sales.Customer.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Sales.Customer.Commands
{
    public class UpdateCustomerCommand : IRequest<CustomerDto>
    {
        public UpdateCustomerDto Customer { get; set; } = default!;
    }

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Accounting.Customer>().GetByIdAsync(request.Customer.Id);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {request.Customer.Id} not found");

            _mapper.Map(request.Customer, customer);
            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Accounting.Customer>().UpdateAsync(customer);
            await _unitOfWork.Save(cancellationToken);

            var result = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Accounting.Customer>().Entities
                .Include(c => c.CustomerPostingGroup)
                .Include(c => c.GenBusPostingGroup)
                .FirstOrDefaultAsync(c => c.Id == customer.Id, cancellationToken);

            return _mapper.Map<CustomerDto>(result);
        }
    }
}

using AstrolPOSAPI.Application.Features.Sales.Customer.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Application.Interfaces.Services;
using AstrolPOSAPI.Domain.Entities.Accounting;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Sales.Customer.Commands
{
    public class CreateCustomerCommand : IRequest<CustomerDto>
    {
        public CreateCustomerDto Customer { get; set; } = default!;
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INoSeriesService _noSeriesService;

        public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, INoSeriesService noSeriesService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _noSeriesService = noSeriesService;
        }

        public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerNo = await _noSeriesService.GenerateNextNumberAsync("CUSTOMER", cancellationToken);

            var customer = _mapper.Map<AstrolPOSAPI.Domain.Entities.Accounting.Customer>(request.Customer);
            customer.No = customerNo;
            customer.Id = Guid.NewGuid().ToString();

            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Accounting.Customer>().AddAsync(customer);
            await _unitOfWork.Save(cancellationToken);

            var result = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Accounting.Customer>().Entities
                .Include(c => c.CustomerPostingGroup)
                .Include(c => c.GenBusPostingGroup)
                .FirstOrDefaultAsync(c => c.Id == customer.Id, cancellationToken);

            return _mapper.Map<CustomerDto>(result);
        }
    }
}

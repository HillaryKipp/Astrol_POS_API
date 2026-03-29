using AstrolPOSAPI.Application.Interfaces.Repositories;
using MediatR;

namespace AstrolPOSAPI.Application.Features.Sales.Customer.Commands
{
    public class DeleteCustomerCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCustomerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Accounting.Customer>().GetByIdAsync(request.Id);
            if (customer == null)
                return false;

            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Accounting.Customer>().DeleteAsync(customer);
            await _unitOfWork.Save(cancellationToken);
            return true;
        }
    }
}

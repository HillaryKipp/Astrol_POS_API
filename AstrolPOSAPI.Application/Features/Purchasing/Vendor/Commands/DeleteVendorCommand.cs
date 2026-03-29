using AstrolPOSAPI.Application.Interfaces.Repositories;
using MediatR;

namespace AstrolPOSAPI.Application.Features.Purchasing.Vendor.Commands
{
    public class DeleteVendorCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class DeleteVendorCommandHandler : IRequestHandler<DeleteVendorCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteVendorCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
        {
            var vendor = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Purchasing.Vendor>().GetByIdAsync(request.Id);
            if (vendor == null)
                return false;

            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Purchasing.Vendor>().DeleteAsync(vendor);
            await _unitOfWork.Save(cancellationToken);
            return true;
        }
    }
}

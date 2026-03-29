using AstrolPOSAPI.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.POS.AssignedDrawer.Commands.DeleteAssignedDrawer
{
    public class DeleteAssignedDrawerCommand : IRequest<bool>
    {
        public string Id { get; set; } = default!;
    }

    public class DeleteAssignedDrawerCommandHandler : IRequestHandler<DeleteAssignedDrawerCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAssignedDrawerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteAssignedDrawerCommand request, CancellationToken cancellationToken)
        {
            var assignedDrawer = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.AssignedDrawer>().GetByIdAsync(request.Id);

            if (assignedDrawer == null)
                throw new KeyNotFoundException($"AssignedDrawer with ID {request.Id} not found");

            // Safety check: Don't allow unassigning if there are open sales
            var hasOpenSales = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.SalesOrder>().Entities.AnyAsync(o => 
                o.DrawerId == assignedDrawer.DrawerId && 
                o.Status == AstrolPOSAPI.Domain.Entities.POS.SalesOrderStatus.Pending &&
                o.DeletedDate == null);

            if (hasOpenSales)
                throw new InvalidOperationException("Cannot unassign drawer with open sales. Please close or delete all open sales first.");

            // Reset physical drawer status to Open (Available)
            var drawer = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.Drawer>().GetByIdAsync(assignedDrawer.DrawerId);
            if (drawer != null)
            {
                drawer.Status = AstrolPOSAPI.Domain.Entities.POS.DrawerStatus.Open;
                await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.Drawer>().UpdateAsync(drawer);
            }

            assignedDrawer.DeletedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.AssignedDrawer>().UpdateAsync(assignedDrawer);
            await _unitOfWork.Save(cancellationToken);

            return true;
        }
    }
}

using AstrolPOSAPI.Application.Features.POS.AssignedDrawer.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.POS.AssignedDrawer.Commands.UpdateAssignedDrawer
{
    public class UpdateAssignedDrawerCommand : IRequest<AssignedDrawerDto>
    {
        public string Id { get; set; } = default!;
        public string DrawerId { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string? DefaultScreenId { get; set; }
        public string? DefaultShortcutBar { get; set; }
        public DateTimeOffset? SessionTimeIn { get; set; }
        public DateTimeOffset? SessionTimeOut { get; set; }
        public decimal OpenCash { get; set; }
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class UpdateAssignedDrawerCommandValidator : AbstractValidator<UpdateAssignedDrawerCommand>
    {
        public UpdateAssignedDrawerCommandValidator()
        {
            RuleFor(p => p.Id).NotEmpty();
            RuleFor(p => p.DrawerId).NotEmpty();
            RuleFor(p => p.UserId).NotEmpty();
            RuleFor(p => p.CompanyId).NotEmpty();
            RuleFor(p => p.StoreOfOperationId).NotEmpty();
            RuleFor(p => p.OpenCash).GreaterThanOrEqualTo(0);
        }
    }

    public class UpdateAssignedDrawerCommandHandler : IRequestHandler<UpdateAssignedDrawerCommand, AssignedDrawerDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAssignedDrawerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AssignedDrawerDto> Handle(UpdateAssignedDrawerCommand request, CancellationToken cancellationToken)
        {
            var assignedDrawer = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.AssignedDrawer>().GetByIdAsync(request.Id);

            if (assignedDrawer == null || assignedDrawer.DeletedDate != null)
                throw new KeyNotFoundException($"AssignedDrawer with ID {request.Id} not found");

            // Safety check: Don't allow changing assignment if there are open sales
            if (assignedDrawer.DrawerId != request.DrawerId || assignedDrawer.UserId != request.UserId)
            {
                var hasOpenSales = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.SalesOrder>().Entities.AnyAsync(o => 
                    o.DrawerId == assignedDrawer.DrawerId && 
                    o.Status == AstrolPOSAPI.Domain.Entities.POS.SalesOrderStatus.Pending &&
                    o.DeletedDate == null, cancellationToken);

                if (hasOpenSales)
                    throw new InvalidOperationException("Cannot change drawer assignment while there are open sales. Please close or delete all open sales first.");
            }

            // 1. Handle Drawer change
            if (assignedDrawer.DrawerId != request.DrawerId)
            {
                // Release old drawer
                var oldDrawer = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.Drawer>().GetByIdAsync(assignedDrawer.DrawerId);
                if (oldDrawer != null)
                {
                    oldDrawer.Status = AstrolPOSAPI.Domain.Entities.POS.DrawerStatus.Open;
                    await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.Drawer>().UpdateAsync(oldDrawer);
                }

                // Lock new drawer
                var newDrawer = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.Drawer>().GetByIdAsync(request.DrawerId);
                if (newDrawer == null)
                    throw new KeyNotFoundException($"New Drawer with ID {request.DrawerId} not found");

                if (newDrawer.Status != AstrolPOSAPI.Domain.Entities.POS.DrawerStatus.Open)
                    throw new InvalidOperationException($"The new drawer '{newDrawer.Name}' is currently {newDrawer.Status} and cannot be assigned.");

                newDrawer.Status = AstrolPOSAPI.Domain.Entities.POS.DrawerStatus.Closed;
                await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.Drawer>().UpdateAsync(newDrawer);
                assignedDrawer.Drawer = newDrawer; // Populate for mapping
            }
            else
            {
                // If drawer didn't change, still load it once to ensure the response has the status
                if (assignedDrawer.Drawer == null)
                {
                    assignedDrawer.Drawer = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.Drawer>().GetByIdAsync(assignedDrawer.DrawerId);
                }
            }

            // 2. Handle User change
            if (assignedDrawer.UserId != request.UserId)
            {
                var isNewUserAssigned = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.AssignedDrawer>().Entities.AnyAsync(ad => 
                    ad.UserId == request.UserId && 
                    ad.DeletedDate == null, cancellationToken);

                if (isNewUserAssigned)
                    throw new InvalidOperationException("The new user is already assigned to another active drawer session.");
            }

            _mapper.Map(request, assignedDrawer);
            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.AssignedDrawer>().UpdateAsync(assignedDrawer);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<AssignedDrawerDto>(assignedDrawer);
        }
    }
}

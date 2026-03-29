using AstrolPOSAPI.Application.Features.POS.AssignedDrawer.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.POS.AssignedDrawer.Commands.CreateAssignedDrawer
{
    public class CreateAssignedDrawerCommand : IRequest<AssignedDrawerDto>
    {
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

    public class CreateAssignedDrawerCommandValidator : AbstractValidator<CreateAssignedDrawerCommand>
    {
        public CreateAssignedDrawerCommandValidator()
        {
            RuleFor(p => p.DrawerId).NotEmpty();
            RuleFor(p => p.UserId).NotEmpty();
            RuleFor(p => p.CompanyId).NotEmpty();
            RuleFor(p => p.StoreOfOperationId).NotEmpty();
            RuleFor(p => p.OpenCash).GreaterThanOrEqualTo(0);
        }
    }

    public class CreateAssignedDrawerCommandHandler : IRequestHandler<CreateAssignedDrawerCommand, AssignedDrawerDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateAssignedDrawerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AssignedDrawerDto> Handle(CreateAssignedDrawerCommand request, CancellationToken cancellationToken)
        {
            // 1. Check if drawer exists and is Open (Available)
            var drawer = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.Drawer>().GetByIdAsync(request.DrawerId);
            if (drawer == null)
                throw new KeyNotFoundException($"Drawer with ID {request.DrawerId} not found");

            if (drawer.Status != AstrolPOSAPI.Domain.Entities.POS.DrawerStatus.Open)
                throw new InvalidOperationException($"Drawer '{drawer.Name}' is currently {drawer.Status} and cannot be assigned. Please unassign it first.");

            // 2. Check if user is already assigned to another drawer
            var isUserAssigned = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.AssignedDrawer>().Entities.AnyAsync(ad => 
                ad.UserId == request.UserId && 
                ad.DeletedDate == null, cancellationToken);

            if (isUserAssigned)
                throw new InvalidOperationException("This user is already assigned to another active drawer session.");

            // 3. Safety check: Don't allow assigning if there are open sales for this drawer
            var hasOpenSales = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.SalesOrder>().Entities.AnyAsync(o => 
                o.DrawerId == request.DrawerId && 
                o.Status == AstrolPOSAPI.Domain.Entities.POS.SalesOrderStatus.Pending &&
                o.DeletedDate == null, cancellationToken);

            if (hasOpenSales)
                throw new InvalidOperationException("Cannot assign drawer with existing open sales. Please close or delete all open sales first.");

            // 4. Transform and save assignment
            var assignedDrawer = _mapper.Map<AstrolPOSAPI.Domain.Entities.POS.AssignedDrawer>(request);
            assignedDrawer.AssignedAt = DateTime.UtcNow; // Ensure AssignedAt is set
            assignedDrawer.Drawer = drawer; // Populate navigation property for mapping

            // 5. Update physical drawer status to Closed (Assigned)
            drawer.Status = AstrolPOSAPI.Domain.Entities.POS.DrawerStatus.Closed;
            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.Drawer>().UpdateAsync(drawer);

            await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.AssignedDrawer>().AddAsync(assignedDrawer);
            await _unitOfWork.Save(cancellationToken);
            return _mapper.Map<AssignedDrawerDto>(assignedDrawer);
        }
    }
}

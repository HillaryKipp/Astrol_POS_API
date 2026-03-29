using AstrolPOSAPI.Application.Features.POS.Sales.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.POS;
using AutoMapper;
using FluentValidation;
using MediatR;
using AppUserEntity = AstrolPOSAPI.Domain.Entities.Identity.AppUser;
using CompanyEntity = AstrolPOSAPI.Domain.Entities.Core.Company;
using DrawerEntity = AstrolPOSAPI.Domain.Entities.POS.Drawer;
using StoreEntity = AstrolPOSAPI.Domain.Entities.Core.Store;

namespace AstrolPOSAPI.Application.Features.POS.Sales.Commands.CreateSalesOrder
{
    public class CreateSalesOrderCommand : IRequest<SalesOrderDto>
    {
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string CashierId { get; set; } = default!;
        public string DrawerId { get; set; } = default!;
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class CreateSalesOrderCommandValidator : AbstractValidator<CreateSalesOrderCommand>
    {
        public CreateSalesOrderCommandValidator()
        {
            RuleFor(p => p.CashierId).NotEmpty();
            RuleFor(p => p.DrawerId).NotEmpty();
            RuleFor(p => p.CompanyId).NotEmpty();
            RuleFor(p => p.StoreOfOperationId).NotEmpty();
        }
    }

    public class CreateSalesOrderCommandHandler : IRequestHandler<CreateSalesOrderCommand, SalesOrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Microsoft.AspNetCore.Identity.UserManager<AppUserEntity> _userManager;

        public CreateSalesOrderCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            Microsoft.AspNetCore.Identity.UserManager<AppUserEntity> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<SalesOrderDto> Handle(CreateSalesOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate Cashier (Existence and Role)
            var cashier = await _userManager.FindByIdAsync(request.CashierId);
            if (cashier == null)
                throw new KeyNotFoundException($"Cashier with ID {request.CashierId} not found.");

            var isCashier_or_SuperAdmin_or_Admin = await _userManager.IsInRoleAsync(cashier, "Cashier") || cashier.Role == "Cashier" || cashier.Role == "Super Admin" || cashier.Role == "Admin";
            if (!isCashier_or_SuperAdmin_or_Admin)
                throw new InvalidOperationException($"User {cashier.UserName} does not have the 'Cashier', 'Super Admin' or 'Admin' role.");

            // 2. Validate Drawer
            var drawer = await _unitOfWork.Repository<DrawerEntity>().GetByIdAsync(request.DrawerId);
            if (drawer == null)
                throw new KeyNotFoundException($"Drawer with ID {request.DrawerId} not found.");

            // 3. Validate Company
            var company = await _unitOfWork.Repository<CompanyEntity>().GetByIdAsync(request.CompanyId);
            if (company == null)
                throw new KeyNotFoundException($"Company with ID {request.CompanyId} not found.");

            // 4. Validate Store
            var store = await _unitOfWork.Repository<StoreEntity>().GetByIdAsync(request.StoreOfOperationId);
            if (store == null)
                throw new KeyNotFoundException($"Store with ID {request.StoreOfOperationId} not found.");

            // Generate order number
            var orderNo = $"SO-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..4].ToUpper()}";

            var salesOrder = new SalesOrder
            {
                OrderNo = orderNo,
                OrderDate = DateTime.UtcNow,
                Status = SalesOrderStatus.Pending,
                CustomerId = request.CustomerId,
                CustomerName = request.CustomerName,
                CashierId = request.CashierId,
                DrawerId = request.DrawerId,
                CompanyId = request.CompanyId,
                StoreOfOperationId = request.StoreOfOperationId,
                Subtotal = 0,
                DiscountAmount = 0,
                TaxAmount = 0,
                TotalAmount = 0
            };

            await _unitOfWork.Repository<SalesOrder>().AddAsync(salesOrder);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<SalesOrderDto>(salesOrder);
        }
    }
}

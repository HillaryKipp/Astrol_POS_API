using AstrolPOSAPI.Application.Features.User.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Application.Interfaces.Services;
using AstrolPOSAPI.Domain.Entities.Core;
using AstrolPOSAPI.Domain.Entities.Identity;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AstrolPOSAPI.Application.Features.User.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string UserName { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? NationalID { get; set; }
        public string Password { get; set; } = default!;
        public string? Role { get; set; }
        public string PhoneNumber { get; set; } = default!;
        public string CompanyId { get; set; } = default!;

        /// <summary>
        /// Primary store (for backward compatibility)
        /// </summary>
        public string? StoreOfOperationId { get; set; }

        /// <summary>
        /// List of all store IDs to assign to this user
        /// If empty and StoreOfOperationId is provided, only primary store will be assigned
        /// </summary>
        public List<string> StoreIds { get; set; } = new();
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INoSeriesService _noSeriesService;
        private readonly UserManager<AppUser> _userManager;

        public CreateUserCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            INoSeriesService noSeriesService,
            UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _noSeriesService = noSeriesService;
            _userManager = userManager;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Validate Company exists
            var company = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.Company>().GetByIdAsync(request.CompanyId);
            if (company == null)
                throw new InvalidOperationException($"Company with ID {request.CompanyId} not found");

            // Validate primary store if provided
            if (request.StoreOfOperationId != null)
            {
                var store = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.Store>().GetByIdAsync(request.StoreOfOperationId);
                if (store == null)
                    throw new InvalidOperationException($"Store with ID {request.StoreOfOperationId} not found");

                if (store.CompanyId != request.CompanyId)
                    throw new InvalidOperationException($"Store does not belong to the specified Company");
            }

            // Validate all stores in StoreIds
            if (request.StoreIds?.Any() == true)
            {
                foreach (var storeId in request.StoreIds)
                {
                    var assignedStore = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.Store>().GetByIdAsync(storeId);
                    if (assignedStore == null)
                        throw new InvalidOperationException($"Store with ID {storeId} not found");

                    if (assignedStore.CompanyId != request.CompanyId)
                        throw new InvalidOperationException($"Store {storeId} does not belong to the specified Company");
                }
            }

            // Auto-generate Employee Number using NoSeries
            var empNo = await _noSeriesService.GenerateNextNumberAsync("EMPLOYEE", cancellationToken);

            var user = new AppUser
            {
                UserName = request.UserName,
                Name = request.Name,
                NationalID = request.NationalID,
                PhoneNumber = request.PhoneNumber,
                CompanyId = request.CompanyId,
                StoreOfOperationId = request.StoreOfOperationId,
                EmpNo = empNo,
                Role = request.Role,
                IsActive = true,
                PasswordChangeRequired = true // Force password change on first login
            };

            // Generate default password if not provided
            var password = !string.IsNullOrEmpty(request.Password) 
                ? request.Password 
                : $"Pass@123."; // Default pattern: Pass + EmpNo (e.g. Pass@EMP001)

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User creation failed: {errors}");
            }

            // TODO: Create UserStore junction records
            // This should be handled in the controller or via a separate service
            // to avoid Application layer depending on Persistence layer directly

            return _mapper.Map<UserDto>(user);
        }
    }
}

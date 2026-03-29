using AstrolPOSAPI.Application.Features.User.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Identity;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AstrolPOSAPI.Application.Features.User.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<UserDto>
    {
        public string Id { get; set; } = default!;
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? StoreOfOperationId { get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(
            UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null || user.IsDeleted)
                throw new KeyNotFoundException($"User with ID {request.Id} not found");

            // Update only provided fields
            if (!string.IsNullOrWhiteSpace(request.Name))
                user.Name = request.Name;

            if (!string.IsNullOrWhiteSpace(request.UserName))
                user.UserName = request.UserName;

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                user.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(request.Email))
                user.Email = request.Email;

            if (!string.IsNullOrWhiteSpace(request.Role))
                user.Role = request.Role;

            // Validate store if provided
            if (!string.IsNullOrWhiteSpace(request.StoreOfOperationId))
            {
                var store = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Core.Store>()
                    .GetByIdAsync(request.StoreOfOperationId);

                if (store == null)
                    throw new InvalidOperationException($"Store with ID {request.StoreOfOperationId} not found");

                if (store.CompanyId != user.CompanyId)
                    throw new InvalidOperationException("Store does not belong to the user's company");

                user.StoreOfOperationId = request.StoreOfOperationId;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User update failed: {errors}");
            }

            return _mapper.Map<UserDto>(user);
        }
    }
}

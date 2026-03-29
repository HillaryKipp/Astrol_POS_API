using AstrolPOSAPI.Application.Features.User.DTOs;
using AstrolPOSAPI.Domain.Entities.Identity;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.User.Queries
{
    public class GetAllUsersQuery : IRequest<List<AstrolPOSAPI.Application.Features.User.DTOs.UserDto>>
    {
        public string? CompanyId { get; set; }
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<AstrolPOSAPI.Application.Features.User.DTOs.UserDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var query = _userManager.Users.Where(u => !u.IsDeleted);

            // Filter by CompanyId if provided
            if (!string.IsNullOrEmpty(request.CompanyId))
            {
                query = query.Where(u => u.CompanyId == request.CompanyId);
            }

            var users = await query.ToListAsync(cancellationToken);
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}

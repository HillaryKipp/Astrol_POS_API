using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.User.Queries.GetRoles
{
    public class GetRolesQuery : IRequest<List<string>>
    {
    }

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<string>>
    {
        private readonly RoleManager<AstrolPOSAPI.Domain.Entities.Identity.AppRole> _roleManager;

        public GetRolesQueryHandler(RoleManager<AstrolPOSAPI.Domain.Entities.Identity.AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<string>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            // Fetch directly from AspNetRoles table using RoleManager
            var roles = await _roleManager.Roles
                .Select(r => r.Name!)
                .OrderBy(r => r)
                .ToListAsync(cancellationToken);

            return roles;
        }
    }
}

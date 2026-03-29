using System;

namespace AstrolPOSAPI.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
    }
}

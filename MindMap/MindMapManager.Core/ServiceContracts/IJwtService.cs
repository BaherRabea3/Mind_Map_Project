using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using System.Security.Claims;

namespace MindMapManager.Core.ServiceContracts
{
    public interface IJwtService
    {
        Task<AuthenticationResponse> CreateJwtTokenAsync(ApplicationUser applicationUser);
        ClaimsPrincipal? GetPrincipaleFromJwtToken(string? jwtToken);
    }
}

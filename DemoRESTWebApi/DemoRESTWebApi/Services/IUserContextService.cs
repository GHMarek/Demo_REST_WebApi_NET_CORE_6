using System.Security.Claims;

namespace DemoRESTWebApi.Services
{
    public interface IUserContextService
    {
        int? GetUserId { get; }
        ClaimsPrincipal User { get; }
    }
}
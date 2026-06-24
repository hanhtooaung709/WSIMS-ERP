using System.Security.Claims;

namespace ERP.Warehouse.Api.Common;

public class AuthorizationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected string AuthorizedUserId => _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value
                              ?? _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? "System";
}

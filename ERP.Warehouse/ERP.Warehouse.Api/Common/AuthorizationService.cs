using System.Security.Claims;

namespace ERP.Warehouse.Api.Common;

public class AuthorizationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthorizationService> _logger;

    public AuthorizationService(IHttpContextAccessor httpContextAccessor, ILogger<AuthorizationService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    protected string AuthorizedUserId
    {
        get
        {
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx is null)
            {
                _logger.LogWarning("AuthorizedUserId: HttpContext is null");
                return "System";
            }

            var user = ctx.User;
            if (user is null)
            {
                _logger.LogWarning("AuthorizedUserId: User is null");
                return "System";
            }

            var isAuth = user.Identity?.IsAuthenticated;
            var authScheme = user.Identity?.AuthenticationType;
            var allClaims = string.Join("; ", user.Claims.Select(c => $"{c.Type}={c.Value}"));

            _logger.LogInformation(
                "AuthorizedUserId: IsAuthenticated={IsAuth}, AuthScheme={Scheme}, Claims=[{Claims}]",
                isAuth, authScheme, allClaims);

            var userIdClaim = user.FindFirst("UserId")?.Value;
            if (userIdClaim is not null)
            {
                _logger.LogInformation("AuthorizedUserId: Found 'UserId' claim = {UserId}", userIdClaim);
                return userIdClaim;
            }

            var nameIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (nameIdClaim is not null)
            {
                _logger.LogInformation("AuthorizedUserId: Found NameIdentifier claim = {NameId}", nameIdClaim);
                return nameIdClaim;
            }

            _logger.LogWarning("AuthorizedUserId: No matching claim found, falling back to 'System'");
            return "System";
        }
    }
}

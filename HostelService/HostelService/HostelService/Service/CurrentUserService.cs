using Repository.Interface;
using System.Security.Claims;

namespace HostelService.Service;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string UserId   => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string HostelId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.PrimarySid);
    public string RoleEntityId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Actor);
    public string DistrictId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.GroupSid);
    public string WorkFlowId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Sid);
    public string DeptId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.PrimaryGroupSid);
    
}
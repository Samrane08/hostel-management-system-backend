using System.Security.Claims;

namespace DDO_Service.API.Features.Token
{
  

    public class UserClaimsService : IUserClaimsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? throw new UnauthorizedAccessException("User ID claim is missing.");
        }

        public string GetDDOId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.PrimarySid)?.Value
                   ?? throw new UnauthorizedAccessException("DDO Id is missing");
        }

       
        public string GetDDOCode()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.GroupSid)?.Value
                   ?? throw new UnauthorizedAccessException("DDO Code is missing.");
        }
        public string GetdetailHead()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Sid)?.Value
                   ?? throw new UnauthorizedAccessException("DDo info is missing.");

        }
        public string GetDeptId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.PrimaryGroupSid)?.Value
                   ?? throw new UnauthorizedAccessException("DDo info is missing.");
        }
    }

}

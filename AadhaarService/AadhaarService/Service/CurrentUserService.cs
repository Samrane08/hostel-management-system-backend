﻿using Repository.Interface;
using System.Security.Claims;

namespace AadhaarService.Service;
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string UserNumericId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.PrimarySid);
}

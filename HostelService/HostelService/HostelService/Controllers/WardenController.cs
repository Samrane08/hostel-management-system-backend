using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Implementation;
using Service.Interface;

namespace HostelService.Controllers;
public class WardenController : APIBaseController
{
    private readonly IWardenProfileService wardenProfile;

    public WardenController(IWardenProfileService wardenProfile)
    {
        this.wardenProfile = wardenProfile;
    }
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var result = await wardenProfile.GetWardenProfile();
        if (result != null)
            return Ok(new { Status = true, Profile = result });
        else
            return Ok(new { Status = false, Profile = result });
    }

    [HttpPost]
    public async Task<IActionResult> SaveProfile([FromBody]WardenProfileModel model)
    {
        var result = await wardenProfile.SaveWardenProfile(model);
        if (result != null)
            return Ok(new { Status = true, Message = "Profile Save Successfully" });
        else
            return Ok(new { Status = false, Message = "Profile Save Failed." });
    }

    [HttpPost("warden-map")]
    [AllowAnonymous]
    public async Task<IActionResult> WardenMap([FromBody] WardenMapModel model)
    {
        try
        {
            await wardenProfile.WardenMap(model);
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            throw;
        }
        return NoContent();
    }
}

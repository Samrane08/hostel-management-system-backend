using ApplicantService.Helper;
using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace ApplicantService.Controllers;
public class AccountController : APIBaseController
{
    private readonly ILoginDetailsService loginDetailsService;

    public AccountController(ILoginDetailsService loginDetailsService)
    {
        this.loginDetailsService = loginDetailsService;
    }

    [HttpGet("get-login-details")]
    [AllowAnonymous]
    public async Task<IActionResult> GetLoginDetails(long UserId)
    {
        try
        {
            var result = await loginDetailsService.GetLoginDetails(UserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new { Status = false, ex.Message });
        }
    }

    [HttpPost("save-login-details")]
    [AllowAnonymous]
    public async Task<IActionResult> SaveLoginDetails([FromBody]WebServiceUserModel model)
    {
        try
        {
            var result = await loginDetailsService.SaveLoginDetails(model);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new { Status = false,ex.Message});
        }      
    }

    [HttpGet("check-verified-status")]
    public async Task<IActionResult> VerifiedStatus()
    {
        try
        {
            //var UIDNO = await loginDetailsService.GetAadharreferenceNumber();
            //if (!string.IsNullOrEmpty(UIDNO))
            //{
            //    var qs = QueryStringEncryptDecrypt.DecryptQueryString(UIDNO);
            //    string UID = qs["UID"];
            //    if (!string.IsNullOrEmpty(UID))
            //    {
            //        await loginDetailsService.UpdateOfflineUIDReference(UID);
            //    }
            //}

            var result = await loginDetailsService.VerifyStatus();
            return Ok(result);
        }catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new VerifiedStatusModel());
        }
    }
}

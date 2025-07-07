using AadhaarService.Service;
using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Controllers;
using Repository.Interface;
using Service.Interface;

namespace AadhaarService.Controllers;

public class AadhaarController : APIBaseController
{
    private readonly IUIDService uIDService;

    public AadhaarController(IUIDService uIDService)
    {
        this.uIDService = uIDService;
    }
    [HttpPost("get-reference")]
    [AllowAnonymous]
    public async Task<IActionResult> Post([FromBody] string AadhaarNumber)
    {
        var dec = AesAlgorithm.DecryptString(AadhaarNumber);
        var result = uIDService.GetUIDReference(dec);
        return Ok(result);
    }




    [HttpGet("update-Aadhar-dob")]
  
    public async Task<IActionResult> updateAadhardob(DateTime dob)
    {
        try
        {

            bool result = await uIDService.UpdateAadharDOB(dob.ToString("dd/MM/yyyy"));
            if (result)
            {
                return Ok(new { status = true, message = "Date Updated successfully" });
            }
            else
                return Ok(new { status = false, message = "Date Updated failed" });
           
        }
        catch (Exception ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new { Status = false , Message = "Date Updated Failed" });
        }
    }

}

using Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repository.Entity;
using Repository.Interface;
using UserService.Helper;
using UserService.Model.Request;
using UserService.Service;

namespace UserService.Controllers;

public class ProfileController : APIBaseController
{
    private readonly ICurrentUserService currentUserService;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly APIUrl urloptions;
    private readonly IHttpClientService clientService;

    public ProfileController(ICurrentUserService currentUserService,
                             UserManager<ApplicationUser> userManager,
                             IOptions<APIUrl> urloptions,
                             IHttpClientService clientService)
    {
        this.currentUserService = currentUserService;
        this.userManager = userManager;
        this.urloptions = urloptions.Value;
        this.clientService = clientService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var currentUser = await userManager.Users.Where(x => x.Id == currentUserService.UserId).Select(x => new
            {
                Id = x.Id,
                Name = !string.IsNullOrEmpty(x.Name) ? x.Name : x.UserName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber
            }).FirstOrDefaultAsync();

            return Ok(currentUser);
        }
        catch (System.Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProfileModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user != null)
                {
                    user.PhoneNumber = model.Mobile;
                    user.Email = model.Email;
                    await userManager.UpdateAsync(user);
                }
                return Ok(new { Status = true, Message = "User Updated." });
            }
            else
                return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return BadRequest(new { Status = false, Message = ex.Message });
        }
    }
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePassword model)
    {
        if (!ModelState.IsValid)
        {
            return Ok(model);
        }
        List<string> errors = new List<string>();
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            errors.Add("No user found.");
            return Ok(new {Status = false,Message = "Password change failed.",Error = errors });
        }

        model.CurrentPassword = AesAlgorithm.DecryptString(model.CurrentPassword);
        model.NewPassword = AesAlgorithm.DecryptString(model.NewPassword);

        var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (result.Succeeded)
        {
            try
            {
                await clientService.RequestSend<string>(HttpMethod.Post, $"{urloptions.HostelService}/account/update-first-login", currentUserService.UserId);
            }
            catch (Exception)
            {
            }           
            return Ok(new { Status = true, Message = "Password change successfully." });
        }
       
        foreach (var error in result.Errors)
        {
            errors.Add(error.Description);
        }
        return Ok(new { Status = false, Message = "Password change failed.",Error = errors });
    }
}
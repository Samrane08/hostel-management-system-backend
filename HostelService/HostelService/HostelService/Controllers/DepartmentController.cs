using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace HostelService.Controllers
{
    
    public class DepartmentController : APIBaseController
    {

        private readonly IDepartmentProfileService departmentProfile;

        public DepartmentController(IDepartmentProfileService departmentProfile)
        {
            this.departmentProfile = departmentProfile;
        }
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var result = await departmentProfile.GetDepartmentProfile();
            if (result != null)
                return Ok(new { Status = true, Profile = result });
            else
                return Ok(new { Status = false, Profile = result });
        }

        [HttpPost]
        public async Task<IActionResult> SaveProfile([FromBody] DeskProfileModel model)
        {
            var result = await departmentProfile.SaveDepartmentProfile(model);
            if (result != null)
                return Ok(new { Status = true, Message = "Profile Save Successfully" });
            else
                return Ok(new { Status = false, Message = "Profile Save Failed." });
        }


    }
}

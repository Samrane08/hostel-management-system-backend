using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Model;
using Service.Interface;
using System.Dynamic;

namespace ApplicantService.Controllers;

public class HostelPreferenceController : APIBaseController
{
    private readonly IProfileService profileService;

    public HostelPreferenceController(IProfileService profileService)
    {
        this.profileService = profileService;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {

        var result = await profileService.GetHostelPreference(1);
        if (result != null && result.Count > 0)
        {

            dynamic obj = new ExpandoObject();

            foreach (var item in result)
            {
                if (item.HostelId > 0)
                {
                    ((IDictionary<string, object>)obj)["Preference" + item.Preference] = item.HostelId;
                    ((IDictionary<string, object>)obj)["HostelName"] = !string.IsNullOrWhiteSpace(item.HostelName) ? item.HostelName : "";
                }
            }
            var personalDetails = await profileService.GetPersonalDetails();
            return Ok(new
            {
                Status = true,
                District = result.Select(x => x.District).FirstOrDefault(),
                Taluka = result.Select(x => x.Taluka).FirstOrDefault(),
                IsNewApplicant = personalDetails?.IsNewApplicant,
                Preferences = obj
            });
        }
        else
        {
            return Ok(new { Status = false, Preferences = new ExpandoObject(), Message = "No preference hostel found," });
        }
    }

    [HttpGet("v2/{lang}")]
    public async Task<IActionResult> v2(int lang = 1)
    {
        var result = await profileService.GetHostelPreference(lang);
        if (result != null && result.Count > 0)
        {
            // var personalDetails = await profileService.GetPersonalDetails();
            return Ok(new
            {
                Status = true,
                District = result.Select(x => x.District).FirstOrDefault(),
                Taluka = result.Select(x => x.Taluka).FirstOrDefault(),
                IsNewApplicant = result.Select(x => x.IsNew).FirstOrDefault(),
                Preferences = result.Where(x => x.HostelId > 0).OrderBy(x => x.Preference).Select(x => new { Value = x.HostelId.ToString(), Text = x.HostelName }).ToList()
            });
        }
        else
        {
            return Ok(new { Status = false, District = "", Taluka = "", Preferences = new List<string>(), Message = "No preference hostel found," });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] List<HostelPreferenceModel2> model)
    {
        try
        {
            List<HostelPreferenceModel> model2 = new List<HostelPreferenceModel>();
            if (model.Count > 0)
            {
                int cnt = 1;
                foreach (var item in model)
                {
                    if (item.Value > 0)
                    {
                        model2.Add(new HostelPreferenceModel { HostelId = item.Value, Preference = cnt });
                        cnt++;
                    }
                }
            }

            var personalDetails = await profileService.GetPersonalDetails();

            if (personalDetails != null && personalDetails.IsNewApplicant == false)
                return Ok(new { Status = false, Message = "Existing applicant not allowed to change preference." });

            var paymentStatus = await profileService.GetPaymentParams();
            if (paymentStatus != null)
            {
                if (!string.IsNullOrEmpty(paymentStatus.AppId) && paymentStatus._ScrutinyStatus != 5)
                    return Ok(new { Status = false, Message = "Application already applied.No changes allowed." });
            }
            if (model2.Count > 0)
            {
               // await profileService.SaveHostelPreference(model);
                await profileService.SaveHostelPreferenceV2(model2);

                var result = model2.OrderBy(x => x.Preference).ToList();

                if (result != null && result.Count > 0)
                {
                    dynamic obj = new ExpandoObject();

                    foreach (var item in result)
                    {
                        if (item.HostelId != 0)
                            ((IDictionary<string, object>)obj)["Preference" + item.Preference] = item.HostelId;
                    }

                    return Ok(new
                    {
                        Status = true,
                        District = "",
                        Taluka = "",
                        Preferences = obj
                    });
                }
                else
                {
                    return Ok(new { Status = false, Preferences = new ExpandoObject(), Message = "No preference hostel found," });
                }
            }
            else
            {
                return Ok(new { Status = false, Message = "Preference Set Failed." });
            }
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new { Status = false, Message = ex.Message });
        }


    }

    [HttpGet("HostelList/{Lang}")]
    public async Task<IActionResult> HostelList(int? Lang = 1)
    {
        var result = await profileService.GetHostelList(Lang);
        return Ok(result);
    }
}

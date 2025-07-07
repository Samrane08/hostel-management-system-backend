using Helper;

namespace Model;

public class VerifiedStatusModel
{
    public bool IsMobileVerified { get; set; }
    public bool IsEmailVerified { get; set; }
    public bool IsAadharVerified { get; set; }
    public bool? HostelExistingEnable { get; set; }
    public bool? HostelNewEnable { get; set; }
    public bool? SwayamExistingEnable { get; set; }
    public bool? SwayamNewEnable { get; set; }
    public bool? AadharExistingEnable { get; set; }
    public bool? AadharNewEnable { get; set; }
    public bool? ProceedToPay { get; set; }

    public string? mararthiMeassage {  get; set; }
    public string? engMessage {  get; set; }
    public string? DOB { get; set; }
    public bool? ApplicantAgeValidate
    {
        get
        {
            var date = Utility.ConvertToDate(DOB);
            if (date.HasValue) { if ((DateTime.Now.Year - date.Value.Year) <= 30) { return true; } else return false; } else { return true; }
        }
    }
}

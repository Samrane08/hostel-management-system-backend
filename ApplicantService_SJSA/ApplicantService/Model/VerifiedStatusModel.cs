namespace Model;

public class VerifiedStatusModel
{
    public bool IsMobileVerified { get; set; }
    public bool IsEmailVerified { get; set; }
    public bool IsAadharVerified { get; set; }
    public bool? HostelExistingEnable { get; set; }
    public bool? HostelNewEnable { get; set; }
    public bool? SwadharExistingEnable { get; set; }
    public bool? SwadharNewEnable { get; set; }
    public bool? ProceedToPay { get; set; }

    public string? mararthiMeassage {  get; set; }
    public string? engMessage {  get; set; }
}

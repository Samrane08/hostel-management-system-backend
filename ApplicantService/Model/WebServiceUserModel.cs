using System.ComponentModel.DataAnnotations;

namespace Model;

public class WebServiceUserModel
{
    public int ID { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? EmailID { get; set; }
    public string? MobileNo { get; set; }
    public string? FullName { get; set; }
    public string? FullNameInMarathi { get; set; }
    public string? Gender { get; set; }
    public string? DateofBirth { get; set; }
    public string? UIDNO { get; set; }
    public string? PANNO { get; set; }
    public string? TrackID { get; set; }
    public string? UserID { get; set; }
    public long? NumericID { get; set; }
    public string? UserIdentity { get; set; }
    public string? Domain { get; set; }
    public bool? IsEmailVerified { get; set; }
    public bool? IsMobileVerified { get; set; }
}

public class ProfileVerifyModel
{
	[Required]
	[StringLength(10)]
	public string MobileNo { get; set; }
	[Required]
	public string EmailID { get; set; }
}

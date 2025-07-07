using Helper;

namespace Model;
public class UIDResponseModel
{
    public int Id { get; set; }
    public string? ApplicantName { get; set; }
    public string? ApplicantName_LL { get; set; }
    public string? Gender { get; set; }
    public string? MobileNo { get; set; }
    public string? Address { get; set; }
    public string? StateName { get; set; }
    public string? DistrictName { get; set; }
    public string? TalukaName { get; set; }
    public string? Pincode { get; set; }
    public string? Email { get; set; }
    public string? DateOfBirth { get; set; }
    public string? DOBFormat
    {
        get
        {
            if(!string.IsNullOrEmpty(DateOfBirth) && DateOfBirth.Length == 4)
            {
                return DateOfBirth;
            }
            var birthDate = Utility.ConvertToDate(DateOfBirth);
            if (birthDate.HasValue)
            {
                return birthDate.Value.ToString("dd-MM-yyyy");
            }
            else
                return "";
        }
    }
    public int Age { get; set; }
    public string? ReferenceNo { get; set; }
    public string? UIDNo { get; set; }
    public string? ApplicantImage_string { get; set; }
    public string? Message { get; set; }
}
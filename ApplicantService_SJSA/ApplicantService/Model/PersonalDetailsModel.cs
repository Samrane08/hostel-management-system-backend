using Helper;

namespace Model;

public class PersonalDetailsModel
{
    public int? ServiceType { get; set; }
    public bool IsNewApplicant { get; set; }
    public int? TypeOfCourse { get; set; }   
    public int? HostelDistId { get; set; }
    public int? HostelTalukaId { get; set; }
    public int? HostelId { get; set; }
    public string? ApplicantName { get; set; }
    public string? DOB { get; set; }
    public string? DOBFormat
    {
        get
        {
            if(!string.IsNullOrEmpty(DOB) && DOB.Length == 4)
            {
                return DOB;
            }
            else
            {
                var date = Utility.ConvertToDate(DOB);
                if (date.HasValue)
                {
                    return date.Value.ToString("dd-MM-yyyy");
                }
                else
                    return DOB;
            }
        }
    }
    public string? Mobile { get; set; }
    public int? Gender { get; set; }
    public int? Age 
    { 
        get 
        {
            var date = Utility.ConvertToDate(DOB);
            if (date.HasValue) { return DateTime.Now.Year - date.Value.Year; } else { return 0; } 
        }
    } 
    public int? MaritalStatus { get; set; }
    public bool? IsOrphan { get; set; }
    public string? FatherName { get; set; }   
    public string? MotherName { get; set; }   
    public string? GuardianName { get; set; }  
    public bool? IsFatherAlive { get; set; }
    public bool? IsMotherAlive { get; set; }
    public int? CasteCategory { get; set; }
    public int? Caste { get; set; }
    public string? CertificateNumber  { get; set; }
    public string? CastCertificateApplicantName { get; set; }
    public int? CertificateIssueDistrict { get; set; }
    public int? IssuingAuthority { get; set; }
    public DateTime? IssueDate { get; set; }
    public bool? IsMaharastraDomicile { get; set; }
    public string? DomicileCertificateNo { get; set; }
    public int? DomicileIssuingAuthority { get; set; }
   
}

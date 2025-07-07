using MasterService.Service.Utility;
namespace MasterService.Models.ReqModel
{
    public class PersonalDetailsModel
    {
     
       
        public int? MaritalStatus { get; set; }
        public bool? IsOrphan { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? GuardianName { get; set; }
        public bool? IsFatherAlive { get; set; }
        public bool? IsMotherAlive { get; set; }
        public bool? IsSignUploaded { get; set; }
        public string? SignFile { get; set; }
        public int? CasteCategory { get; set; }
        public int? Caste { get; set; }
        public string? CertificateNumber { get; set; }
        public string? CastCertificateApplicantName { get; set; }
        public int? CertificateIssueDistrict { get; set; }
        public int? IssuingAuthority { get; set; }
        public DateTime? IssueDate { get; set; }
        public bool? IsMaharastraDomicile { get; set; }
        public string? DomicileCertificateNo { get; set; }
        public int? DomicileIssuingAuthority { get; set; }
    }

    public class PrePersonalDetailsModel
    {


        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
       
        public string? IsFatherAlive { get; set; }
        public string? IsMotherAlive { get; set; }
       
      
        public string? Caste { get; set; }
       
        public string? IsMaharastraDomicile { get; set; }
    
    }

}

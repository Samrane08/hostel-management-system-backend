namespace Model
{
    public class OfflineRegistrationModel
    {

        // public string? UIDNo { get; set; }
        //public string? UIDNoref { get; set; }

        public List<OfflineAadhharList> aadhaarData { get; set; }
        public string? FilePath { get; set; }
    }

    public class OfflineAadhharList
    {
        public string? UIDNo { get; set; }
        public string? ApplicantName { get; set; }
        public string? CourseType { get; set; }
        public string? AdmissionYear { get; set; }

        public string? EncryptedAadhar { get; set; }

        public string? Message { get; set; }

    }
    public class GetOfflineAadhharList
    {
        public string? UIDNo { get; set; }
        public string? ApplicantName { get; set; }
        public string? CourseType { get; set; }

        public string? EncryptedAadhar { get; set; }

        public string? HostelId { get; set; }

        public string? Message { get; set; }
    }
    public class SearchExistingAadharDataModel : FilterParamModel
    {
        public string? HostelId { get; set; }
    }

    public class ExistingAadharData
    {
        public int? Id { get; set; }
        public string? EncryptedAadhar { get; set; }
        public string? Aadhar { get; set; }
        public string? ApplicantName { get; set; }
        public string? HostelName { get; set; }
        public string? coursetype { get; set; }
        public string? ExcelAdmissionYear { get; set; }
        public string? ApplicationType { get; set; }
        public string? ServiceType { get; set; }
        public string? CreatedOn { get; set; }
        public string? ApplicationNo { get; set; }
    }

    public class DirectVacancyCount
    {
        public int? exitingStudentCount { get; set; }
        public int? newStudentCount { get; set; }
    }

    public class SearchDeleteExistingAadharDataModel : FilterParamModel
    {
        public string? HostelId { get; set; }
    }

    public class DeleteOfflineRegistrationModel
    {

        // public string? UIDNo { get; set; }
        //public string? UIDNoref { get; set; }

        public List<DeleteOfflineAadhharList> aadhaarData { get; set; }
        public int HostelId { get; set; }
    }

    public class DeleteOfflineAadhharList
    {
        public string? UIDNo { get; set; }
        public string? EncryptedAadhar { get; set; }
        public string? Message { get; set; }

    }
}
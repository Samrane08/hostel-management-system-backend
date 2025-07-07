namespace Model
{
    public class ApplicationModel
    {
        public long Id { get; set; }
        public string? AppId { get; set; }
        public string? Name { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public int? PaymentStatusId { get; set; }
        public string? Message { get; set; }
        public string? UserId { get; set; }
        public int? ProceedToPay { get; set; }
        public int? _ScrutinyStatus { get; set; }
        public int? ServiceType { get; set; }

    }

    public class ApplicationMainModel
    {
        public long Id { get; set; }
        public string? ApplicationNo { get; set; }
        public string? Name { get; set; }
        public string? ServiceName { get; set; }
        public string? ApplicationType { get; set; }
        public string? ApplicationDate { get; set; }
        public string? PaymentStatus { get; set; }
        public string? ApplicationStatus { get; set; }
        public int? PaymentStatusId { get; set; }       
        public ApplicationModel? PaymentParam { get; set; }
        public List<ApplicationModel2>? HostelList { get; set; }
    }

    public class ApplicationModel2
    {
        public long? ApplicationId { get; set; }
        public int? HostelId { get; set; }
        public string? HostelName { get; set; }
        public int? Prefrence { get; set; }
        public string? Status { get; set; }
        public int? StatusId { get; set; }
        public bool IsAlloted { get; set; }
        public bool Accept { get; set; }
        public bool IsAdmitted { get; set; }
        public string Remark { get; set; } = "";
    }
    public class OfflineApplicationModel
    {
        public int? IsOfflineApplication { get; set; }
        public int? typeOfCourse { get; set; }
    }

    public class AdmittedApplicationModel
    {
        public int? IsAdmittedApplication { get; set; }
        public int? typeOfCourse { get; set; }
    }

    public class ApplicationInstallmentModel
    {
        public string? message { get; set; }
        public int? documentSize { get; set; }
    }


    public class DbtApplicantStatusModel
    {
        public int? Result { get; set; }
        public string? AadharRefNo { get; set; }
    }

    public class ApplicationCommonModel
    {
        public long Id { get; set; }
        public string? ApplicationNo { get; set; }
        public string? Name { get; set; }
        public string? ServiceName { get; set; }
        public string? ApplicationType { get; set; }
        public string? ApplicationDate { get; set; }
        public string? PaymentStatus { get; set; }
        public string? ApplicationStatus { get; set; }
        public int? PaymentStatusId { get; set; }
        public ApplicationModel? PaymentParam { get; set; }
       // public List<ApplicationModel2>? HostelList { get; set; }
        public string? Departmrnt { get; set; }
        public string? DeptId { get; set; }

        public string? IsActive { get; set; }

    }
}
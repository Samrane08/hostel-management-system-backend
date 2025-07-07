

namespace Model;

public class CurrentCourseDetails
{
    public long? Id { get; set; }
    public string? UserId { get; set; }
    public int? AdmissionYear { get; set; }
    public int? StateID { get; set; }
    public int? DistrictID { get; set; }
    public int? TalukaID { get; set; }
    public int QualificationTypeID { get; set; }
    public int? StreamID { get; set; }
    public int? CollegeTypeID { get; set; }
    public int? CourseTypeID { get; set; }
    public int? AdmissionTypeID { get; set; }
    public int? IsCompletedOrContinue { get; set; }
    public int? CourseYrID { get; set; }
    public int? CourseCategoryId { get; set; }
    public int? IsAdmissionThroughOpenOrResID { get; set; }
    public int? GapYears { get; set; }
    public bool? IsCompleted { get; set; }
    public string GapReason { get; set; } = string.Empty;
    public int? EducationMode { get; set; }
    public decimal? Percentage { get; set; }
    public int? Result { get; set; }
    public int? StartYear { get; set; }
    public DateTime? Admissiondate { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? CreatedOn { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedOn { get; set; }
    public decimal CGPA { get; set; }
    public string? CGPAFileId { get; set; }
    public string? CollegeName { get; set; }
}
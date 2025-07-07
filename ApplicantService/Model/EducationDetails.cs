

namespace Model;

public class EducationDetails
{
    public int?rowId {  get; set; }
    public string QualificationLevel { get; set; }
    public string StreamID { get; set; }
    public string CompletedOrContinue { get; set; } = "1";
    public string StateID { get; set; }
    public string DistrictID { get; set; }
    public string TalukaID { get; set; }
    public string CollegeID { get; set; }
    public string? CourseID { get; set; }
    public string? BoardUniversity { get; set; }
    public string Mode { get; set; }
    public string AdmissionYear { get; set; }
    public string PassingYear { get; set; }
    public string? Result { get; set; }
    public string ?Percentage { get; set; }
    public string? Attempts { get; set; }
    public string? IsGap { get; set; }
    public string? GapYears { get; set; }
    public string? CollegeName { get; set; }
    public string? CourseName { get; set; }
    public string? UniversityName { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public decimal? CGPA { get; set; }

    public string? CGPAFileId { get; set; }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CourseDetailsTblViewModel
    {
        public int? Id { get; set; }
        public string? college_name { get; set; }
        public string? YearTxt { get; set; }
        public string? course_name { get; set; }
        public string? course_type { get; }
        public string? Text { get; set; }
        public string? StatusName { get; set; }
        public bool? IsCompleted { get; set; }
        public string AdmissionYear { get; set; } = string.Empty;
        public string StateID { get; set; } = string.Empty;
        public string DistrictID { get; set; } = string.Empty;
        public string TalukaID { get; set; } = string.Empty;
        public string QualificationTypeID { get; set; } = string.Empty;
        public string StreamID { get; set; } = string.Empty;
        public string CollegeTypeID { get; set; } = string.Empty;
        public string CourseTypeID { get; set; } = string.Empty;
        public string AdmissionTypeID { get; set; } = string.Empty;
        public string IsCompletedOrContinue { get; set; } = string.Empty;
        public string CourseYrID { get; set; } = string.Empty;
        public string CourseCategoryId { get; set; } = string.Empty;
        public string IsAdmissionThroughOpenOrResID { get; set; } = string.Empty;
        public string GapYears { get; set; } = string.Empty;
        public string GapReason { get; set; } = string.Empty;
        public string EducationMode { get; set; } = string.Empty;
        public string Percentage { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string StartYear { get; set; } = string.Empty;
        public string Admissiondate { get; set; } = string.Empty;
        public string CGPAFileId { get; set; } = string.Empty;

        public string CollegeName { get; set; } = string.Empty;
    }
}

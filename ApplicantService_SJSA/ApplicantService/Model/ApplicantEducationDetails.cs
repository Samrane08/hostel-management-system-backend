using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ApplicantEducationDetails
    {
        public long? RowID { get; set; }
        public long? UserID { get; set; }
        public int? QualificationTypeID { get; set; }
        public int? DegreeNameID { get; set; }
        public bool? IsCompletedOrPursuing { get; set; }
        public int? DegreeWiseYearID { get; set; }
        public int? SubjectID { get; set; }
        public int? StateID { get; set; }
        public int? BoardUniversityID { get; set; }
        public string Result { get; set; }=string.Empty;
        public DateTime? AdmissionDate { get; set; }
        public DateTime? ResultDate { get; set; }
        public int? Attempts { get; set; }
        public decimal? Percentage { get; set; }
        public int? CourseDurationInMonths { get; set; }
        public int? ClassOrGradeID { get; set; }
        public int? ModeID { get; set; }
        public string CompulsorySubject { get; set; } =string.Empty;
        public string OptionalSubject { get; set; }=string.Empty;
        public bool? IsGap { get; set; }
        public int? GapYears { get; set; }
        public int? MappingID { get; set; }
        public string Rollnumber { get; set; }=string.Empty;
        public string exsession { get; set; }=string.Empty;
        public int? totalmarks { get; set; }
        public int? totalMarksObtained { get; set; }
        public string PassingYr { get; set; }=string.Empty;
        public int? AdmissionYr { get; set; }
        public int? DistrictID { get; set; }
        public int? TalukaID { get; set; }
        public int? CollegeID { get; set; }
        public string CollegeSchoolName { get; set; }=string.Empty ;
        public string CourseName { get; set; }=string.Empty ;
        public string UniversityName { get; set; }=string.Empty ;
        public bool? IsActive { get; set; }
        public string FlagStatus { get; set; } = string.Empty;
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

}

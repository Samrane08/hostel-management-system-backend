using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Asn1.Cmp;
using System.ComponentModel.DataAnnotations;

namespace MasterService.Models.ReqModel
{
    public class CurrentCourseDetailsData
    {
        public long RowID { get; set; }
        public long UserId {  get; set; }
        public int? AdmissionYear { get; set; }
        public int? StateID { get; set; }
        public int? DistrictID { get; set; }
        public int? TalukaID { get; set; }
        public int? CollegeID { get; set; }
        public int? CourseID { get; set; }
        public int? MappingID { get; set; }
        public int? CurrentAcademicyear { get; set; }
        public int? UniversityID { get; set; }
        public int? GrantTypeID { get; set; }
        public int? CourseTypeID { get; set; }
        public int? CollegeTypeID { get; set; }
        public DateTime? DataOfAdmission { get; set; }
        public bool? ISForeignCourse { get; set; }
        public bool? IsCompletedOrContinue { get; set; }
        public int? Attempts { get; set; }
        public double? Percentage { get; set; }
        public int? ClassOrGrade { get; set; }
        public DateTime? ResultDate { get; set; }
        public bool? IsGap { get; set; }
        public int? GapYears { get; set; }
        public string CreatedBy { get; set; }=string.Empty;
        public DateTime? CreatedOn { get; set; }
        public string Remarks { get; set; }= string.Empty;
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string FlagStatus { get; set; } = string.Empty;
        public int? QualificationTypeID { get; set; }
        public int? StreamID { get; set; }
        public int? ResultYearID { get; set; }
        public string CapID { get; set; } = string.Empty;
        public int? CourseYrID { get; set; }
        public int? AdmissionTypeID { get; set; }
        public int? IsAdmissionThroughOpenOrResID { get; set; }
        public int? EducationMode { get; set; }
        public decimal? FeesPaid { get; set; }
        public string GRNNo { get; set; } = string.Empty;
        public int? GapReason { get; set; }
        public double? CAP_Percentage { get; set; }
        public string IPAddress { get; set; }=string.Empty;

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }

}
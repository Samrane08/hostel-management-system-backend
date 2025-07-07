using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class ApplicantPreSchoolRecord
    {
        public int? Id { get; set; }
     
        public int StateID { get; set; }
        public int DistrictID { get; set; }
        public long TalukaID { get; set; }
        public string ?SchoolName { get; set; }
        public string ?SchoolUDISE { get; set; }
        public int Standard { get; set; }
        public int Result { get; set; }
        public decimal? PrevAttendence { get; set; } 
        public decimal? PrevPercentage { get; set; } 
        public int? PrevRank { get; set; } 
        public bool? IsGap { get; set; } 
        public DateTime? AdmissionDate { get; set; }
        public bool? IsCompleted { get; set; }
        public string? GapReason { get; set; }
    }
    public class ApplicantPreSchoolRecordView
    {
        public string? Id { get; set; }

        public string? statename { get; set; }
        public string? Districtname { get; set; }
        public string? School_Name { get; set; }
        public string? Standard { get; set; }
        
    
        public DateTime? Admissiondate { get; set; }
        public decimal? prevAttendence { get; set; }
        public decimal? Prevpercentage { get; set; }
        public string? PrevRank { get; set; }
        public bool? IsCompleted { get; set; }
        public int? StandardID { get; set; }
        public string? StartYear { get; set; }
        public string? PassingYear { get; set; }
        public string? percentage { get; set; }

        public string? Rank { get; set; }
        public string? GapReason { get; set; }
        public string ? CompletedOrPursuing { get; set; }



    }
    public class ApplicantPreschoolRecordPast
    {
        public int? Id { get; set; }
        public int? StateId { get; set; }


        public int? DistrictId { get; set; }

        public long? TalukaId { get; set; }


        public string? SchoolName { get; set; }


        public string? SchoolUdise { get; set; }


        public int? Standard { get; set; }


        public int? Result { get; set; }


        public decimal? Percentage { get; set; }

        public int? Rank { get; set; }

        public bool? IsGap { get; set; }


        public bool? IsCompleted { get; set; }

        public int? StartYear { get; set; }


        public int? PassingYear { get; set; }
        public string? GapReason { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AttendanceDetailsModel
    {
        public string AcademicYear { get; set; } = string.Empty;
        public string InstallmentNo { get; set; } = string.Empty;
     
        public string AttendanceDoc { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public string AttendancePercentage { get; set; } = string.Empty;
        public string PercentageOfMarks { get; set; } = string.Empty;
        public string PostMatricScholarshipAmount { get; set; } = string.Empty;
        public string isPostMatricScholarship { get; set; } = string.Empty;
    }
}

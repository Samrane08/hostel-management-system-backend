using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AttendanceDetailsUploadedByApplicantModel
    {
        public string ApplicantFullName { get; set; } = string.Empty;
        public string AcademicYear { get; set; } = string.Empty;
        public string InstallmentNumber { get; set; } = string.Empty;
        public string AttendanceDoc { get; set; } = string.Empty;
        public string AttendancePercentage { get; set; } = string.Empty;
        public string PercentageOfMarks { get; set; } = string.Empty;
        public string isPostMatricScholarship { get; set; } = string.Empty;
        public string PostMatricScholarshipAmount { get; set; } = string.Empty;
    }
}

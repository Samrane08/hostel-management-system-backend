using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class OtherDetailsModel
    {
        public int? UserId { get; set; }
        public int isAvailedMaintainaceAllowance { get; set; }
        public string? AvailedMaintainaceAllowanceUserId { get; set; }
        public int AreYouSalariedOrSelfEmployed { get; set; }
        public int DoYouDoAnyKindOfBusiness { get; set; }
        public int IsLivedInGovermentHostel { get; set; }
        public int IsApplicantDisable { get; set; }
        public string DisabilityPercentage { get; set; }
        public int? NameOfHostel { get; set; }
        public int? HostelDistrict { get; set; }
        public DateTime? DurationFrom { get; set; }
        public DateTime? DurationTo { get; set; }
        public int? IsMaterialsReturn { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DepartmentProfileModel
    {
        public int? DistrictId { get; set; }
        public string? UserIdentity { get; set; }
        public int? WorkFlowId { get; set; }
        public bool? IsFirstLogin { get; set; }
    }
}

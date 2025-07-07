using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UpdateAdmissionStatusRequestModel
    {
        public int? AppAllotmentId { get; set; }
        public int AdmissionStatus { get; set; }
        public string? Remarks { get; set; } = "";
        public List<int>? RejectReasonsIds { get; set; } 
    }
}

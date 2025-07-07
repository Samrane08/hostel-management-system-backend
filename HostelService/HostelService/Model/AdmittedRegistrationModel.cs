using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AdmittedRegistrationModel
    {
        public List<AdmittedAadhharList> aadhaarData { get; set; }
        public string? courseType { get; set; }
    }
    public class AdmittedAadhharList
    {
        public string? UIDNo { get; set; }
        public string? UIDNoref { get; set; }

        public string? EncryptedAadhar { get; set; }

    }
    public class GetAdmittedAadhharList
    {
        public string? UIDNo { get; set; }
        public string? UIDNoref { get; set; }

        public string? CourseType { get; set; }

    }
}

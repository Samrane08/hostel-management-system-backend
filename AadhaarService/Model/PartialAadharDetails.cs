using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PartialAadharDetails
    {
        public string ApplicantName {  get; set; }=string.Empty;
        public long UserId {  get; set; }
    }
    public class Applicantdetails
    {
        public string EmailId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserAadhaarModel
    {
       
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string LoginMobile { get; set; }
        public string? LoginEmail { get; set; }
        public string RegisteredMobileNo { get; set; }
        public string? RegisteredEmail { get; set; }
    }
}

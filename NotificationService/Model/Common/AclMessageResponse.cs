using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public class AclMessageResponse
    {
        public string? msgid { get; set; }
        public string? respid { get; set; }
        public string? error { get; set; }
        public bool? accepted { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public class EnvironmentSetting
    {
        public string? Environment { get; set; }
        public string? LocalUrl { get; set; }
        public string? UATUrl { get; set; }
        public string? ProdUrl { get; set; }
    }
}

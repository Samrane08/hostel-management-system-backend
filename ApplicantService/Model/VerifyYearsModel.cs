using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class VerifyYearsModel
    {
        public List<Drp> yearsStatus {  get; set; }
        public string Result {  get; set; }=string.Empty;
        public bool IsValidYear {  get; set; }
        public string errormsg {  get; set; }=string.Empty ;
    }

    public class IsValidResult
    {
        public string Result { get; set; } = string.Empty;
    }
    public class Drp
    {
        public int Value { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}

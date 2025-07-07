using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SelectListModel
    {
        public string? Value { get; set; }
        public string? Text { get; set; }
        public string? FinancialYear { get; set; }
    }

    public class SelectListAttendence
    {
        public int? Id { get; set; }
        public string? ApplicationNo { get; set; }
    }
}

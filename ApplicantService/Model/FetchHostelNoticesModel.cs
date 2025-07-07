using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class FetchHostelNoticesModel
    {
        public string? NotificationText { get; set; } = "";
        public string? NotificationHeader { get; set; } = "";
        public string? DocumentId { get; set; } = "";
        public string? CreatedOn { get; set; } = "";
    }
}

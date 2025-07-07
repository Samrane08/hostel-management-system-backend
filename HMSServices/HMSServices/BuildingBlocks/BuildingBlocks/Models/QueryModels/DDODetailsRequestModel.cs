using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.QueryModels
{
    public class DDODetailsRequestModel
    {

        public int SchemeId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}

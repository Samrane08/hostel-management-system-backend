using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.QueryModels
{
    public class ExportBeneficiaryExcelQueryModel
    {
        public string BatchID { get; set; } = string.Empty;
        public int SchemeId { get; set; }=0;
    }
}

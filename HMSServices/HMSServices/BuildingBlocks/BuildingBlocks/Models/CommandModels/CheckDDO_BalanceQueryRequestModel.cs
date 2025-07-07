using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.CommandModels
{
    public class CheckDDO_BalanceQueryRequestModel
    {
       public int? Year1 { get; set; }
        public int? Year2 { get; set; }
        public int SchemeDDOMapID { get; set; }
       
          public string Schemecode {  get; set; }=string.Empty;
         
            public bool IsActive {  get; set; }
           public string CreatedBy { get; set; } = string.Empty;

        public string totalApplicantAmt { get; set; } = string.Empty;
        public string DDOCode { get; set; } = string.Empty;
       
        public string DetailsHead { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int? CheckReqId { get; set; }
        public int financialYearId { get; set; }
    }
}

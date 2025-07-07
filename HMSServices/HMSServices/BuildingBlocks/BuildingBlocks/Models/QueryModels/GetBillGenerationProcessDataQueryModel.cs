using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.QueryModels
{
    public class GetBillGenerationProcessDataQueryModel
    {
        public int p_SchemeID { get; set; } = 0;
        public string p_DDOCode { get; set; }=string.Empty;

        public int p_InstallmentFlag { get; set; }=0;

        public int p_financialYearID { get; set; }=0;

        public string searchTerm { get;set; }=string.Empty;

        public string UserId { get; set;} = string.Empty;
        public string DDO_Code {  get; set; } = string.Empty;

        public int pazeNumber { get;set; }=0;
        public int pazeSize { get; set;}=0;
    }

    public class GetBillGenerationProcessDataResponseModel
    {
        public int BEAMSBillGenerationID { get; set; } 
        public int SchemeID { get; set; }
        public string BeamsSchemeCode { get; set; }=string.Empty ;
        public string DDOCode { get; set; } = string.Empty;
        public int TotalNoBefAllocatedCnt { get; set; }
        public decimal AllocatedAmount { get; set; }
        public int BatchID { get; set; }
        public bool IsProcessed { get; set; }
        public string TreasuryTokenNo { get; set; } = string.Empty;
        public int Installment { get; set; }
        public int FinancialYearID { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}

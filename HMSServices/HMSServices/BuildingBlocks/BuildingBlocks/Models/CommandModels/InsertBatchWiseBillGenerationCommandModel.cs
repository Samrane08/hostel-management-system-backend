using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.CommandModels
{
    public class InsertBatchWiseBillGenerationCommandModel
    {

        public int SchemeID { get; set; }=int.MinValue;
        public string DDOCode { get; set; }=string.Empty;
        public string SchemeCode { get; set; } = string.Empty;
        public int TotalNoBefAllocatedCnt { get; set; } = int.MinValue;
        public decimal AllocatedAmount { get; set; }=decimal.MinValue;
        public int Year2 { get; set; } = 0;
        public int Year1 { get; set; } = 0;
        public string CreatedBy { get; set; }= string.Empty;
        public string OfficePaymentNumber { get; set; } = string.Empty;
        public string SubSchemeCode { get; set; } = string.Empty;
        public int InstallmentFlag { get; set; } = 0;
        public int FinancialYearID { get; set; } = 0;
        public string IPAddress { get; set; } = string.Empty;
        public string BillType { get; set; }=string.Empty ;

        public string DDLDDOACtionsValue { get; set; }=string.Empty ;
        public string UserId { get; set; } = string.Empty ;
    }
}

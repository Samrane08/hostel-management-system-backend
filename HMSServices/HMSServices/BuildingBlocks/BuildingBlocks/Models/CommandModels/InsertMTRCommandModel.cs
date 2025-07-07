using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.CommandModels
{
    public class InsertMTRCommandModel
    {
        public string PaybillId { get; set; }=string.Empty;
        public int PayYear { get; set; }
        public string PayMonth { get; set; } = string.Empty;
        public int BeneficiaryCount { get; set; }
        public string GrossAmount { get; set; } = string.Empty;
        public int FinYear2 { get; set; }
        public int FinYear1 { get; set; }
        public string FormId { get; set; }=string.Empty ;
        public string PaymentMode { get; set; } = string.Empty;
        public string BifurcatedGISDedMap { get; set; }=string.Empty;
        public string TotalDeduction { get; set; } = string.Empty;
        public string SchemeCode { get; set; } = string.Empty;
        public string DetailHead { get; set; } = string.Empty;
        public string DDOCode { get; set; } = string.Empty;
        public string BulkFlag { get; set; } = string.Empty;
        public string BillCreationDate { get; set; }=string.Empty;
        public int PayeeCount { get; set; }
        public string BillType { get; set; } = string.Empty;
        public string PayeeType { get; set; } = string.Empty;
        public string MTRBillPortalName { get; set; } = string.Empty;
        public string XMLString { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;

        public int BeamsBillRequestID { get; set; }
        public int MTR_Year2 { get; set; }
        public int MTR_Year1 { get; set; }
        public string MTR_expTotal { get; set; } = string.Empty;
        public string MTR_TotalBudget { get; set; } = string.Empty;
        public string MTR_TransNo { get; set; } = string.Empty;
        public string MTR_ValidTo { get; set; } = string.Empty;
        public string MTR_additionalFields { get; set; } = string.Empty;
    }
}

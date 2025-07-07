using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.QueryModels
{
    public class BillDetailsModel
    {
        public string OfficePaymentNumber { get; set; } = string.Empty;
        public int SchemeID { get; set; }
        public string SchemeName { get; set; } = string.Empty;
        public string BeamsSchemeCode { get; set; } = string.Empty;
        public string BatchID { get; set; } = string.Empty;

        public string TotalNoBefAllocatedCnt { get; set; } = string.Empty;

        public string AllocatedAmount { get; set; } = string.Empty;
        public string financialYear { get; set; } = string.Empty;
        public string ddo_code { get; set; } = string.Empty;
        public string MTR_additionalFields { get; set; } = string.Empty;
        public string MTR_TreasuryName { get; set; } = string.Empty;
        public string MTR_DemandNo { get; set; } = string.Empty;

        public string MTR_AdminDept { get; set; } = string.Empty;

        public string MTR_MajorHead { get; set; } = string.Empty;
        public string MTR_MinorHead { get; set; } = string.Empty;
        public string MTR_subhead { get; set; } = string.Empty;
        public string MTR_DetailHead { get; set; } = string.Empty;
        public string MTR_SubDetailHead { get; set; } = string.Empty;
        public string MTR_SchemeCode { get; set; } = string.Empty;
        public string MTR_Designation { get; set; } = string.Empty;
        public string MTR_PanNo { get; set; } = string.Empty;
        public string MTR_IFSCCode { get; set; } = string.Empty;
        public string MTR_BankName { get; set; } = string.Empty;
        public string MTR_BankBranchName { get; set; } = string.Empty;
        public string MTR_AccountNo { get; set; } = string.Empty;
        public string AllocatedAmountInWords { get; set; } = string.Empty;

        public string MTR_AccountHolder { get; set; } = string.Empty;

        public string MTR_InwordsExtraOneRuppes { get; set; } = string.Empty;

        public string AuthNo { get; set; } = string.Empty;
        public DateTime? AuthDate { get; set; }

        public string MTR_expTotal { get; set; } = string.Empty;


        public string MTR_TotalBudget { get; set; } = string.Empty;

        public string MTR_RemainingBudget { get; set; } = string.Empty;

        public string MTR_ValidTo { get; set; } = string.Empty;

        public string MTR_Year1 { get; set; } = string.Empty;

        public string MTR_Year2 { get; set; } = string.Empty;

        public string PayMonth { get; set; } = string.Empty;
        public string PayYear { get; set; } = string.Empty;
        public string PayMonthWord { get; set; } = string.Empty;


    }
    public class ExcelExportModel
    {
        public BillDetailsModel BillDetails { get; set; }
        public List<BeneficiaryQueryModel> Beneficiaries { get; set; }


    }
}

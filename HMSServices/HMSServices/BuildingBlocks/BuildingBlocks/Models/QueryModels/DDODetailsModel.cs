
namespace BuildingBlocks.Models.QueryModels
{
    public class DDODetailsModel
    {
        public int DDOID { get; set; }
        public int DeptID { get; set; }
        public int SchemeID { get; set; }
        public string IntegrationSchemeCode { get; set; }=string.Empty;
        public string DetailHead { get; set; } = string.Empty;
        public string DDO_Code { get; set; } = string.Empty;
        public string BeneficiaryType { get; set; } = string.Empty;
        public string PurposeOfPaymentCode { get; set; } = string.Empty;
        public string UserID { get; set; } = string.Empty;
        public string DDO_Name { get; set; }=string.Empty ;
        public string DDO_Email { get; set; } = string.Empty;
        public string DDO_MobileNo { get; set; }=string.Empty;
        public string Address { get; set; } = string.Empty;
        public string DDO_Designation { get; set; }=string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountHolderName { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public string IFSCCode { get; set; } = string.Empty;
        public string FormID { get; set; } = string.Empty;
        public string BillType { get; set; } = string.Empty;
        public string PaymentMode { get; set; } = string.Empty;
        public string PayeeType { get; set; }=string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }
    public class DDOCheckBalanceQueryModel
    {
      
        public string totalApplicantAmt { get; set; }= string.Empty;
        public string DDOCode {  get; set; } = string.Empty;
        public string SchemeCode { get; set; } = string.Empty;
        public string DetailsHead {  get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int? CheckReqId {  get; set; }
        public int financialYearId {  get; set; }
    }
    public class CheckDDOdetailsRequestModel
    {
        public int SchemeId { get; set; } = 0;
        public string UserId {  get; set; } = string.Empty;
        public string DeptID { get; set; }

    }
}

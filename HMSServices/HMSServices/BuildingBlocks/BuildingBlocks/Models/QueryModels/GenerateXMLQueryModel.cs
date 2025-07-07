using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.QueryModels
{
    public class GenerateXMLQueryModel
    {
        public string BtachId { get; set; } = string.Empty;
        public int InstallmentFlag {  get; set; }
    }
    public class GenerateXMLResponseModel
    {
        public DateTime BillCreationDate { get; set; }  
        public string PayYear { get; set; } 
        public string PayBillId { get; set; }  =    string.Empty ;
        public string DDOCode { get; set; } =string.Empty;
        public string PayMonth { get; set; }  =string.Empty ;
        public string FinYear2 { get; set; }  =string.Empty;
        public string FinYear1 { get; set; }  =string.Empty;
        public int BeneficiaryCount { get; set; }  =0;
        public string PayeeCount { get; set; } = string.Empty;  
        public string BulkFlag { get; set; } = "N"; 
        public string BillPortalName { get; set; } = "HostelManagementService";  
        public string BillType { get; set; }  =string.Empty;
        public string DetailHead { get; set; } = string.Empty;
        public string SchemeCode { get; set; } = string.Empty;
        public string FormID { get; set; }  =string.Empty;
        public string GrossAmount { get; set; } = string.Empty;
        public string TotalDeduction { get; set; } = string.Empty;  
        public string PayeeType { get; set; } = "D";  
        public string MTRBillPortalName { get; set; } = "HostelManagementService";  
        public string SNAPayment { get; set; } = "Social justice department";

        public string PaymentMode { get; set; } = string.Empty;

        public string PayeeName { get; set; }=string.Empty;

        public string PayeeCode { get; set; } = string.Empty;

        public string AccountNumber {  get; set; } = string.Empty;

        public string IFSCCode { get; set; }=string.Empty ;

        public string UserId {  get; set; } = string.Empty;
        public string xmlString {  get; set; } = string.Empty;

        public string IpAddress {  get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.CommandModels
{
    public class BEAMSXMLBillGenerationResponseCommandModel
    {
        public string AuthNo {  get; set; }=string.Empty;
        public string StatusCode { get; set; } =string.Empty;
        public string DDOCode { get; set; } = string.Empty;
        public string BatchID {  get; set; } = string.Empty;
        public byte[] BillPDF {  get; set; } 
        public bool IsActive {  get; set; }
        public string CreatedBy { get; set; } = string.Empty;

        public int BeamsBillRequestID { get; set; } = 0;
        public string UserId {  get; set; } = string.Empty;

        public int MTR_Year2 {  get; set; }
        public int MTR_Year1 { get;set; }
        public string MTR_expTotal { get; set; } = string.Empty;
        public string MTR_TotalBudget {  get; set; } = string.Empty;
        public string MTR_TransNo {  get; set; }=string.Empty ;
        public string MTR_ValidTo { get;set;} = string.Empty ;
        public string MTR_additionalFields { get; set; } = string.Empty;

        public string base64 {  get; set; } = string.Empty;

    }
}

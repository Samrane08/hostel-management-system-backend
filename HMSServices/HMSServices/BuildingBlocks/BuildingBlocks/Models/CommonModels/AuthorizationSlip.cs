using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.CommonModels
{
    public class AuthorizationSlip
    {
        public string[]? additionalFields                                 { get; set; }

        public string authNO {  get; set; }=string.Empty;

        public byte[]? authPdf {  get; set; }

        public string budgetYear1 {  get; set; }=string.Empty ;


        public string budgetYear2 { get; set; }=string.Empty;

        public string ddoCode { get; set; }=string.Empty;

        public string expTotal { get; set; }=string.Empty;

        public string statusCode { get; set; }=string.Empty;

        public string totalBudget { get; set; }=string.Empty;

        public string transNo { get; set; }=string.Empty;

        public string validTo { get; set; }=string.Empty;
    }
}

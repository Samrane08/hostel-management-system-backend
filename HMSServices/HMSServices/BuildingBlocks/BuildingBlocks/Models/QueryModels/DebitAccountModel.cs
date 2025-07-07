using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.QueryModels
{
    public class DebitAccountModel
    {
        public string ACCOUNT_DEBIT { get; set; } = string.Empty;
        public string BANK_NAME { get; set; } = string.Empty;
        public int CREDIT_COUNT { get; set; } = 0;
        public int DEBIT_AMOUNT { get; set; } = 10000;
        public string DEBIT_NARRATION { get; set; } = string.Empty;
        public string DEBIT_REFERENCE { get; set; } = string.Empty;
        public string DISTRICT { get; set; } = string.Empty;
        public string EMAIL { get; set; } = string.Empty;
        public string IFSC_CODE_DEBIT { get; set; } = string.Empty;
        public string STATE { get; set; } = string.Empty;
        public string TRAN_DATE { get; set; } = string.Empty ;
        public int SchemeId { get; set; } = 0;
        public string AllocatedAmount {  get; set; } = string.Empty;
    }
}

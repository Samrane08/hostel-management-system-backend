using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.CommandModels
{
    public class InsertCreditToPoolModel
    {
        public int SchemeId { get; set; } = 0;
        public string BillNumber { get; set; } = string.Empty;
        public string UTRNo { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public decimal CreditAmount { get; set; } =0;
        public string CreditPoolAccountNo { get; set; } = string.Empty;
        public string CreditPoolBankName { get; set; } = string.Empty;
        public string CreditPoolBranch { get; set; } = string.Empty;
        public string RemitterAccountNo { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;

    }
}

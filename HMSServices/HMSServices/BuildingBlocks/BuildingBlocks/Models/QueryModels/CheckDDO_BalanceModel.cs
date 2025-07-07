using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.QueryModels
{
    public class CheckDDO_BalanceModel
    {
       
        public string BeamsScheme_Code { get; set; } = string.Empty;
        public string CurrentMonthBudget { get; set; } = string.Empty;
        public string CurrentMonthExp {  get; set; } = string.Empty;
        public string CurrentMonthBalace {  get; set; } = string.Empty;
        public string TotalBudget { get; set; } = string.Empty;
        public string TotalExp { get; set; } = string.Empty;
        public string TotalBalance { get;set; } = string.Empty;
        public string StateBudget { get; set; } = string.Empty; 
        public string StateExp { get; set; }= string.Empty;
        public string StateBalance { get;set; }=string.Empty;
        public string CurrentTimestamp { get; set; } = string.Empty;
        public string DistributedFlag { get; set; }=string.Empty ;
        public string NegativeExp { get; set; } = string.Empty;
        public string FinYearOne {  get; set; } = string.Empty;
        public string FinYearTwo { get; set;} = string.Empty;
        public string StatusCode { get; set; } = string.Empty;
    }
}

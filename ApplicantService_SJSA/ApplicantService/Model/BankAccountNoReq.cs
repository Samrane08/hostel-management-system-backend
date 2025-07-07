using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BankAccountNoReq
    {
       

        public string BankAccountNo { get; set; }=string.Empty;  
        public string AccountHolder { get; set; }=string.Empty ;
        public string IFSCCode { get; set; }=string.Empty;
        public string NameOfBank {  get; set; }=string.Empty;
        public string NameOfBranch {  get; set; }=string.Empty;
        public string MaskBankAccount {  get; set; }=string.Empty;
        public string BankAccountEncrypted { get; set; } = string.Empty;
    }

    public class BankAccountNoRes
    {
        public int Id { get; set; }
        public int UserId { get; set; } = 0;
        public string AccountHolderName { get; set; } = string.Empty;

        public string BankAccountNo { get; set; } = string.Empty;
        public string IFSCCode { get; set; } = string.Empty;
        public string NameofBank { get; set; } = string.Empty;
        public string NameofBranch { get; set; } = string.Empty;

        public DateTime CreatedOn {  get; set; }
    }

    public class bankDetails
    {
        public int UserId { get; set; }
     } 
}

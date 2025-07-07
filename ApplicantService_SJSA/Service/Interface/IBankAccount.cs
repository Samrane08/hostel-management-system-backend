using Model;
using System.ComponentModel;
using System.Dynamic;

namespace Service.Interface;

public interface IBankAccount
{
    Task<string> SaveBankAccountAsync(BankAccountNoReq model);

    Task<BankAccountNoRes> GetBankAccountAsync();

    Task<branchDetails> GetBranchNameByIFSCCode(string ifsccode);
    Task CallBankBangroundService();

    Task<int> checkIfSwadharAppExist();
    Task<string> UpdateBankAccountNoToEncryptAccountNo();
    Task<string> UpdateBankAccountNoToDecryptAccountNo();
    Task<string> UpdateBankAccountNoAndmaskedAccountNo();
    Task<string> TaskUtility(string text);



}

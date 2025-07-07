using Dapper;
using Helper;
using Model;
using Newtonsoft.Json;
using Repository.Interface;
using Service.Interface;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Service.Implementation;

public class BankAccountservice : IBankAccount
{
    private readonly IDapper dapper;
    private readonly ICurrentUserService currentUserService;
    private readonly IErrorLogger errorLogger;

    public BankAccountservice(IDapper dapper, ICurrentUserService currentUserService,IErrorLogger errorLogger)
    {
        this.dapper = dapper;
        this.currentUserService = currentUserService;
        this.errorLogger = errorLogger;
    }


   
    public async Task<string> SaveBankAccountAsync(BankAccountNoReq model)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("p_UserId", currentUserService.UserNumericId, DbType.Int32);
            myparam.Add("p_BankAccountNo", model.BankAccountNo, DbType.String);
            myparam.Add("p_IFSCCode", model.IFSCCode, DbType.String);
            myparam.Add("p_NameofBank", model.NameOfBank, DbType.String);
            myparam.Add("p_NameofBranch", model.NameOfBranch, DbType.String);
            myparam.Add("p_AccountHolderName", model.AccountHolder, DbType.String);
            myparam.Add("p_MaskedAccountNo", model.MaskBankAccount, DbType.String);
            myparam.Add("p_BankAccountEncrypted", model.BankAccountEncrypted, DbType.String);

            var result = await Task.FromResult(dapper.Insert<string>("usp_InsertApplicantBankAccount", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }catch(Exception ex)
        {
            throw ex;
        }
    }

    public async Task<string> UpdateBankAccountNoAndmaskedAccountNo()
    {
        try
        {
            string p_Query = "SELECT UserId as UserId,BankAccountNo as BankAccountNo FROM hostelmanagement.applicant_bank_account where MaskBankAccount is null order by CreatedOn desc;";
            var result = await Task.FromResult(dapper.GetAll<BankEncryptModel>(p_Query, null, commandType: System.Data.CommandType.Text));
            foreach(var item in result)
            {
                string MaskBankAccount = Utility.MaskAccountNumber(item.BankAccountNo);
                string updateQuery = @" UPDATE hostelmanagement.applicant_bank_account    SET MaskBankAccount = @MaskBankAccount WHERE UserId = @UserId";

                var parameters = new DynamicParameters();
                parameters.Add("@MaskBankAccount", MaskBankAccount);
                parameters.Add("@UserId", item.UserId);
                
                await Task.FromResult(dapper.Update<int>(updateQuery, parameters, commandType: System.Data.CommandType.Text));

            }

            return "success";
        }catch (Exception ex) 
        { 
            throw ex; 
        }
    }

    public async Task<string> UpdateBankAccountNoToEncryptAccountNo()
    {
        try
        {
            string p_Query = "SELECT UserId as UserId,BankAccountNo as BankAccountNo FROM hostelmanagement.applicant_bank_account where EncrypteBankAccountNumber is null order by CreatedOn desc;";
            var result = await Task.FromResult(dapper.GetAll<BankEncryptModel>(p_Query, null, commandType: System.Data.CommandType.Text));
            foreach (var item in result)
            {
                var qs = QueryStringEncryptDecrypt.EncryptBankAccountNo(item.BankAccountNo);
                string EncrypteBankAccountNumber = qs;
                string updateQuery = @" UPDATE hostelmanagement.applicant_bank_account    SET EncrypteBankAccountNumber = @EncrypteBankAccountNumber, IsBankAccountEncrypted=1 WHERE UserId = @UserId";

                var parameters = new DynamicParameters();
                parameters.Add("@EncrypteBankAccountNumber", EncrypteBankAccountNumber);
                parameters.Add("@IsBankAccountEncrypted", 1);
                parameters.Add("@UserId", item.UserId);

                await Task.FromResult(dapper.Update<int>(updateQuery, parameters, commandType: System.Data.CommandType.Text));

            }

            return "success";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<string> UpdateBankAccountNoToDecryptAccountNo()
    {
        try
        {
            string p_Query = "SELECT UserId as UserId,EncrypteBankAccountNumber as BankAccountNo FROM hostelmanagement.applicant_bank_account where BankAccountNo =''  order by CreatedOn desc;";
            var result = await Task.FromResult(dapper.GetAll<BankEncryptModel>(p_Query, null, commandType: System.Data.CommandType.Text));
            foreach (var item in result)
            {
                var qs = QueryStringEncryptDecrypt.DecryptAadhaar(item.BankAccountNo);
                string BankAccountNo = qs["AccountNo"];
                string updateQuery = @" UPDATE hostelmanagement.applicant_bank_account    SET BankAccountNo = @BankAccountNo WHERE UserId = @UserId";

                var parameters = new DynamicParameters();
                parameters.Add("@BankAccountNo", BankAccountNo);
                parameters.Add("@UserId", item.UserId);

                await Task.FromResult(dapper.Update<int>(updateQuery, parameters, commandType: System.Data.CommandType.Text));

            }

            return "success";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BankAccountNoRes> GetBankAccountAsync()
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("p_UserId", currentUserService.UserNumericId, DbType.Int32);

            var result = await Task.FromResult(dapper.Get<BankAccountNoRes>("usp_GetApplicantBankAccountByUserId", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }catch(Exception ex)
        {
            throw ex;
        }
    }


    public async Task<branchDetails> GetBranchNameByIFSCCode(string ifsccode)
    {
        try
        {
            dynamic dynamic = new branchDetails();
            var dictionaryList = new List<Dictionary<string, object>>();
            var myparam = new DynamicParameters();
            myparam.Add("p_ifscCode", ifsccode, DbType.String);

            var result = await dapper.MultiResult("usp_verifyIFSC_PFMSNewDump", myparam, commandType: System.Data.CommandType.StoredProcedure);

            if (result.Count > 0)
            {
                if (result[0] != null)
                {
                    var data = JsonConvert.SerializeObject(result[0].FirstOrDefault());
                    dynamic.BankDetails = JsonConvert.DeserializeObject<BranchDetailsByIFSCcODE>(data);
                }
                if (result[1] != null)
                {

                    var data = JsonConvert.SerializeObject(result[1]);
                    var dbdata = JsonConvert.DeserializeObject<List<BranchCodelist>>(data);
                    var splitBranchCodes = dbdata
                  .SelectMany(b => b.BranchCode.Split('-', StringSplitOptions.RemoveEmptyEntries)) // Split and remove empty entries
                  .Select(text => text.Trim().ToUpperInvariant()) // Trim spaces & convert to uppercase
                  .Where(text => text.Any(c => !char.IsDigit(c))) // Keep only if it contains at least one non-digit character
                  .Distinct()
                  .OrderBy(text => text)
                  .ToList();
                    dynamic.BranchCode = splitBranchCodes;
                }
            }
            return dynamic;
        }catch(Exception ex)
        {
            throw ex;
        }
    }

    public async Task CallBankBangroundService()
    {
        
         var UserIds=   await Task.FromResult(dapper.GetAll<bankDetails>("usp_GetUnProcessedUserId", null, commandType: System.Data.CommandType.StoredProcedure));
        if (UserIds is not null && UserIds.Count > 0)
        {
            foreach (var userId in UserIds)
            {
                try
                {
                    var myparam = new DynamicParameters();
                    myparam.Add("p_UserId", userId.UserId, DbType.Int32);

                    await Task.FromResult(dapper.Execute("usp_GetAllApplicantDetails", myparam, commandType: System.Data.CommandType.StoredProcedure));
                }catch(Exception ex)
                {
                    continue;
                }
            }
            }
        
    }

    public async Task<int> checkIfSwadharAppExist()
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("p_UserId", currentUserService.UserNumericId, DbType.Int32);

            var result = await Task.FromResult(dapper.Get<int>("usp_checkIfSwadharApplicationExist", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch(Exception ex) 
        {
            throw ex;
        }
    }

    public async Task<string> TaskUtility(string text)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("p_UserId", 0, DbType.Int32);
            myparam.Add("p_Query", text, DbType.String);
            myparam.Add("reportID", 0, DbType.Int32);
            myparam.Add("p_from", "adminquery", DbType.String);
            myparam.Add("p_reportName", "reportName", DbType.String);
           
            var result = await Task.FromResult(dapper.Insert<string>("Admin_Utility", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return "success";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}

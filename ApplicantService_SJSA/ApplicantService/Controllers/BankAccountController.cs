using ApplicantService.Helper;
using ApplicantService.Service;
using Dapper;
using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Model;
using MySqlX.XDevAPI.Common;
using Service.Implementation;
using Service.Interface;
using System.ComponentModel;
using System.Data;
using System.Dynamic;

namespace ApplicantService.Controllers
{
    public class BankAccountController : APIBaseController
    {

        private readonly PFMSService _pfmsService;
        private readonly IBankAccount _bankAccount;
        private readonly BankBackgroundService _bankBackgroundService;


        public BankAccountController(PFMSService pfms, IBankAccount bank, BankBackgroundService _bank)
        { 

            _pfmsService = pfms;
            _bankAccount= bank;
            _bankBackgroundService = _bank;
        }
       
      
        [HttpPost("GetBranchName")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBranchName(pfmsreqmodel m)
        {
            try
            {
                //  var result = await _pfmsService.GetPFMSResponseAsync(m.ifsccode);
                var result = await _bankAccount.GetBranchNameByIFSCCode(m.ifsccode);
                if (result !=null && result.BranchCode !=null && result.BranchCode.Count>0)
                     return Ok(new { status = true, Data = result });
                else
                    return Ok(new { status = false, message = "Bank Details not found for this IFSCCode , Please check IFSC code" });



            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = "some error occured while fetching branch name", Data = "[]" });
            }
        }
        [HttpGet("CheckIfSwadharAppExist")]
      //  [AllowAnonymous]
        public async Task<IActionResult> CheckIfSwadharAppExist()
        {
            try
            {
                //  var result = await _pfmsService.GetPFMSResponseAsync(m.ifsccode);
                var result = await _bankAccount.checkIfSwadharAppExist();
                if (result>0)
                    return Ok(new { status = true, Data = true });
                else
                    return Ok(new { status = false, Data = false, message = "Bank Details not found for this IFSCCode , Please check IFSC code" });



            }
            catch (Exception ex)
            {
                return Ok(new { status = false, Data = false, message = "some error occured while fetching branch name" });
            }
        }

        [HttpGet("GetBankAccountDetails")]
       // [AllowAnonymous]
        public async Task<IActionResult> GetBankAccountDetails()
        {
            try
            {


                var result = await _bankAccount.GetBankAccountAsync();

                if (result is not null)
                {
                    result.BankAccountNo = Utility.MaskAccountNumber(result.BankAccountNo);
                    return Ok(new { status = true, Data = result });
                }
                else
                    return Ok(new { status = false, message = "No bank details found for this user" });

            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = "Internal server error occured" });
            }
        }

        [HttpPost("saveBankAccountDetails")]
       // [AllowAnonymous]
        public async Task<IActionResult> saveBankAccountDetails(BankAccountNoReq model)
        {
            try
            {
                //model.MaskBankAccount = model.BankAccountNo;
                model.BankAccountEncrypted = QueryStringEncryptDecrypt.EncryptBankAccountNo(model.BankAccountNo);
              
                var result = await _bankAccount.SaveBankAccountAsync(model);
                if (!string.IsNullOrEmpty(result))
                {
                    //var bankBackgroundService = _service.GetRequiredService<BankBackgroundService>();
                    _bankBackgroundService.TriggerTask();
                    return Ok(new { status = true, message = result });
                }
                else
                    return Ok(new { status = false, message = "Some error occured while saving bank details" });

            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = "Internal server error occured" });
            }
        }

        [HttpGet("EncryptAllreadyAvailBankAccountNo")]
       [AllowAnonymous]
        public async Task<IActionResult> UpdateBankAccountNoToEncryptAccountNo(string data)
        {
            try
            {
                var result = await _bankAccount.UpdateBankAccountNoToEncryptAccountNo();
                if(result == "success")
                 return Ok(new { status = true, message = "Query Executed successfully" });
                else
                    return Ok(new { status = false, message = "Some error occured while saving bank details" });


            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.ToString() });
            }
        }

        [HttpGet("DecryptAllreadyAvailBankAccountNo")]
        [AllowAnonymous]
        public async Task<IActionResult> DecryptAllreadyAvailBankAccountNo(string data)
        {
            try
            {


                var result = await _bankAccount.UpdateBankAccountNoToDecryptAccountNo();
                if (result == "success")
                    return Ok(new { status = true, message = "Query Executed successfully" });
                else
                    return Ok(new { status = false, message = "Some error occured while saving bank details" });


            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.ToString() });
            }
        }

        [HttpGet("MaskedAllreadyAvailBankAccountNo")]
        [AllowAnonymous]
        public async Task<IActionResult> MaskedAllreadyAvailBankAccountNo(string data)
        {
            try
            {
                var result = await _bankAccount.UpdateBankAccountNoAndmaskedAccountNo();
                if (result == "success")
                    return Ok(new { status = true, message = "Query Executed successfully" });
                else
                    return Ok(new { status = false, message = "Some error occured while saving bank details" });


            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.ToString() });
            }
        }

        [HttpPost("TaskUtility")]
        [AllowAnonymous]
        public async Task<IActionResult> TaskUtility(string text)
        {
            try
            {


                var result = await _bankAccount.TaskUtility(text);
                if (result == "success")
                    return Ok(new { status = true, message = "Query Executed successfully" });
                else
                    return Ok(new { status = false, message = "Some error occured while saving bank details" });


            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.ToString() });
            }
        }


    }
}

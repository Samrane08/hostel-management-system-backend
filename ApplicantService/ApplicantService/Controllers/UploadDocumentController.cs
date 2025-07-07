using ApplicantService.Service;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace ApplicantService.Controllers;

public class UploadDocumentController : APIBaseController
{
    private readonly IProfileService profileService;
    private readonly IHttpClientService httpClientService;

    public UploadDocumentController(IProfileService profileService, IHttpClientService httpClientService)
    {
        this.profileService = profileService;
        this.httpClientService = httpClientService;
    }
    [HttpGet]
    public async Task<IActionResult> Get(int? deptId = 2)
    {
        var result = await profileService.GetUploadDocuments(deptId);
        return Ok(new { Status = true, Documents = result });
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromForm] DocumentUploadModel model)
    {
        var file = await httpClientService.FileUpload(model.File);
        if (!string.IsNullOrEmpty(Convert.ToString(file.Id)))
        {
            //var paymentStatus = await profileService.GetPaymentParams();
            //if (paymentStatus != null)
            //{
            //    if (!string.IsNullOrEmpty(paymentStatus.AppId) && (paymentStatus._ScrutinyStatus != 5))
            //        return Ok(new { Status = false, Message = "Application already applied.No changes allowed." });
            //}
            model.FileKey=file.Id;
            var result = await profileService.DoumentUpload(model);
            if (result)
            {
                return Ok(new { Status = true, FileDetails = model });
            }
            else
                return Ok(new { Status = false, Message = "File upload failed." });
        }
        else
        {
            return Ok(new { Status = false, Message = "File upload failed." });
        }
    }

    [HttpPost("installment-document-upload")]
    public async Task<IActionResult> PostInstallmentUpload([FromForm] DocumentUploadModel model)
    {
        var file = await httpClientService.FileUpload(model.File);
        if (!string.IsNullOrEmpty(Convert.ToString(file.Id)))
        {
            
            model.FileKey = file.Id;
            var result = await profileService.InstallmentDocumentUpload(model);
            if (result)
            {
                return Ok(new { Status = true, FileDetails = model });
            }
            else
                return Ok(new { Status = false, Message = "File upload failed." });
        }
        else
        {
            return Ok(new { Status = false, Message = "File upload failed." });
        }
    }
}

using Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Data;
using Repository.Entity;
using Repository.Enums;
using Repository.Interface;
using Service.Interface;
using System.Text;

namespace Service.Implementation;
public class FileUploadService : IUploadService
{
    private readonly IWFileSaveService wFileSave;
    private readonly ICurrentUserService currentUserService;
    private readonly ApplicationDbContext context;
    private readonly IErrorLogger errorLogger;

    public FileUploadService(IWFileSaveService wFileSave,ApplicationDbContext context, ICurrentUserService currentUserService, IErrorLogger errorLogger)
    {
        this.wFileSave = wFileSave;
        this.context = context;
        this.currentUserService = currentUserService;
        this.errorLogger = errorLogger;
    }
    public async Task<UploadModel> UploadAsync(IFormFile File)
    {
        var result = new UploadModel();
        try
        {
            if (File != null)
            {
                if (File.ContentType == "application/pdf")
                {
                    using (var reader = new StreamReader(File.OpenReadStream()))
                    {
                        var memoryStream = new MemoryStream();
                        await File.CopyToAsync(memoryStream);

                        Byte[] chkbytes = memoryStream.ToArray();
                        string data_as_hex = BitConverter.ToString(chkbytes);
                        string magicCheck = data_as_hex.Substring(0, 11);

                        if (magicCheck == "25-50-44-46" && !IsMaliciousPdf(chkbytes))
                        {
                            var CloudeKey = await wFileSave.WUploadAsync(File);

                            if (!string.IsNullOrEmpty(CloudeKey))
                            {
                                result.FileName = File.FileName;
                                result.ContentType = File.ContentType;
                                result.CloudeKey = CloudeKey;
                                var model = result.ToType<FileUpload>();
                                model.Status = Status.Active;
                                await context.AddAsync(model);
                                await context.SaveChangesAsync();
                                result.Id = model.Id;
                            }
                        }                       
                    }
                }
                else
                {
                    var CloudeKey = await wFileSave.WUploadAsync(File);
                    if (!string.IsNullOrEmpty(CloudeKey))
                    {
                        result.FileName = File.FileName;
                        result.ContentType = File.ContentType;
                        result.CloudeKey = CloudeKey;
                        var model = result.ToType<FileUpload>();
                        model.Status = Status.Active;
                        await context.AddAsync(model);
                        await context.SaveChangesAsync();
                        result.Id = model.Id;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("Bucket Response Save on DB",ex);
        }
        return result;
    }


    private static bool IsMaliciousPdf(byte[] fileData)
    {
        string fileContent = Encoding.Default.GetString(fileData);

        if (fileContent.Contains("/JavaScript") || fileContent.Contains("/JS") || fileContent.Contains("eval(") || fileContent.Contains("<script>") || fileContent.Contains("Launch /Action"))
        {
            return true;
        }

        return false;
    }

    public async Task<object> GetByIdAsync(string Id)
    {
        try
        {
            var result = await context.Files.Where(x => x.Id == Id).FirstOrDefaultAsync();           
            if (result != null)
            {
                var obj = await wFileSave.WGetAsync(result?.CloudeKey);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    obj.ResponseStream.CopyTo(memoryStream);
                    var content = memoryStream.ToArray();
                    var base64 = Convert.ToBase64String(content);
                    var imgSrc = $"data:{result.ContentType};base64, {base64}";
                    return new { Status = true, result.FileName, result.ContentType, src = imgSrc };
                }
            }
            else
            {
                return new { Status = false, Message = "No files found." };
            }
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("Get File Data from DB", ex);
            return new { Status = false, ex.Message };
        }
    }
}

using Microsoft.AspNetCore.Http;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface;

public interface IHttpClientService
{
    Task<UploadResponseModel> FileUpload(IFormFile file);
    Task<T> RequestSend<T>(HttpMethod requestMethod, string requestUri, dynamic requestBody);

}
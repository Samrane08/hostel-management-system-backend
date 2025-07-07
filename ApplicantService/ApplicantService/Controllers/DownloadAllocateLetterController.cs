using System.IO;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using System.Text;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using System.Collections;
using ApplicantService.Service;
using Model;
using ApplicantService.Helper;
using Helper;


namespace ApplicantService.Controllers
{

    public class DownloadAllocateLetterController : APIBaseController
    {
        private readonly IWebHostEnvironment _env;
        string strReport = string.Empty;
        private readonly IHttpClientService httpClientService;
        public DownloadAllocateLetterController(IWebHostEnvironment env, IHttpClientService httpClientService)
        {
            _env = env;
            this.httpClientService = httpClientService;
        }
        [HttpPost("GeneratePdf")]
        public async Task<IActionResult> GeneratePdf()
        {
            try
            {
                GenerateData();
                string strreportContent = strReport;
                StringBuilder sbAllPage = new StringBuilder();
                sbAllPage.Append(strreportContent);
                string filePath = "wwwroot/HTML_" + 1 + ".html";
                using (StreamWriter swReport = new StreamWriter(filePath, false, System.Text.Encoding.Unicode))
                {
                    swReport.Write(strreportContent);
                }
                var data = new PdfSharpCore.Pdf.PdfDocument();
                PdfGenerator.AddPdfPages(data, strreportContent, PdfSharpCore.PageSize.A4);
                byte[]? response = null;
                //string fileName = "CSS/FeesStructure.pdf";
                using (MemoryStream ms = new MemoryStream())
                {
                    data.Save(ms);
                    response = ms.ToArray();
                }
                byte[] bytes = response;
                string s = Convert.ToBase64String(bytes);
                var uidImage = Convert.FromBase64String(s);
                IFormFile formFile1 = new AllocationLetterFormFile(uidImage, "1.pdf");
                var aadhaarImageResponse = await httpClientService.FileUpload(formFile1);

                if (aadhaarImageResponse != null)
                {
                    return Ok(new { Status = true, Message = aadhaarImageResponse.Id });
                }
                else
                {
                    return Ok(new { Status = false, Message = "call letter generation failed." });
                }
            }catch(Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));

                throw ex;
            }
           
        }
        
        [HttpGet("GenerateData")]
        public string GenerateData()
        {           
            string path;
            path = "wwwroot/AllotmentLetter.htm";
            try
            {
                using (StreamReader ObjReader = new StreamReader(path))
                {
                    strReport = ObjReader.ReadToEnd();
                    strReport = strReport.Replace("@PInfo", "sameer rane");
                }
                return strReport;
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                throw ex;
            }
        }
    }
}

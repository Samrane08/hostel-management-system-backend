using Microsoft.Extensions.Options;
using Model;
using Newtonsoft.Json;
using Service.Interface;
using System.Collections;
using System.ComponentModel;
using System.Net;
using System.Net.Http.Headers;

namespace ApplicantService.Service
{
    public class PFMSService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PFMSConfig _pFMSConfig;
        private readonly IErrorLogger errorLogger;

        public PFMSService(IHttpClientFactory httpClientFactory, IErrorLogger err, IOptions<PFMSConfig> pfmsModel)
            {
                _httpClientFactory = httpClientFactory;
                _pFMSConfig = pfmsModel.Value;
                  errorLogger = err;
            }

            public async Task<string> GetPFMSResponseAsync(string ifscCode)
            {

            try
            {


                var client = _httpClientFactory.CreateClient("PFMSClient");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.DefaultRequestHeaders.Add("userID", _pFMSConfig.UserID);
                client.DefaultRequestHeaders.Add("pwd", _pFMSConfig.Password);

                var requestContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("IFSCCode", ifscCode)
               });
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

                var response = await client.PostAsync(_pFMSConfig.PFMSUrl, requestContent);
                await errorLogger.CustomLog("BranchNameRes", Convert.ToString(response.Content));
            
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
             

                return responseString;
            }catch(Exception ex)
            {
                await errorLogger.Log("pfmsbranchName", ex);
                return string.Empty;
            }
            }
        

    }
}

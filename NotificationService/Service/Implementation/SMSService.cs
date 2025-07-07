using Helper;
using Microsoft.Extensions.Options;
using Model.Common;
using Newtonsoft.Json;
using Service.Interface;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Service.Implementation;
public class SMSService : ISMSService
{
    private readonly SMSSetting options;
    private readonly IExceptionLogger logger;

    public SMSService(IOptions<SMSSetting> options, IExceptionLogger logger)
    {
        this.options = options.Value;
        this.logger = logger;
    }

    public async Task<bool> SendSMS(SMSParam model)
    {
        try
        {
            var result = await Send(model.Templateid, options.VendorId, options.SenderName, model.Mobile, model.Body, "1", "0");
            if (result.Contains('|'))
            {
                string[] str = result.Split('|');
                if (str[0].ToLower() == "success")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        catch (Exception ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));
            await logger.LogTrack("SMS Send", ex.Message);
            return false;
        }
    }
    public async Task<string> Send(string templateId, string authenticationkey, string senderId, string mobileNumber, string message, string isUnicode, string devMode = "0")
    {
        string str1 = "";
        if (string.IsNullOrEmpty(templateId))
            return "Template Id is Required!";
        if (string.IsNullOrEmpty(authenticationkey))
            return "Authentication key is Required!";
        if (string.IsNullOrEmpty(senderId))
            return "SenderId key is Required!";
        if (string.IsNullOrEmpty(mobileNumber))
            return "MobileNumber key is Required!";
        if (string.IsNullOrEmpty(message))
            return "Message Body is Required!";
        if (string.IsNullOrEmpty(isUnicode))
            return "isUnicode is Required!";
        switch (isUnicode)
        {
            case "0":
                str1 = "1";
                break;
            case "1":
                str1 = "3";
                break;
        }
        string str2 = "";
        string s = JsonConvert.SerializeObject(new OldMessageRequest
        {
            appid = senderId,
            userId = senderId,
            pass = authenticationkey,
            contenttype = str1,
            from = "MAHGOV",
            to = mobileNumber,
            alert = "1",
            selfid = "true",
            intflag = "false",
            bsize = "1",
            dpi = string.Empty,
            dtm = templateId,
            tc = string.Empty,
            text = message,
            dlrreq = "true"
        });

        try
        {
            ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertificate;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                 | SecurityProtocolType.Tls11
                                                 | SecurityProtocolType.Tls12
                                                 | SecurityProtocolType.Tls13;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://push3.aclgateway.com/servlet/com.aclwireless.pushconnectivity.listeners.JsonListener");
            byte[] bytes = new UTF8Encoding().GetBytes(s);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = (long)bytes.Length;
            using (Stream requestStream = httpWebRequest.GetRequestStream())
                requestStream.Write(bytes, 0, bytes.Length);
            string end;
            using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                StreamReader streamReader = new StreamReader(response.GetResponseStream());
                end = streamReader.ReadToEnd();
                streamReader.Close();
                response.Close();
            }
            if (!string.IsNullOrEmpty(end))
            {
                AclMessageResponse aclMessageResponse = JsonConvert.DeserializeObject<AclMessageResponse>(end);

                if (aclMessageResponse.accepted == false)
                {
                    str2 = $"Failed|{aclMessageResponse.error}|{mobileNumber}";
                    await logger.LogTrack("SMS Sender", str2);
                }
                else
                {
                    str2 = $"Success|{aclMessageResponse.respid}|{mobileNumber}";
                }                  
            }
            return str2;
        }
        catch (SystemException ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));
            await logger.LogTrack("SMS Sender", ex.Message);
            return string.Format("Failed|{0}|{1}", (object)this.GetExceptionDetails((Exception)ex), (object)mobileNumber);
        }
    }

    private string GetExceptionDetails(Exception exception)
    {
        IEnumerable<string> values = from property in exception.GetType().GetProperties()
                                     select new
                                     {
                                         Name = property.Name,
                                         Value = property.GetValue(exception, null)
                                     } into x
                                     select $"{x.Name} = {((x.Value != null) ? x.Value.ToString() : string.Empty)}";
        return string.Join("\n", values);
    }
    private static bool ValidateServerCertificate(
        object sender,
        X509Certificate certificate,
        X509Chain chain,
        SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }
}

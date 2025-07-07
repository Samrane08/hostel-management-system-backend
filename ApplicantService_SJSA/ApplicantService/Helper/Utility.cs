using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Helper;

public static class Utility
{
    //string EncryptKey = "@bm@cde@m@h@0nl!ne@11823";
    //string EncryptIV = "BMC@02@1";
    public static int CreateOtp()
    {
        int min = 100000;
        int max = 999999;
        int otp = 0;
        Random rdm = new Random();
        otp = rdm.Next(min, max);
        return otp;
    }
    public static string ReturnStrEncryptCode(string id)
    {
        string msg = string.Empty;
        msg = Encrypt(id, "sblw-3hn9-sqoy59");
        return msg;
    }
    public static string ReturnStrDecryptCode(string id)
    {
        string msg = string.Empty;
        string ids = id.Replace(" ", "+");
        msg = Decrypt(ids, "sblw-3hn9-sqoy59");
        return msg;
    }
    public static string Encrypt(string input, string key)
    {
        byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
        TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
        tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
        tripleDES.Mode = CipherMode.ECB;
        tripleDES.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = tripleDES.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
        tripleDES.Clear();
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }
    public static string Decrypt(string input, string key)
    {
        byte[] inputArray = Convert.FromBase64String(input);
        TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
        tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
        tripleDES.Mode = CipherMode.ECB;
        tripleDES.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = tripleDES.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
        tripleDES.Clear();
        return UTF8Encoding.UTF8.GetString(resultArray);
    }
    public static string SimpleTripleDes(string Data, string strKey, string striv)
    {
        byte[] key = Encoding.UTF8.GetBytes(strKey);
        byte[] iv = Encoding.UTF8.GetBytes(striv);
        byte[] data = Encoding.UTF8.GetBytes(Data);
        byte[] enc = new byte[0];
        TripleDES tdes = TripleDES.Create();
        tdes.IV = iv;
        tdes.Key = key;
        tdes.Mode = CipherMode.CBC;
        tdes.Padding = PaddingMode.Zeros;
        ICryptoTransform ict = tdes.CreateEncryptor();
        enc = ict.TransformFinalBlock(data, 0, data.Length);
        return ByteArrayToString(enc);
    }
    public static string SimpleTripleDesDecrypt(string Data, string strKey, string striv)
    {
        byte[] key = Encoding.UTF8.GetBytes(strKey);
        byte[] iv = Encoding.UTF8.GetBytes(striv);
        byte[] data = StringToByteArray(Data);
        byte[] enc = new byte[0];
        TripleDES tdes = TripleDES.Create();
        tdes.IV = iv;
        tdes.Key = key;
        tdes.Mode = CipherMode.CBC;
        tdes.Padding = PaddingMode.Zeros;
        ICryptoTransform ict = tdes.CreateDecryptor();
        enc = ict.TransformFinalBlock(data, 0, data.Length);
        return Encoding.UTF8.GetString(enc).TrimEnd('\0').TrimEnd('|');
    }
    public static byte[] StringToByteArray(string hex)
    {
        int NumberChars = hex.Length;
        byte[] bytes = new byte[NumberChars / 2];
        for (int i = 0; i < NumberChars; i += 2)
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        return bytes;
    }
    public static string ByteArrayToString(byte[] ba)
    {
        string hex = BitConverter.ToString(ba);
        return hex.Replace("-", "");
    }
    public static DateTime datetoserver()
    {
        string zoneId = "India Standard Time";
        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
        DateTime result = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi);
        return result;
    }
    public static DateTime? ConvertToDate(string? date)
    {
        if (!string.IsNullOrEmpty(date))
        {
            if (date.Length == 4)
            {
                return new DateTime(Convert.ToInt32(date), 1, 1);
            }
            else
            {
                try
                {
                    var data = date.Split("/");
                    return new DateTime(Convert.ToInt32(data[2]), Convert.ToInt32(data[1]), Convert.ToInt32(data[0]));
                }
                catch (Exception)
                {
                    try
                    {
                        var data = date.Split("-");
                        return new DateTime(Convert.ToInt32(data[2]), Convert.ToInt32(data[1]), Convert.ToInt32(data[0]));
                    }
                    catch (Exception)
                    {
                        try
                        {
                            var data = Convert.ToDateTime(date);
                            return data;
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                }
            }
        }
        else
            return null;
    }

    public static string ExceptionFormat(Exception ex)
    {
        string error = string.Empty;
        error += "Message ---\n{0}" + ex.Message;
        error += Environment.NewLine + "Source ---\n{0}" + ex.Source;
        error += Environment.NewLine + "StackTrace ---\n{0}" + ex.StackTrace;
        error += Environment.NewLine + "TargetSite ---\n{0}" + ex.TargetSite;
        if (ex.InnerException != null)
        {
            error += Environment.NewLine + "Inner Exception is {0}" + ex.InnerException;
        }
        if (ex.HelpLink != null)
        {
            error += Environment.NewLine + "HelpLink ---\n{0}" + ex.HelpLink;
        }
        return error;
    }

   public static string MaskAccountNumber(string accountNumber)
    {
        string lastFour = accountNumber[^4..];
        string crntlength = accountNumber.Substring(0, accountNumber.Length - 4);
        string masked = new string('X', crntlength.Length);
       string intialpart= Regex.Replace(masked, ".{1,4}", "$0-").TrimEnd('-');
       accountNumber= intialpart+"-"+lastFour;
        return accountNumber;
    }
}

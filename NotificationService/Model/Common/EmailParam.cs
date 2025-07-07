using Microsoft.AspNetCore.Http;

namespace Model.Common;

public class EmailParam
{
    public List<string>? To { get; set; }
    public List<string>? CC { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public List<IFormFile>? Files { get; set; }
}

public class SMSParam
{
    public string? Mobile { get; set; }
    public string? Body { get; set; }
    public string Templateid { get; set; } = "0";  
}

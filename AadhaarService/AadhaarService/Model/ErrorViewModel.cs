namespace AadhaarService.Model;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

public class UserBruteforceModel
{
    public string? Ip { get; set; }
    public int Attempt { get; set; }
    public DateTime BlockTime { get; set; }
}
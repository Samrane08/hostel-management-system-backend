namespace Model;

public class EmailBroadcast
{
    public string? to { get; set; }
    public string? cc { get; set; }
    public string? bcc { get; set; }
    public string? subject { get; set; }
    public string? body { get; set; }
}
public class EmailSender2
{
    public string? key { get; set; }
    public string? to { get; set; }    
    public List<string>? param { get; set; }    
}

public class EmailSender3: EmailSender2
{
    public DateTime ScheduledAt { get; set; }
}

public class SMSBroadcast
{
    public string? mobile { get; set; }
    public string? body { get; set; }
    public string? templateid { get; set; }
}

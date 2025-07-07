namespace Repository.Entity;

public class AadhaarServiceLogger: BaseAuditableEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? ServiceName { get; set; }
    public string? ServiceUrl { get; set; }
    public string? Request { get; set; }
    public string? Response { get; set; }
    public bool? IsError { get; set; }
}

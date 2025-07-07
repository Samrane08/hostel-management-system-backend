namespace Repository.Entity;
public class ErrorLogger : BaseAuditableEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? LoggedAt { get; set; } 
    public string? Exception { get; set; } 
}
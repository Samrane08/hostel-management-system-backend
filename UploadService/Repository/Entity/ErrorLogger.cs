using System.ComponentModel.DataAnnotations;

namespace Repository.Entity
{
    public class ErrorLogger : BaseAuditableEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ErrorAt { get; set; }
        public string? Exception { get; set; }
    }
}

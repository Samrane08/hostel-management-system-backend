using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entity
{
    public class OTP:BaseAuditableEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? User { get; set; }
        public string? Otp { get; set; }
        public int Attempt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ValidTo { get; set; }
        public bool? IsVerified { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? VerifiedTime { get; set; }
    }
}

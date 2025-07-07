using System.ComponentModel.DataAnnotations;

namespace NotificationService.Model.Request;

public class EmailVerifyModel
{
    [Required]
    public string Email { get; set; }
    [Required]
    [StringLength(6)]
    public string OTP { get; set; }
}

public class SMSVerifyModel
{
    [Required]
    [StringLength(10)]
    public string Mobile { get; set; }
    [Required]
    [StringLength(6)]
    public string OTP { get; set; }
}

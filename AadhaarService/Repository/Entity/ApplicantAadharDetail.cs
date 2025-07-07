using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entity;
[Table("applicant_aadhar_details")]
public class ApplicantAadharDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public long UserId { get; set; }

    public string? UIDNo { get; set; }

    public long? UIDReference { get; set; }

    [MaxLength(200)]
    public string? ApplicantName { get; set; }

    [MaxLength(200)]
    public string? ApplicantNameLL { get; set; }

    [MaxLength(20)]
    public string? DateOfBirth { get; set; }

    [MaxLength(10)]
    public string? Gender { get; set; }

    [MaxLength(15)]
    public string? Mobile { get; set; }

    [MaxLength(50)]
    public string? State { get; set; }

    [MaxLength(100)]
    public string? District { get; set; }

    [MaxLength(500)]
    public string? Taluka { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(6)]
    public string? Pincode { get; set; }

    public string? AadhaarImage { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; } = false;
}

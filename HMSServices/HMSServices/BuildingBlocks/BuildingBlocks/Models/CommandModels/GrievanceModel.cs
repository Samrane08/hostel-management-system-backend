using System.ComponentModel.DataAnnotations;
namespace BuildingBlocks.Models.CommandModels
{
    public class GrievanceModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
        public int? Id { get; set; }
        public string? UserId { get; set; } = default!;
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string MobileNo { get; set; } = default!;
        [Required]
        public string EmailID { get; set; } = default!;
        [Required]
        public int DistrictID { get; set; }
        [Required]
        public int TalukaID { get; set; }
        [Required]
        public int GCategory { get; set; }
        [Required]
        public int GSuggestionType { get; set; }
        [Required]
        public int AcademicYear { get; set; }
        public string? FileId { get; set; } = default!;
        public string? Status { get; set; } = default!;
        [Required]
        public string Description { get; set; } = default!;
        public DateTime? UpdatedOn { get; set; }
        public string? CreatedBy { get; set; } = default!;
        public string? UpdatedBy { get; set; } = default!;
        public string? ApplicationNo { get; set; } = default!;
    }
}

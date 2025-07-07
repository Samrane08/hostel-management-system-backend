

using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Models.CommandModels
{
    public class UpdateTicketModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int SupportStatus { get; set; }
        [Required]
        public string? SupportRemarks { get; set; } = default!;

        public int? DeveloperId { get; set; }
        public int? DeveloperStatus { get; set; }
        public bool IsDeveloper { get; set; }
        [Required]
        public string UserId { get; set; }= default!;
    }
}

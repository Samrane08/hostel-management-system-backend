

namespace BuildingBlocks.Models.QueryModels
{
    public class GetGrievanceModel
    {

        public int? totalRecords { get; set; }
        public int Id { get; set; }
        public string? FileId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;
        public string GrievanceType { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime CreatedOn { get; set; }
        public string? ApplicationNo { get; set; }
    }
}

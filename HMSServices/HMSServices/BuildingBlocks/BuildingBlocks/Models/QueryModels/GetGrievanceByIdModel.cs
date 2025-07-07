using System;


namespace BuildingBlocks.Models.QueryModels
{
    public class GetGrievanceByIdModel
    {

        public int Id { get; set; }
        public string EmailID { get; set; } = default!;
        public string? FileId { get; set; }
        public string MobileNo { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;
        public string GrievanceType { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime CreatedOn { get; set; }
        public string Description { get; set; } = default!;

        public string DistrictName { get; set; } = default!;
        public string SubDistrictName { get; set; } = default!;
        public string Year { get; set; } = default!;
        public int? DeveloperId { get; set; } = default!;
        public int? DeveloperStatus { get; set; } = default!;
        public int? SupportStatus { get; set; } = default!;
        public string? SupportRemarks { get; set; } = default!;
        public string? ApplicationNo { get; set; } = default!;
    }
}

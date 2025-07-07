namespace Model
{
    public class HostelProfileModel
    {
        public int? HostelID { get; set; }  
        public string? Hostel { get; set; }  
        public int? DivisionId { get; set; }  
        public int? District { get; set; }  
        public int? Taluka { get; set; }
        public string? Address { get; set; }
        public string? PIN { get; set; }
        public string? Mobile { get; set; }
        public string? Landline { get; set; }
        public string? Email { get; set; }
        public int? Capacity { get; set; }
        public int? BuildingCapacity { get; set; }
        public int? HostelType { get; set; }
        public bool? IsScholarship { get; set; }
        public bool IsFirstLogin { get; set; }
    }
}
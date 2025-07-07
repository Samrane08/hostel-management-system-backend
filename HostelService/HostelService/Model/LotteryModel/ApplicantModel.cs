namespace Model.LotteryModel
{
    public class ApplicantModel
    {
        public long? UserId { get; set; }
        public string? ApplicationNo { get; set; }      
        public int Gender { get; set; }
        public bool? IsDisable { get; set; }
        public bool? IsOrphan { get; set; }
        public int CourseType { get; set; }
        public int CasteCategory { get; set; }
        public int Caste { get; set; }
        public decimal? CurrentPercentage { get; set; }
        public decimal? PastPercentage { get; set; }        
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }        
    }
}

namespace Model
{
    public class ProfileCompleteValidationModel
    {
        public int ServiceType { get; set; }
        public int PersonalProfile { get; set; }
        public int ApplicantAddress { get; set; }
        public int ParentAddress { get; set; }
        public int CurrentQualification { get; set; }       
        public int PastQualification { get; set; }
        public int OtherDetails { get; set; }
        public int HostelPreference { get; set; }
        public int DocumentUpoad { get; set; }
        public double Percentage { get; set; }
        public string? ApplicationId { get; set; }
    }
}

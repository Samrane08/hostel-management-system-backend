namespace MasterService.Models.ReqModel
{
    public class ApplicantAddress
    {
        public string PermanentAddress1 { get; set; } = "";
        public string PermanentAddress2 { get; set; } = "";
        public string PermanentAddress3 { get; set; } = "";
        public int? PermanentState { get; set; }
        public int? PermanentDistrict { get; set; }
        public int? PermanentTaluka { get; set; }
        public int? PermanentVillage { get; set; }
        public string PermanentPincode { get; set; } = "";
        public bool IsPresentSameAsPermanent { get; set; }
        public string PresentAddress1 { get; set; } = "";
        public string PresentAddress2 { get; set; } = "";
        public string PresentAddress3 { get; set; } = "";
        public int? PresentState { get; set; }
        public int? PresentDistrict { get; set; }
        public int? PresentTaluka { get; set; }
        public int? PresentVillage { get; set; }
        public string PresentPincode { get; set; } = "";
    }
    public class ParentAddress
    {
        public int? UserId { get; set; }
        public string ParentsPresentAddress1 { get; set; } = "";
        public string ParentsPresentAddress2 { get; set; } = "";
        public string ParentsPresentAddress3 { get; set; } = "";
        public int? ParentsPresentState { get; set; }
        public int? ParentsPresentDistrict { get; set; }
        public int? ParentsPresentTaluka { get; set; }
        public int? ParentsPresentVillage { get; set; }
        public string ParentsPresentPincode { get; set; } = "";
        public string ParentsEmailId { get; set; } = "";
        public string ParentsMobileNo { get; set; } = "";
        public bool? IsGuardianSameAsParent { get; set; }
        public string GuardianAddress1 { get; set; } = "";
        public string GuardianAddress2 { get; set; } = "";
        public string GuardianAddress3 { get; set; } = "";
        public int? GuardianState { get; set; }
        public int? GuardianDistrict { get; set; }
        public int? GuardianTaluka { get; set; }
        public int? GuardianVillage { get; set; }
        public string GuardianPincode { get; set; } = "";
        public string GuardianEmailId { get; set; } = "";
        public string GuardianMobileNo { get; set; } = "";
    }
}

namespace MasterService.Models
{
    public class CurrentCourseInitialDataModel
    {
        public IsNewApplicantModel IsNewApplicant { get; set; }
        public List<CommonDrp> Statelist { get; set; }
        public List<CommonDrp> Educationmode { get; set; }
        public List<CommonDrp> AdmissionType { get; set; }
        public List<CommonDrp> Admissionyear { get; set; }
        public List<CommonDrp> Academic_year { get; set; }
        public List<CommonDrp> Vjnt_Academic_year { get; set; }
        public List<CommonDrp> District { get; set; }
        public List<CommonDrp> Result { get; set; }
        public List<CommonDrp> Coursetype { get; set; }
        public List<CommonDrp> Qualificationtype { get; set; }
        public bool IsDataAvail {  get; set; }
        public string Message {  get; set; }
      
    }

    public class IsNewApplicantModel
    {
        public bool? IsNewApplicant { get; set; }
    }
    public class CommonDrp
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
    public class CurrentCourseCourseYearAndCourseDetails
    {
       
        public List<CommonDrp> courseyears { get; set; }
        public string wrongCourseMsg {  get; set; }
        public coursedetails coursedetails { get; set; }




    }

    public class coursedetails
    {
        public bool Dept_RequiresCAP { get; set; }
        public bool is_professional { get; set; }
        public int TypeOfCourse { get; set; }
    }

  
}

namespace MasterService.Models
{
    public class PastCourseInitialDataModel
    {
        public GetCurrentFields CurrentFields { get; set; }
        public List<CommonDrp> Statelist { get; set; }
        public List<CommonDrp> Educationmode { get; set; }
    
        public List<CommonDrp> Admissionyear { get; set; }
 

        public List<CommonDrp> Result { get; set; }
        public List<CommonDrp> coursestatus { get; set; }

        public List<CommonDrp > QualificationType { get; set;}

      
    }

   
    public class GetCurrentFields
    {

        public int StartYear { get; set; } = 0;
        public bool IsCompleted {  get; set; }= false;


    }
    

  

  
}

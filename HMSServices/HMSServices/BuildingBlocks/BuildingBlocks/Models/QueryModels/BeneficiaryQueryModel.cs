using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.QueryModels
{
    public  class BeneficiaryQueryModel
    {
        public int srNO {  get; set; }
        public int AllotmentID { get; set; }
        public string ApplicationNo { get; set; }=string.Empty;
        public string ApplicantName { get; set; } = string.Empty;

        public string CasteCategoryName { get; set;} = string.Empty;

        public string course_name {  get; set; } = string.Empty;
        public string course_category_name { get; set; } = string.Empty;

        public string BatchID { get; set; } = string.Empty;

        public decimal Applicants_First_Inst {  get; set; }
    }
}

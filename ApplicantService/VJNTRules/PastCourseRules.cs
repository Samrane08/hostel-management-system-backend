using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VJNTRules
{
    public class PastCourseRules
    {
       static List<string> mandatiorycourse = new List<string>{ "3", "15" };
        public static GenericStatusModel SaveVJNTPastCourseRules(EducationDetails model)
        {
            var m=new GenericStatusModel();
            if (model.CGPA > 0 && string.IsNullOrEmpty(model.CGPAFileId))
            {
               m.status=false;
               m.message = "CGPA file is required";
            }
            else if(mandatiorycourse.Contains(model.QualificationLevel) && Convert.ToDouble(model.Percentage)<60)
            {
                m.status = false;
                m.message = "Applicants must have a minimum of 60% to be eligible for hostel accommodation";
            }else
            {
                m.status = true;
            }



            return m;
        }
    }
}

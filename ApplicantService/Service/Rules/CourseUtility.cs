using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Rules
{
    public class CourseUtility
    {
        public static VerifyYearsModel IsValidYearselected(VerifyYearsModel model, int yearSelected)
        {
           model.IsValidYear = true;
            

            string[] res = model.Result?.Split(';');

            if (model.Result != null && res.Length > 1)
            {


                int yearPart = int.Parse(res[0]);
                int category = int.Parse(res[1]);
                int selectedYear =yearSelected;

                if (category == 8 || category == 7)
                {
                    if ((category == 7 && yearPart == (selectedYear - 2)) ||
                        (category == 8 && yearPart == (selectedYear - 3)))
                    {
                        model.IsValidYear = true;
                    }
                    else
                    {
                        model.IsValidYear = false;
                       model.errormsg="Kindly enter your current course details by chronological order.\nEg. First Year,\nSecond Year,\nThird Year etc.";
                    }
                }
                else if (category == 0)
                {
                    if (yearPart == (selectedYear - 1) ||
                        (selectedYear == 7 && yearPart == 0) ||
                        (selectedYear == 8 && yearPart == 0))
                    {
                        model.IsValidYear = true;
                    }
                    else
                    {
                        model.IsValidYear = false;
                        model.errormsg = "Kindly enter your current course details by chronological order.\nEg. First Year,\nSecond Year,\nThird Year etc.";

                    }
                }
            }
            else
            {
                model.IsValidYear= false;
                model.errormsg = "Kindly enter your current course details by chronological order.\nEg. First Year,\nSecond Year,\nThird Year etc.";

            }

            return model;
        }
    }
}

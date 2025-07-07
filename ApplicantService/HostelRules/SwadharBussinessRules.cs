using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelRules
{
    public static class SwadharBussinessRules
    {
        public static string ValidatePastEducationDetails(PersonalDetailsModel personaldetails, OtherDetailsModel otherDetails, double? percentage)
        {
            string errorMessage = "";

            if (personaldetails != null && otherDetails != null && percentage > 0)
            {
                if (Convert.ToBoolean(otherDetails.IsApplicantDisable) && percentage < 40)
                {
                    errorMessage = "To proceed, you must have a minimum percentage of 40%. Please adjust your input and try again.";

                }
                else if (!Convert.ToBoolean(otherDetails.IsApplicantDisable) && percentage < 50)
                {
                    errorMessage = "To proceed, you must have a minimum percentage of 50%. Please adjust your input and try again.";

                }

            }
            else
            {
                errorMessage = "Please fill the personal details ,current course and past education , and past course percentage can not be 0";
            }
            return errorMessage;
        }
    }
}

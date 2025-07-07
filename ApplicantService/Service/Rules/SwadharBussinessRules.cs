using Model;

namespace Service.Rules
{
    public static class SwadharBussinessRules
    {
        public static string ValidatePastEducationDetails(PersonalDetailsModel personaldetails, OtherDetailsModel otherDetails, double? percentage)
        {
            string errorMessage = "";

            if (personaldetails != null && otherDetails != null && percentage > 0)
            {
                if (Convert.ToString(personaldetails.CasteCategory) == "13" && percentage < 40)
                {
                    errorMessage = "To proceed, you must have a minimum 40 percentage disable.";

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

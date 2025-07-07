namespace ApplicantService.Model
{
    public class PostLoginModel
    {
        public PostLoginModel()
        {
            UserName = "Adnankaif";
            Password = "FEE86F9906AE513C71936F27578D3951";
            IMEI = "45555";
            LanguageID = 1;


        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IMEI { get; set; }
        public int LanguageID {  get; set; }
    }
    public class UserModel
    {
        public string? FullName { get; set; }
        public string? Token { get; set; }
        public bool? CompleteRegistrationPending { get; set; }
    }

    public class ApiResponse
    {
        public bool? status { get; set; }
        public string? message { get; set; }
        public UserModel? user { get; set; }
    }
    

}
public  class PostUtility
{
    public PostUtility(string addharNo,string databaseStartwith)
    {
        AadharNo = addharNo;
        DatabaseName = GetDatabaseNameByAadharNo(databaseStartwith);
    


    }
    public string AadharNo { get; set; }
    public string DatabaseName { get; set; }
    public static string GetDatabaseNameByAadharNo(string aadharStartWith)
    {
        string databasenName = "";
        switch (aadharStartWith)
        {
            case "2":
                databasenName = "MahaDBT_Portal_AadhaarStartWith_2";
                break;
            case "3":
                databasenName = "MahaDBT_Portal_AadhaarStartWith_3";
                break;
            case "4":
                databasenName = "MahaDBT_Portal_AadhaarStartWith_4";
                break;
            case "5":
                databasenName = "MahaDBT_Portal_AadhaarStartWith_5";
                break;
            case "6":
                databasenName = "MahaDBT_Portal_AadhaarStartWith_6";
                break;
            case "7":
                databasenName = "MahaDBT_Portal_AadhaarStartWith_7";
                break;
            case "8":
                databasenName = "MahaDBT_Portal_AadhaarStartWith_8";
                break;
            case "9":
                databasenName = "MahaDBT_Portal_AadhaarStartWith_9";
                break;
            default:
                databasenName = "Invalid";
                break;
        }
        return databasenName;
    }


 
}

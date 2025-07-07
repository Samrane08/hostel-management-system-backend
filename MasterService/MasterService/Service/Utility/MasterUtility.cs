namespace MasterService.Service.Utility
{
    public static class MasterUtility
    {
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


        public static DateTime? ConvertToDate(string? date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                if (date.Length == 4)
                {
                    return new DateTime(Convert.ToInt32(date), 1, 1);
                }
                else
                {
                    try
                    {
                        var data = date.Split("/");
                        return new DateTime(Convert.ToInt32(data[2]), Convert.ToInt32(data[1]), Convert.ToInt32(data[0]));
                    }
                    catch (Exception)
                    {
                        try
                        {
                            var data = date.Split("-");
                            return new DateTime(Convert.ToInt32(data[2]), Convert.ToInt32(data[1]), Convert.ToInt32(data[0]));
                        }
                        catch (Exception)
                        {
                            try
                            {
                                var data = Convert.ToDateTime(date);
                                return data;
                            }
                            catch (Exception)
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            else
                return null;
        }
    }
}
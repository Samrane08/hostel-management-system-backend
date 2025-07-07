namespace Model;

public class HostelPreferenceModel
{
    public long? Id { get; set; }   
    public long HostelId { get; set; }
    public string? HostelName { get; set; }   
    public int? DistrictID { get; set; }
    public string? District { get; set; }
    public int? TalukaID { get; set; }
    public string? Taluka { get; set; }
    public int Preference { get; set; }
    public bool? IsNew { get; set; }
}

public class HostelPreferenceModel2
{
    public int Value { get; set; }
    public string? Text { get; set; }
}

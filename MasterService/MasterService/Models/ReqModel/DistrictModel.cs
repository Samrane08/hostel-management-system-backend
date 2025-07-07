namespace MasterService.Models.ReqModel
{
    public class DistrictModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int StateId { get; set; }
        public int? DivisionId { get; set; }
        public int Lang { get; set; }
    }

    public class TalukaModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int DistrictId { get; set; }
        public int Lang { get; set; }
    }

    public class VillageModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int TalukaId { get; set; }
        public int Lang { get; set; }
    }

    public class DivisionModel
    {
        public int DivisionCode { get; set; }
        public string? DivisionName { get; set; }
        public string? DivisionMr { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BenefitAloowanceCategoryModel
    {
        public int Id { get; set; }
        public int DistrictId { get; set; }
        public int TalukaId { get; set; }
        public int VillageId { get; set; }
        public int SwadharCategoryId { get; set; }
        public string? DistrictName { get; set; }
        public string? TalukaName { get; set; }
        public string? VillageName { get; set; }
    }

    public class BenefitAloowanceCategoryInsertModel
    {
        public int CategoryId { get; set; }
        public List<int> VillegeIds { get; set; }
    }
}

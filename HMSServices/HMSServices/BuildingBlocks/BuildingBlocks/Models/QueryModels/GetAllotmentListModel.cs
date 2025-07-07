
using System.Globalization;

using System.Text.Json;
using System.Text.Json.Serialization;


namespace BuildingBlocks.Models.QueryModels
{
    public class GetAllotmentListModel
    {
        public string ApplicationNo { get; set; } = string.Empty;
        public double BeneficiaryAmount { get; set; }


        public string ApplicantName { get; set; } = string.Empty;
    }
    public class GetAllotmentListQueryModel
    {
        public int SrNo { get; set; }
        public int SchemeID { get; set; }
        public int Installment { get; set; }

        public int FinancialYearID { get; set; }
        public int pageNumber { get; set; }

        public int pageSize { get; set; }

        public string dynamicSearchConditions { get; set; } = string.Empty;
        public string applicanttotalAmt { get; set; } = string.Empty;

        public string? ddoTotalAmount { get; set; } = string.Empty;

        public string specifiedLimit { get; set; } = string.Empty;
        public string ddo_code { get; set; } = string.Empty;
        public string deptId { get; set; } = string.Empty;
    }
    public class CustomDateConverter : JsonConverter<DateTime>
    {
        private readonly string _format = "dd-MM-yyyy";  // Target format

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString(), _format, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_format));
        }
    }
    public class TableUtilityModel
    {
        public int? TotalRecords { get; set; }
        private double? _totalAmount;
        public double? TotalAmount
        {
            get { return _totalAmount; }
            set
            {
                _totalAmount = value.HasValue ? Math.Ceiling(value.Value) : (double?)null;
            }
        }

    }
    public class TableFeatureModel
    {
        public int TotalCount { get; set; }


    }

}

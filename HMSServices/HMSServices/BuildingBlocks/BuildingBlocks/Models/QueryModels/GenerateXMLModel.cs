using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.QueryModels
{
    public class GenerateXMLModel
    {
        public string BtachId {  get; set; }=string.Empty;
        public int InstallmentId { get; set; }
        public string UserId {  get; set; }=string.Empty;
        public string IpAddress {  get; set; }=string.Empty;
    }
}

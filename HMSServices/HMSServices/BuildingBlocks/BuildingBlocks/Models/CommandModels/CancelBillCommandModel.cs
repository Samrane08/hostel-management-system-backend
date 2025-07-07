using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.CommandModels
{
    public class CancelBillCommandModel
    {
        public string BatchID {  get; set; }=string.Empty;
        public string UserId {  get; set; }=string.Empty;
        public int SchemeId { get; set; } = 0;
        public string IpAddress {  get; set; }=string.Empty;
    }
}

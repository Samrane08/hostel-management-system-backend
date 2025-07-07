using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.CommonModels
{
    public class CommonResponse
    {
        public bool status {  get; set; }=false;
        public string message { get; set; }=string.Empty;
        

    }
    public class CommonRequest
    {
        public string UserId { get; set; } = string.Empty;

   
        

    }

    public class DebitAccountDetails
    {
        public int SchemeId { get; set; } = 0;

        public string BatchId { get; set; } = string.Empty;
    }
}

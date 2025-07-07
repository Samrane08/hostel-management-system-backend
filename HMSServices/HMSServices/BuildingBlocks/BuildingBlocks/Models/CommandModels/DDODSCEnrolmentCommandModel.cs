using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.CommandModels
{
    public class DDODSCEnrolmentCommandModel
    {
       
        public decimal? tblDDODSCEnrolment_Details { get; set; }
        public IFormFile File { get; set; }
        public string PFXFilePassword { get; set; } = string.Empty;
        public byte[] PFXFile { get; set; }

        public string BatchID { get; set; }
        public string DSCStatus { get; set; } = string.Empty;
        public string RequestFor { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string MaximumAmountofDebitTransaction { get; set; } = string.Empty;

        public int status { get; set; } = 0;
        public string StatusMessage { get; set; }=string.Empty;
    }
}

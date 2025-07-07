using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Models.QueryModels
{
    public class DDOModel
    {
        public string DDOID { get; set; } = string.Empty;
        public string DetailHead { get; set; } = string.Empty;
        public string DDO_Code { get; set; } = string.Empty;
        public string DDO_Name { get; set; } = string.Empty;
        public string DDO_PFXFile { get; set; } = string.Empty;
        public string SchemeCode {  get; set; } = string.Empty;
    }
    public class pfxDDOModel
    {
        public string DDOID { get; set; } = string.Empty;
        public string DetailHead { get; set; } = string.Empty;
        public string DDO_Code { get; set; } = string.Empty;
        public string DDO_Name { get; set; } = string.Empty;
        public byte[] PFXFile { get; set; } = Array.Empty<byte>();
        public string PFXFilePassword {  get; set; } = string.Empty;
        public string pfxFileExpiryDate { get; set; } = string.Empty;
    }
    public class pfxviewDDOModel
    {
        public string DDOID { get; set; } = string.Empty;
        public string DetailHead { get; set; } = string.Empty;
        public string DDO_Code { get; set; } = string.Empty;
        public string DDO_Name { get; set; } = string.Empty;
     
        public string pfxExpiry { get; set; } = string.Empty;
    }
}

using BuildingBlocks.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winnovative;

namespace BuildingBlocks.Utility
{
    public static class Utility
    {

        public static string GetMonthName(int month, string year)
        {
            switch (month)
            {
                case 1: return "January" + "-" + year;
                case 2: return "February" + "-" + year;
                case 3: return "March" + "-" + year;
                case 4: return "April" + "-" + year;
                case 5: return "May" + "-" + year;
                case 6: return "June" + "-" + year;
                case 7: return "July" + "-" + year;
                case 8: return "August" + "-" + year;
                case 9: return "September" + "-" + year;
                case 10: return "October" + "-" + year;
                case 11: return "November" + "-" + year;
                case 12: return "December" + "-" + year;
                default: return "Invalid month number!";
            }
        }
        public static string byteArrayToImage(byte[] BarCodeImage)
        {
            var base64Data = Convert.ToBase64String((byte[])BarCodeImage);
            return "data:image/gif;base64," + base64Data;

        }
        public static async Task<string> ConvertHtmlToPdf(string strReport, string batchId, BillDetailsModel model)
        {
            BarCodeHandler objBarCodeHandler = new BarCodeHandler();
            byte[] BarCodeImage = objBarCodeHandler.ProcessRequest("30", "0", Convert.ToString(model.AuthNo));
            string img = byteArrayToImage(BarCodeImage);

            var replacements = new Dictionary<string, string>
        {
            { "<>AllocatedAmount<>", model.AllocatedAmount },
            { "<>TotalBudget<>",model.MTR_TotalBudget },
            { "<>expTotal<>", model.MTR_expTotal},
              { "<>AccountHolderName<>", model.MTR_AccountHolder},
             { "<>AllocatedAmountInWords<>", model.AllocatedAmountInWords},
             { "<>AddOneRupeesAllocatedAmountWords<>", model.MTR_InwordsExtraOneRuppes},
              { "<>AddOneRupeesAllocatedAmount<>", Convert.ToString(Convert.ToDecimal(model.AllocatedAmount)+1)},
              { "<>RemainingBalance<>",Convert.ToString(Convert.ToDecimal(model.MTR_TotalBudget)- Convert.ToDecimal(model.MTR_expTotal))},
            { "<>TreasuryName<>", model.MTR_TreasuryName },
            { "<>DDOCode<>", model.ddo_code },
            { "<>Designation<>",model.MTR_Designation },
            { "<>OfficePaymentNumber<>",model.OfficePaymentNumber },
            { "<>PanNo<>", model.MTR_PanNo },
            { "<>IFSCCode<>", model.MTR_IFSCCode },
            { "<>BankName<>", model.MTR_BankName },
            { "<>AccountNo<>", model.MTR_AccountNo },
            { "<>PayMonthWord<>", model.PayMonthWord },
            { "<>AuthNo<>", model.AuthNo },
             { "<>ValidTo<>", Convert.ToDateTime(model.MTR_ValidTo).ToString("dd-MM-yyyy") },
            { "<>AuthDate<>", model.AuthDate?.ToString("dd-MM-yyyy") },
            { "<>AdminDept<>",model.MTR_AdminDept },
            { "<>DemandNo<>", model.MTR_DemandNo },
            { "<>MajorHead<>",model.MTR_MajorHead },
            { "<>MinorHead<>", model.MTR_MinorHead },
            { "<>subhead<>", model.MTR_subhead },
            { "<>DetailHead<>",model.MTR_DetailHead },
            { "<>SubDetailHead<>",model.MTR_SubDetailHead },
             { "<>BarCode<>","<img src=" + img + "alt='test'>" },
            { "<>SchemeCode<>", model.BeamsSchemeCode }
        };

            foreach (var kvp in replacements)
            {
                strReport = strReport.Replace(kvp.Key, kvp.Value);
            }



            string pdfDirectory = Path.Combine(Directory.GetCurrentDirectory(), "ConvertedPdf");

            if (!Directory.Exists(pdfDirectory))
                Directory.CreateDirectory(pdfDirectory);


            string pdfFilePath = Path.Combine(pdfDirectory, batchId + "MTR45.pdf");

            ConvertURLToPDF(strReport, pdfFilePath);

            byte[] pdfBytes = await System.IO.File.ReadAllBytesAsync(pdfFilePath);
            string base64String = Convert.ToBase64String(pdfBytes);
            return base64String;

        }

        private static void ConvertURLToPDF(string htmlContent, string pdfFilePath)
        {
            PdfConverter pdfConverter = new PdfConverter();

            //  pdfConverter.LicenseKey = "XNLB08LTxMLTyt3D08DC3cLB3crKyso="; //Live Key
            pdfConverter.PdfSecurityOptions.CanEditContent = false;
            pdfConverter.PdfDocumentOptions.BottomMargin = 20;
            pdfConverter.PdfDocumentOptions.TopMargin = 20;
            pdfConverter.PdfDocumentOptions.LeftMargin = 30;
            pdfConverter.PdfDocumentOptions.RightMargin = 20;
            // set the converter options - optional
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
            pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
            pdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
            // set if header and footer are shown in the PDF - optional - default is false
            pdfConverter.PdfDocumentOptions.ShowHeader = false;
            pdfConverter.PdfDocumentOptions.ShowFooter = false;
            pdfConverter.PdfDocumentOptions.FitWidth = true;
            // set the embedded fonts option - optional - default is false
            pdfConverter.PdfDocumentOptions.EmbedFonts = true;
            // set the live HTTP links option - optional - default is true
            pdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
            pdfConverter.PdfDocumentOptions.JpegCompressionEnabled = true;
            pdfConverter.PdfDocumentInfo.AuthorName = "MahaOnline Ltd";
            pdfConverter.SavePdfFromHtmlStringToFile(htmlContent, pdfFilePath);

            //  pdfConverter.SavePdfFromHtmlStringToFile(htmlContent, pdfFilePath);
        }

    }
}

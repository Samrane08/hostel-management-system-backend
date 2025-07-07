
using System.Security.Cryptography.X509Certificates;



namespace DDO_Service.API.Features.Common
{
    public static class CommonFunc
    {


        private static readonly string[] Units =
    {
        "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
        "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"
    };

        private static readonly string[] Tens =
        {
        "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
    };

        public static string ConvertAmountToWords(decimal amount)
        {
            if (amount == 0) return "Zero Rupees";

            var integerPart = (long)amount;
            var decimalPart = (int)((amount - integerPart) * 100); // Extract paise

            string words = ConvertNumberToWords(integerPart) + " Rupees";

            if (decimalPart > 0)
            {
                words += " and " + ConvertNumberToWords(decimalPart) + " Paise";
            }

            return words + " Only";
        }

        private static string ConvertNumberToWords(long number)
        {
            if (number == 0) return "";

            if (number < 20)
                return Units[number];

            if (number < 100)
                return Tens[number / 10] + (number % 10 != 0 ? " " + Units[number % 10] : "");

            if (number < 1000)
                return Units[number / 100] + " Hundred" + (number % 100 != 0 ? " and " + ConvertNumberToWords(number % 100) : "");

            if (number < 100000)
                return ConvertNumberToWords(number / 1000) + " Thousand" + (number % 1000 != 0 ? " " + ConvertNumberToWords(number % 1000) : "");

            if (number < 10000000)
                return ConvertNumberToWords(number / 100000) + " Lakh" + (number % 100000 != 0 ? " " + ConvertNumberToWords(number % 100000) : "");

            return ConvertNumberToWords(number / 10000000) + " Crore" + (number % 10000000 != 0 ? " " + ConvertNumberToWords(number % 10000000) : "");
        }
        //private IDapper dapper;
        //public CommonFunc(IDapper _dapper)
        //{
        //    dapper = _dapper;
        //}




        //public async Task<int> InsertBEAMSXMLBillGenerationRequestDetails(GenerateXMLResponseModel model, string xmlString)
        //{
        //    try
        //    {
        //        var myparam = new DynamicParameters();
        //        myparam.Add("p_BillType", model.BillType, DbType.String);
        //        myparam.Add("p_FinYear2", model.FinYear2, DbType.Int32);
        //        myparam.Add("p_FinYear1", model.FinYear1, DbType.Int32);
        //        myparam.Add("p_BeneficiaryCount", model.BeneficiaryCount, DbType.Int32);
        //        myparam.Add("p_PayeeCount", model.PayeeCount, DbType.Int32);
        //        myparam.Add("p_BulkFlag", model.BulkFlag, DbType.String);
        //        myparam.Add("p_BillCreationDate", model.BillCreationDate.ToString("dd-MM-yyyy"), DbType.String);
        //        myparam.Add("p_PayYear", model.PayYear, DbType.Int32);
        //        myparam.Add("p_BillPortalName", model.BillPortalName, DbType.String);
        //        myparam.Add("p_PaybillId", model.PayBillId, DbType.String);
        //        myparam.Add("p_DDOCode", model.DDOCode, DbType.String);
        //        myparam.Add("p_DetailHead", model.DetailHead, DbType.String);
        //        myparam.Add("p_SchemeCode", model.SchemeCode, DbType.String);
        //        myparam.Add("p_FormId", model.FormID, DbType.String);
        //        myparam.Add("p_GrossAmount", model.GrossAmount, DbType.String);
        //        myparam.Add("p_TotalDeduction", model.TotalDeduction, DbType.String);
        //        myparam.Add("p_PayMonth", model.PayMonth, DbType.String);
        //        myparam.Add("p_PaymentMode", model.PaymentMode, DbType.String); 
        //        myparam.Add("p_TotalGISDeduction", "0", DbType.String);
        //        myparam.Add("p_IsProcessed", false, DbType.Boolean);
        //        myparam.Add("p_XMLString", xmlString, DbType.String);
        //        myparam.Add("p_IsActive", true, DbType.Boolean);
        //        myparam.Add("p_CreatedBy", "7", DbType.String);
        //        myparam.Add("p_IPAddress", "127.89.90", DbType.String);
        //        var result = await Task.FromResult(dapper.Insert<int>("usp_InsertBEAMSXMLBillGenerationRequestDetails", myparam, commandType: System.Data.CommandType.StoredProcedure));
        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //public async Task<int> InsertBEAMSXMLBillGenerationResponseDetails(BEAMSXMLBillGenerationResponseCommandModel model, string xmlString)
        //{
        //    try
        //    {
        //        var myparam = new DynamicParameters();
        //        myparam.Add("p_AuthNo", model.AuthNo, DbType.String);
        //        myparam.Add("p_StatusCode", model.StatusCode, DbType.String);
        //        myparam.Add("p_DDOCode", model.DDOCode, DbType.String);
        //        myparam.Add("p_BatchID", model.BatchID, DbType.String);
        //        myparam.Add("p_BillPDF", model.BillPDF, DbType.Binary);
        //        myparam.Add("p_IsActive",true, DbType.Boolean);
        //        myparam.Add("p_CreatedBy", "7", DbType.String);
        //        myparam.Add("p_BeamsBillRequestID", model.BeamsBillRequestID, DbType.Int32);

        //        var result = await Task.FromResult(dapper.Insert<int>("usp_Insert_BEAMSXMLBillGenerationResponse_Details", myparam, commandType: System.Data.CommandType.StoredProcedure));
        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        public static string GenerateBatchID()
        {

            return String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);
        }
        //public  static async  List<AuthorizationSlip> GetAuthorizationSlip(string xmlString)
        //{




        //  //  BeamsAuthorizationService. objAuthorizationService = new BeamsAuthorizationService();
        //   // LiveBEAMSBillGeneration.AuthorizationSlip[] resultAuthorizationSlip = objAuthorizationService.getAuthSlip(xmlString);
        //    List<AuthorizationSlip> authmodel = new List<AuthorizationSlip>();
        //    var sampleAuthSlip = new AuthorizationSlip
        //    {

        //        authNO = "AUTH123456",
        //        authPdf = null, // Sample Base64 PDF data
        //        budgetYear1 = "2023",
        //        budgetYear2 = "2024",
        //        ddoCode = "DDO001234",
        //        expTotal = "500000.00",
        //        statusCode = "00",
        //        totalBudget = "1000000.00",
        //        transNo = "TRANS98765",
        //        validTo = "2024-12-31"
        //    };

        //    authmodel.Add(sampleAuthSlip);
        //    return authmodel;
        //}

        //public async Task<int> InsertMTR45Details(InsertMTRCommandModel model)
        //{
        //    if(Convert.ToInt32(model.PayMonth)<10)
        //        model.PayMonth="0"+model.PayMonth;
        //    if(Convert.ToInt32(model.BillType)<10)
        //        model.BillType="0"+model.BillType;

        //    try
        //    {
        //        var myparam = new DynamicParameters();

        //        myparam.Add("p_PaybillId", model.PaybillId, DbType.String);
        //        myparam.Add("p_PayYear", model.PayYear, DbType.Int32);
        //        myparam.Add("p_PayMonth", model.PayMonth, DbType.String);
        //        myparam.Add("p_BeneficiaryCount", model.BeneficiaryCount, DbType.Int32);
        //        myparam.Add("p_GrossAmount", model.GrossAmount, DbType.String);
        //        myparam.Add("p_FinYear2", model.FinYear2, DbType.Int32);
        //        myparam.Add("p_FinYear1", model.FinYear1, DbType.Int32);
        //        myparam.Add("p_FormId", model.FormId, DbType.String);
        //        myparam.Add("p_PaymentMode", model.PaymentMode, DbType.String);
        //        myparam.Add("p_BifurcatedGISDedMap", model.BifurcatedGISDedMap, DbType.String);
        //        myparam.Add("p_TotalDeduction", model.TotalDeduction, DbType.String);
        //        myparam.Add("p_SchemeCode", model.SchemeCode, DbType.String);
        //        myparam.Add("p_DetailHead", model.DetailHead, DbType.String);
        //        myparam.Add("p_DDOCode", model.DDOCode, DbType.String);
        //        myparam.Add("p_BulkFlag", model.BulkFlag, DbType.String);
        //        myparam.Add("p_BillCreationDate", model.BillCreationDate, DbType.String);
        //        myparam.Add("p_PayeeCount", model.PayeeCount, DbType.Int32);
        //        myparam.Add("p_BillType", model.BillType, DbType.String);
        //        myparam.Add("p_PayeeType", model.PayeeType, DbType.String);
        //        myparam.Add("p_MTRBillPortalName", model.MTRBillPortalName, DbType.String);
        //        myparam.Add("p_XMLString", model.XMLString, DbType.String);
        //        myparam.Add("p_IsActive", model.IsActive, DbType.Boolean);
        //        myparam.Add("p_CreatedBy", model.CreatedBy, DbType.String);
        //        myparam.Add("p_IPAddress", model.IPAddress, DbType.String);
        //        myparam.Add("p_BeamsBillRequestID", model.BeamsBillRequestID, DbType.Int32);

        //        var result = await Task.FromResult(dapper.Insert<int>("usp_InsertMTRDetailsRequest", myparam, commandType: System.Data.CommandType.StoredProcedure));
        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        public static DDODSCEnrolmentCommandModel CheckWheathervalidpfxfile(IFormFile file, string pfxpassword)
        {
            var objDDODSCEnrolmentModel = new DDODSCEnrolmentCommandModel();
            try
            {


                using var fs = file.OpenReadStream();
                using var br = new BinaryReader(fs);
                objDDODSCEnrolmentModel.PFXFile = br.ReadBytes((int)file.Length);
                var cert = new X509Certificate2(objDDODSCEnrolmentModel.PFXFile, pfxpassword,
                                            X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);


                if (!cert.HasPrivateKey)
                {
                    objDDODSCEnrolmentModel.status = 0;
                    objDDODSCEnrolmentModel.StatusMessage = "Uploaded file is invalid (missing private key).";
                    return objDDODSCEnrolmentModel;
                }

                objDDODSCEnrolmentModel.BatchID = $"{DateTime.Now.Ticks / 10 % 1000000000:D9}";
                objDDODSCEnrolmentModel.status = 1;
                objDDODSCEnrolmentModel.StatusMessage = "Success";


            }

            catch (Exception)
            {
                objDDODSCEnrolmentModel.status = 0;
                objDDODSCEnrolmentModel.StatusMessage = "Uploaded file is invalid (missing private key).";


            }
            return objDDODSCEnrolmentModel;
        }
        public static string GetPfxFileExpiryDate(byte[] file, string pfxpassword)
        {

            string strSignedXML = "";
            try
            {
                if (file != null && !string.IsNullOrEmpty(pfxpassword))
                {
                    X509Certificate2 cert = new X509Certificate2(file, pfxpassword);
                    strSignedXML = cert.GetExpirationDateString();

                }
            }
            catch (Exception ex)
            {

                return "";

            }

            return strSignedXML;
        }
        //public async Task<string> GetStatusDescription(string Statuscode)
        //{
        //    try
        //    {
        //        var myparam = new DynamicParameters();

        //        myparam.Add("p_StatusCode", Statuscode, DbType.String);
        //        var result = await Task.FromResult(dapper.Get<string>("usp_GetStatusDescriptionByStatusCode", myparam, commandType: System.Data.CommandType.StoredProcedure));
        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }


    public record CheckDDO_BalanceQueryRequest(CheckDDO_BalanceQueryRequestModel model) : IQuery<int>;

    public class CheckDDO_BalanceQueryRequestValidator : AbstractValidator<CheckDDO_BalanceQueryRequest>
    {
        public CheckDDO_BalanceQueryRequestValidator()
        {
            RuleFor(x => x.model.Schemecode).NotEmpty().WithMessage("Scheme code can't be empty.");
            RuleFor(x => x.model.DDOCode).NotEmpty().WithMessage("DDOCode can't be empty.");
            RuleFor(x => x.model.Year1).NotEmpty().WithMessage("Year1 can't be empty.");
            RuleFor(x => x.model.Year2).NotEmpty().WithMessage("Year2 can't be empty.");
            RuleFor(x => x.model.DetailsHead).NotEmpty().WithMessage("Detailhead can't be empty.");
            RuleFor(x => x.model.CreatedBy).NotEmpty().WithMessage("CreatedBy can't be empty.");

        }

    }
    internal class CheckDDO_BalanceQueryHandler : IQueryHandler<CheckDDO_BalanceQueryRequest, int>
    {

        private readonly IDapper _dapper;
        public CheckDDO_BalanceQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<int> Handle(CheckDDO_BalanceQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {

                var MyParam = new DynamicParameters();
                MyParam.Add("p_Year1", request.model.Year1, DbType.Int32);
                MyParam.Add("p_Year2", request.model.Year2, DbType.Int32);
                MyParam.Add("p_SchemeDDOMapID", 0, DbType.Int32);
                MyParam.Add("p_DDOcode", request.model.DDOCode, DbType.String);
                MyParam.Add("p_Schemecode", request.model.Schemecode, DbType.String);
                MyParam.Add("p_Detailhead", request.model.DetailsHead, DbType.String);
                //MyParam.Add("p_IsActive",1, DbType.String);
                MyParam.Add("p_CreatedBy", request.model.CreatedBy, DbType.String);


                var result = await Task.FromResult(_dapper.Insert<int>("InsertIntoDDOCheckAvailableBalance_Request", MyParam, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    public record GetFinancialYearQueryBySchemeId(int Id) : IQuery<string>;

    public class GetFinancialYearQueryBySchemeIdValidator : AbstractValidator<GetFinancialYearQueryBySchemeId>
    {
        public GetFinancialYearQueryBySchemeIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Financial year Id can't be empty.")
                .GreaterThan(0).WithMessage("Financial year Id must be greather than 0");

        }

    }
    internal class GetFinancialYearQueryBySchemeIdHandler : IQueryHandler<GetFinancialYearQueryBySchemeId, string>
    {

        private readonly IDapper _dapper;
        public GetFinancialYearQueryBySchemeIdHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<string> Handle(GetFinancialYearQueryBySchemeId request, CancellationToken cancellationToken)
        {
            try
            {

                var finParam = new DynamicParameters();
                finParam.Add("p_Id", request.Id, DbType.Int32);

                var financialYearResult = await Task.FromResult(_dapper.Get<string>("usp_GetfinancialyearById", finParam, commandType: CommandType.StoredProcedure));


                var finYearParts = financialYearResult.Split("-");

                if (Convert.ToInt32(finYearParts[0]) <= 0 && Convert.ToInt32(finYearParts[1]) <= 0)
                {
                    throw new Exception("Invalid financial year format");
                }
                var RS = Convert.ToString(finYearParts[0]) + ":" + Convert.ToString(finYearParts[1]);

                return RS;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public record GetDDODetailsBySchemeIdAndUserId(int schemeID, string userId) : IQuery<DDODetailsModel>;

    public class GetDDODetailsBySchemeIdAndUserIdValidator : AbstractValidator<GetDDODetailsBySchemeIdAndUserId>
    {
        public GetDDODetailsBySchemeIdAndUserIdValidator()
        {
            RuleFor(x => x.schemeID).NotEmpty().WithMessage("schemeID can not be empty.")
                .GreaterThan(0)
            .WithMessage("schemeID MUST BE GREATER THAN 0");
            RuleFor(x => x.userId).NotEmpty().WithMessage("userId can not be empty.");

        }


    }
    internal class GetDDODetailsBySchemeIdAndUserIdQueryHandler : IQueryHandler<GetDDODetailsBySchemeIdAndUserId, DDODetailsModel>
    {

        private readonly IDapper _dapper;
        public GetDDODetailsBySchemeIdAndUserIdQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<DDODetailsModel> Handle(GetDDODetailsBySchemeIdAndUserId request, CancellationToken cancellationToken)
        {
            try
            {

                var myparam = new DynamicParameters();
                myparam.Add("p_SchemeID", request.schemeID, DbType.Int32);
                myparam.Add("p_UserId", request.userId, DbType.String);
                var result = await Task.FromResult(_dapper.Get<DDODetailsModel>("usp_GetDDODetails", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return result;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public record InsertBEAMSXMLBillGenerationRequestDetailsCommand(GenerateXMLResponseModel model) : ICommand<int>;

    public class InsertBEAMSXMLBillGenerationRequestDetailsCommandValidator : AbstractValidator<InsertBEAMSXMLBillGenerationRequestDetailsCommand>
    {
        public InsertBEAMSXMLBillGenerationRequestDetailsCommandValidator()
        {
            RuleFor(x => x.model.BillType).NotEmpty().WithMessage("BillType can't be empty.");
            RuleFor(x => x.model.FinYear2).NotEmpty().WithMessage("FinYear2 can't be empty.");
            RuleFor(x => x.model.FinYear1).NotEmpty().WithMessage("FinYear1 can't be empty.");
            RuleFor(x => x.model.BeneficiaryCount).NotEmpty().WithMessage("BeneficiaryCount can't be empty.")
             .GreaterThan(0).WithMessage("BeneficiaryCount must be greater than 0");
            RuleFor(x => x.model.BillCreationDate).NotEmpty().WithMessage("BillCreationDate can't be empty.");
            RuleFor(x => x.model.PayYear).NotEmpty().WithMessage("PayYear can't be empty.");
            RuleFor(x => x.model.BillPortalName).NotEmpty().WithMessage("BillPortalName can't be empty.");
            RuleFor(x => x.model.PayBillId).NotEmpty().WithMessage("PayBillId can't be empty.");
            RuleFor(x => x.model.DDOCode).NotEmpty().WithMessage("DDOCode can't be empty.");
            RuleFor(x => x.model.DetailHead).NotEmpty().WithMessage("DetailHead can't be empty.");
            RuleFor(x => x.model.SchemeCode).NotEmpty().WithMessage("SchemeCode can't be empty.");
            RuleFor(x => x.model.PayMonth).NotEmpty().WithMessage("PayMonth can't be empty.");
            RuleFor(x => x.model.xmlString).NotEmpty().WithMessage("xmlString can't be empty.");
            RuleFor(x => x.model.UserId).NotEmpty().WithMessage("UserId can't be empty.");

        }

    }


    internal class InsertBEAMSXMLBillGenerationRequestDetailsCommandHandler : ICommandHandler<InsertBEAMSXMLBillGenerationRequestDetailsCommand, int>
    {

        private readonly IDapper _dapper;
        public InsertBEAMSXMLBillGenerationRequestDetailsCommandHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<int> Handle(InsertBEAMSXMLBillGenerationRequestDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var myparam = new DynamicParameters();
                myparam.Add("p_BillType", request.model.BillType, DbType.String);
                myparam.Add("p_FinYear2", request.model.FinYear2, DbType.Int32);
                myparam.Add("p_FinYear1", request.model.FinYear1, DbType.Int32);
                myparam.Add("p_BeneficiaryCount", request.model.BeneficiaryCount, DbType.Int32);
                myparam.Add("p_PayeeCount", request.model.PayeeCount, DbType.Int32);
                myparam.Add("p_BulkFlag", request.model.BulkFlag, DbType.String);
                myparam.Add("p_BillCreationDate", request.model.BillCreationDate.ToString("dd-MM-yyyy"), DbType.String);
                myparam.Add("p_PayYear", request.model.PayYear, DbType.Int32);
                myparam.Add("p_BillPortalName", request.model.BillPortalName, DbType.String);
                myparam.Add("p_PaybillId", request.model.PayBillId, DbType.String);
                myparam.Add("p_DDOCode", request.model.DDOCode, DbType.String);
                myparam.Add("p_DetailHead", request.model.DetailHead, DbType.String);
                myparam.Add("p_SchemeCode", request.model.SchemeCode, DbType.String);
                myparam.Add("p_FormId", request.model.FormID, DbType.String);
                myparam.Add("p_GrossAmount", request.model.GrossAmount, DbType.String);
                myparam.Add("p_TotalDeduction", request.model.TotalDeduction, DbType.String);
                myparam.Add("p_PayMonth", request.model.PayMonth, DbType.String);
                myparam.Add("p_PaymentMode", request.model.PaymentMode, DbType.String);
                myparam.Add("p_TotalGISDeduction", "0", DbType.String);
                myparam.Add("p_IsProcessed", false, DbType.Boolean);
                myparam.Add("p_XMLString", request.model.xmlString, DbType.String);
                myparam.Add("p_IsActive", true, DbType.Boolean);
                myparam.Add("p_CreatedBy", request.model.UserId, DbType.String);
                myparam.Add("p_IPAddress", request.model.IpAddress, DbType.String);
                var result = await Task.FromResult(_dapper.Insert<int>("usp_InsertBEAMSXMLBillGenerationRequestDetails", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return result;

            }
            catch (Exception)
            {
                throw;
            }
        }


    }

    public record InsertBEAMSXMLBillGenerationResponseDetailsCommand(BEAMSXMLBillGenerationResponseCommandModel model) : ICommand<int>;

    public class InsertBEAMSXMLBillGenerationResponseDetailsCommandValidator : AbstractValidator<InsertBEAMSXMLBillGenerationResponseDetailsCommand>
    {
        public InsertBEAMSXMLBillGenerationResponseDetailsCommandValidator()
        {
            RuleFor(x => x.model.AuthNo).NotEmpty().WithMessage("AuthNo can't be empty.");
            RuleFor(x => x.model.StatusCode).NotEmpty().WithMessage("StatusCode can't be empty.");
            RuleFor(x => x.model.DDOCode).NotEmpty().WithMessage("DDOCode can't be empty.");
            RuleFor(x => x.model.BatchID).NotEmpty().WithMessage("BatchID can't be empty.");
            RuleFor(x => x.model.UserId).NotEmpty().WithMessage("UserId can't be empty.");
            RuleFor(x => x.model.BeamsBillRequestID).NotEmpty().WithMessage("BeamsBillRequestID can't be empty.")
                .GreaterThan(0).WithMessage("BeamsBillRequestID must be greater than 0");
            RuleFor(x => x.model.MTR_Year2).NotEmpty().WithMessage("MTR_Year2 can't be empty.");
            RuleFor(x => x.model.MTR_Year1).NotEmpty().WithMessage("MTR_Year1 can't be empty.");
            RuleFor(x => x.model.DDOCode).NotEmpty().WithMessage("DDOCode can't be empty.");
            RuleFor(x => x.model.MTR_expTotal).NotEmpty().WithMessage("MTR_expTotal can't be empty.");
            RuleFor(x => x.model.MTR_TotalBudget).NotEmpty().WithMessage("MTR_TotalBudget can't be empty.");
            RuleFor(x => x.model.MTR_ValidTo).NotEmpty().WithMessage("MTR_ValidTo can't be empty.");
            RuleFor(x => x.model.MTR_additionalFields).NotEmpty().WithMessage("MTR_additionalFields can't be empty.");
        

        }

    }

    internal class InsertBEAMSXMLBillGenerationResponseDetailsCommandHandler : ICommandHandler<InsertBEAMSXMLBillGenerationResponseDetailsCommand, int>
    {

        private readonly IDapper _dapper;
        public InsertBEAMSXMLBillGenerationResponseDetailsCommandHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<int> Handle(InsertBEAMSXMLBillGenerationResponseDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_AuthNo", request.model.AuthNo, DbType.String);
                myparam.Add("p_StatusCode", request.model.StatusCode, DbType.String);
                myparam.Add("p_DDOCode", request.model.DDOCode, DbType.String);
                myparam.Add("p_BatchID", request.model.BatchID, DbType.String);
                myparam.Add("p_BillPDF", request.model.BillPDF, DbType.Binary);
                myparam.Add("p_IsActive", true, DbType.Boolean);
                myparam.Add("p_CreatedBy", request.model.UserId, DbType.String);
                myparam.Add("p_BeamsBillRequestID", request.model.BeamsBillRequestID, DbType.Int32);
                myparam.Add("p_MTR_Year2", request.model.MTR_Year2, DbType.Int32);
                myparam.Add("p_MTR_Year1", request.model.MTR_Year1, DbType.Int32);
                myparam.Add("p_MTR_expTotal", request.model.MTR_expTotal, DbType.String);
                myparam.Add("p_MTR_TotalBudget", request.model.MTR_TotalBudget, DbType.String);
                myparam.Add("p_MTR_TransNo", request.model.MTR_TransNo, DbType.String);
                myparam.Add("p_MTR_ValidTo", request.model.MTR_ValidTo, DbType.String);
                myparam.Add("p_MTR_additionalFields", request.model.MTR_additionalFields, DbType.String);

                var result = await Task.FromResult(_dapper.Insert<int>("usp_Insert_BEAMSXMLBillGenerationResponse_Details", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return result;

            }
            catch (Exception)
            {
                throw;
            }
        }


    }

    public record InsertMTR45DetailsCommand(InsertMTRCommandModel model) : ICommand<int>;

    public class InsertMTR45DetailsCommandValidator : AbstractValidator<InsertMTR45DetailsCommand>
    {
        public InsertMTR45DetailsCommandValidator()
        {
            RuleFor(x => x.model.PaybillId).NotEmpty().WithMessage("PaybillId can't be empty.");
            RuleFor(x => x.model.PayYear).NotEmpty().WithMessage("PayYear can't be empty.");
            RuleFor(x => x.model.PayMonth).NotEmpty().WithMessage("PayMonth can't be empty.");
            RuleFor(x => x.model.BeneficiaryCount).NotEmpty().WithMessage("BeneficiaryCount can't be empty.")
                .GreaterThan(0).WithMessage("BeneficiaryCount must be greater than 0");
            RuleFor(x => x.model.FinYear2).NotEmpty().WithMessage("FinYear2 can't be empty.");
            RuleFor(x => x.model.FinYear1).NotEmpty().WithMessage("FinYear1 can't be empty.");
            RuleFor(x => x.model.SchemeCode).NotEmpty().WithMessage("SchemeCode can't be empty.");
            RuleFor(x => x.model.DetailHead).NotEmpty().WithMessage("DetailHead can't be empty.");
            RuleFor(x => x.model.DDOCode).NotEmpty().WithMessage("DDOCode can't be empty.");
            RuleFor(x => x.model.BillCreationDate).NotEmpty().WithMessage("BillCreationDate can't be empty.");
            RuleFor(x => x.model.BillType).NotEmpty().WithMessage("BillType can't be empty.");
            RuleFor(x => x.model.MTRBillPortalName).NotEmpty().WithMessage("MTRBillPortalName can't be empty.");
            RuleFor(x => x.model.XMLString).NotEmpty().WithMessage("XMLString can't be empty.");
            RuleFor(x => x.model.BeamsBillRequestID).NotEmpty().WithMessage("BeamsBillRequestID can't be empty.")
                .GreaterThan(0).WithMessage("BeamsBillRequestID must be greater than 0");


        }

    }

    internal class InsertMTR45DetailsCommandHandler : ICommandHandler<InsertMTR45DetailsCommand, int>
    {

        private readonly IDapper _dapper;
        public InsertMTR45DetailsCommandHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<int> Handle(InsertMTR45DetailsCommand request, CancellationToken cancellationToken)
        {

            try
            {
                if (Convert.ToInt32(request.model.PayMonth) < 10)
                    request.model.PayMonth = "0" + request.model.PayMonth;
                if (Convert.ToInt32(request.model.BillType) < 10)
                    request.model.BillType = "0" + request.model.BillType;
                var myparam = new DynamicParameters();

                myparam.Add("p_PaybillId", request.model.PaybillId, DbType.String);
                myparam.Add("p_PayYear", request.model.PayYear, DbType.Int32);
                myparam.Add("p_PayMonth", request.model.PayMonth, DbType.String);
                myparam.Add("p_BeneficiaryCount", request.model.BeneficiaryCount, DbType.Int32);
                myparam.Add("p_GrossAmount", request.model.GrossAmount, DbType.String);
                myparam.Add("p_FinYear2", request.model.FinYear2, DbType.Int32);
                myparam.Add("p_FinYear1", request.model.FinYear1, DbType.Int32);
                myparam.Add("p_FormId", request.model.FormId, DbType.String);
                myparam.Add("p_PaymentMode", request.model.PaymentMode, DbType.String);
                myparam.Add("p_BifurcatedGISDedMap", request.model.BifurcatedGISDedMap, DbType.String);
                myparam.Add("p_TotalDeduction", request.model.TotalDeduction, DbType.String);
                myparam.Add("p_SchemeCode", request.model.SchemeCode, DbType.String);
                myparam.Add("p_DetailHead", request.model.DetailHead, DbType.String);
                myparam.Add("p_DDOCode", request.model.DDOCode, DbType.String);
                myparam.Add("p_BulkFlag", request.model.BulkFlag, DbType.String);
                myparam.Add("p_BillCreationDate", request.model.BillCreationDate, DbType.String);
                myparam.Add("p_PayeeCount", request.model.PayeeCount, DbType.Int32);
                myparam.Add("p_BillType", request.model.BillType, DbType.String);
                myparam.Add("p_PayeeType", request.model.PayeeType, DbType.String);
                myparam.Add("p_MTRBillPortalName", request.model.MTRBillPortalName, DbType.String);
                myparam.Add("p_XMLString", request.model.XMLString, DbType.String);
                myparam.Add("p_IsActive", request.model.IsActive, DbType.Boolean);
                myparam.Add("p_CreatedBy", request.model.CreatedBy, DbType.String);
                myparam.Add("p_IPAddress", request.model.IPAddress, DbType.String);
                myparam.Add("p_BeamsBillRequestID", request.model.BeamsBillRequestID, DbType.Int32);

                var result = await Task.FromResult(_dapper.Insert<int>("usp_InsertMTRDetailsRequest", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return result;


            }
            catch (Exception)
            {
                throw;
            }
        }


    }

    public record GetStatusDescription(string Statuscode) : IQuery<string>;
    internal class GetStatusDescriptionQueryHandler : IQueryHandler<GetStatusDescription, string>
    {

        private readonly IDapper _dapper;
        public GetStatusDescriptionQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<string> Handle(GetStatusDescription request, CancellationToken cancellationToken)
        {

            try
            {
                var myparam = new DynamicParameters();

                myparam.Add("p_StatusCode", request.Statuscode, DbType.String);
                var result = await Task.FromResult(_dapper.Get<string>("usp_GetStatusDescriptionByStatusCode", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return result;


            }
            catch (Exception)
            {
                throw;
            }
        }


    }

}




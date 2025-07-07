
using BeamsAuthorizationService;
using BeamsCheckBalance;
using System.Collections;

namespace DDO_Service.API.Features.GenerateXML
{
    public record GenerateXMLQuery(GenerateXMLModel model) : IQuery<CommonResponse>;
    public record GenerateXMLResult(int statusCode, bool status, string message);
    public record GenerateXMLErrorResponse(int statusCode, bool status, string message);

    public class GenerateXMLQueryValidator : AbstractValidator<GenerateXMLQuery>
    {
        public GenerateXMLQueryValidator()
        {
            RuleFor(x => x.model.BtachId).NotEmpty().WithMessage("BtachId can't be empty.")
                 .MinimumLength(6).WithMessage("BtachId can't be less than 6 characters");


        }

    }
    public class GenerateXML : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            _ = app.MapPost("/ddo/submit-bill",
                async (GenerateXMLModel request, ISender sender, HttpContext context, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
                {
                    try
                    {
                        request.UserId = claimsService.GetUserId();
                        request.IpAddress = context.Connection.RemoteIpAddress?.ToString();
                        if (!string.IsNullOrEmpty(request.UserId))
                        {
                            var reslt = await sender.Send(new GenerateXMLQuery(request));
                            var resultmodel = reslt.Adapt<CommonResponse>();



                            if (resultmodel.status) return Results.Ok(new GenerateXMLResult(StatusCodes.Status200OK, true, resultmodel.message));
                            else return Results.Ok(new GenerateXMLErrorResponse(StatusCodes.Status200OK, false, resultmodel.message));
                        }
                        else
                        {
                            return Results.Ok(new GenerateXMLErrorResponse(StatusCodes.Status200OK, false, "Internal server error"));
                        }
                    }
                    catch (Exception)

                    {
                        return Results.BadRequest(new GenerateXMLErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }
                })
            .WithName("SubmitBill")
          .RequireAuthorization();
        }

        internal class GenerateXMLQueryHandler : IQueryHandler<GenerateXMLQuery, CommonResponse>
        {

            private readonly IDapper _dapper;
            private readonly ISender _sender;
            public GenerateXMLQueryHandler(IDapper dapper, ISender sender)
            {
                _dapper = dapper;
                _sender = sender;
            }

            public async Task<CommonResponse> Handle(GenerateXMLQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var res = new CommonResponse();


                    var myparam = new DynamicParameters();
                    myparam.Add("p_BatchID", Convert.ToInt32(request.model.BtachId), DbType.Int32);
                    // myparam.Add("p_Installment", request.model.InstallmentId, DbType.Int32);
                    var data = await Task.FromResult(_dapper.Get<GenerateXMLResponseModel>("USP_GetBeamsXMLRequestDetails", myparam, commandType: System.Data.CommandType.StoredProcedure));
                    if (data != null && !string.IsNullOrEmpty(data.FinYear1) && !string.IsNullOrEmpty(data.PayMonth) && data.BeneficiaryCount>0 && !string.IsNullOrEmpty(data.GrossAmount))
                    {
                        data.UserId = request.model.UserId;
                        data.IpAddress = request.model.IpAddress;
                        data.xmlString = GenerateXMLString(data);
                        if (!string.IsNullOrEmpty(data.xmlString))
                        {



                            int result = await _sender.Send(new InsertBEAMSXMLBillGenerationRequestDetailsCommand(data));
                            if (result > 0)
                            {
                                var mm = new BEAMSXMLBillGenerationResponseCommandModel();
                                mm.UserId = request.model.UserId;
                                int IsInserted = 0;
                                var client = new AuthorizationServicePortTypeClient(AuthorizationServicePortTypeClient.EndpointConfiguration.AuthorizationServiceHttpSoap11Endpoint);


                                string xmlRequest = "<collection><BillType>01</BillType><FinYear2>2025</FinYear2><FinYear1>2024</FinYear1><BeneficiaryCount>1</BeneficiaryCount><PayeeCount>1</PayeeCount><BulkFlag>Y</BulkFlag><BillCreationDate>2025-02-14 13:02:48</BillCreationDate><PayYear>2025</PayYear><BillPortalName>MAHADBT</BillPortalName><PaybillId>924750750</PaybillId>  <BifurcatedGISDedMap/><GISClassCount>0</GISClassCount><DDOCode>2201005648</DDOCode><DetailHead>34</DetailHead><SchemeCode>22250403</SchemeCode><FormId>MTR45</FormId><GrossAmount>2050</GrossAmount><TotalDeduction>0</TotalDeduction><PayMonth>02</PayMonth><PaymentMode>CMP</PaymentMode><TotalGISDeduction>0</TotalGISDeduction><PayeeType>D</PayeeType><PaymentFile>Y</PaymentFile></collection>";


                                var response = await client.getAuthSlipAsync(xmlRequest);

                              //  var checkBalance = new GetBalancePortTypeClient(GetBalancePortTypeClient.EndpointConfiguration.GetBalanceHttpSoap11Endpoint);


                               // string balancexml = "<collection><BillType>01</BillType><FinYear2>2025</FinYear2><FinYear1>2024</FinYear1><BeneficiaryCount>1</BeneficiaryCount><PayeeCount>1</PayeeCount><BulkFlag>Y</BulkFlag><BillCreationDate>2025-02-14 13:02:48</BillCreationDate><PayYear>2025</PayYear><BillPortalName>MAHADBT</BillPortalName><PaybillId>924750750</PaybillId>  <BifurcatedGISDedMap/><GISClassCount>0</GISClassCount><DDOCode>2201005648</DDOCode><DetailHead>34</DetailHead><SchemeCode>22250403</SchemeCode><FormId>MTR45</FormId><GrossAmount>2050</GrossAmount><TotalDeduction>0</TotalDeduction><PayMonth>02</PayMonth><PaymentMode>CMP</PaymentMode><TotalGISDeduction>0</TotalGISDeduction><PayeeType>D</PayeeType><PaymentFile>Y</PaymentFile></collection>";

                               // string[] InputArr = { Convert.ToString(2024), Convert.ToString(2025), "2201005648", "22250403", "34" };

                              //  var chkbalres = await checkBalance.getBalanceAsync(InputArr);


                                //  var slipdata =  CommonFunc.GetAuthorizationSlip(data.xmlString);
                                if (response?.@return[0] != null)
                                {

                                        var item = response?.@return[0];


                                        mm.AuthNo = item?.authNO;
                                        mm.StatusCode = item?.statusCode;
                                        mm.DDOCode = item?.ddoCode;
                                        mm.BatchID = request.model.BtachId;
                                        mm.CreatedBy = request.model.UserId;
                                        mm.BeamsBillRequestID = result;
                                        mm.MTR_Year2 = Convert.ToInt32(item?.budgetYear2);
                                        mm.MTR_Year1 = Convert.ToInt32(item?.budgetYear1);
                                        mm.MTR_expTotal = item?.expTotal;
                                        mm.MTR_TotalBudget = item?.totalBudget;
                                        mm.MTR_TransNo = item.transNo;
                                        mm.MTR_ValidTo = item.validTo;
                                        if (item.authPdf is not null)
                                        {
                                            mm.BillPDF = item.authPdf;
                                            mm.base64 = Convert.ToBase64String(item.authPdf);
                                        }
                                    //string.Join("$", item.additionalFields.ToArray());
                                      mm.MTR_additionalFields = "$2201|PUNE$W|HIGHER AND TECHNICAL EDUCATION AND EMPLOYMENT DEPT|W-02$2202|GENERAL EDUCATION$800|Other Expenditure.$09|VIIIA1-FREESHIP TO STUDT P.INC.NOT EXD 15000 PA$31|GRANT IN AID (NON SALARY)$01|SALARY$22021456|Freeship to students whose or whose parents Income does not exceed Rs. 6 lakh per annum$DIRECTOR DIRECTORATE OF HIGHER EDUCATION PUNE$PNED00403E$SBIN0000454$STATE BANK OF INDIA$MAIN BRANCH, PUNE$37345850623$ASST.DIR ACCOUNTS M.F.A.S.GR-1 JR .DIR.OF EDUCATIO";



                                       
                                        
                                    

                                    if (mm.StatusCode is not null && mm.StatusCode == "00")
                                    {
                                        IsInserted = await _sender.Send(new InsertBEAMSXMLBillGenerationResponseDetailsCommand(mm));
                                      
                                        if (IsInserted > 0)
                                        {
                                            var m = new InsertMTRCommandModel();
                                            m.PaybillId = data.PayBillId;
                                            m.PayYear = Convert.ToInt32(data.PayYear);
                                            m.PayMonth = data.PayMonth;
                                            m.BeneficiaryCount = Convert.ToInt32(data.BeneficiaryCount);
                                            m.GrossAmount = data.GrossAmount;
                                            m.FinYear1 = Convert.ToInt32(data.FinYear1);
                                            m.FinYear2 = Convert.ToInt32(data.FinYear2);
                                            m.FormId = data.FormID;
                                            m.PaymentMode = data.PaymentMode;
                                            m.BillType = data.BillType;
                                            m.PayeeCount = Convert.ToInt32(data.PayeeCount);
                                            m.BulkFlag = data.BulkFlag;
                                            m.BillCreationDate = data.BillCreationDate.ToString();
                                            m.MTRBillPortalName = data.MTRBillPortalName;
                                            m.DDOCode = data.DDOCode;
                                            m.DetailHead = data.DetailHead;
                                            m.SchemeCode = data.SchemeCode;
                                            m.TotalDeduction = data.TotalDeduction;
                                            m.PayeeType = data.PayeeType;
                                            m.CreatedBy = request.model.UserId;
                                            m.BeamsBillRequestID = result;
                                            m.IPAddress = request.model.IpAddress;
                                            m.XMLString = data.xmlString;
                                      if (await _sender.Send(new InsertMTR45DetailsCommand(m)) > 0)
                                            {
                                                res.status = true;
                                                res.message = "Your bill successfully submitted to beam.";

                                            }
                                            else
                                            {
                                                res.status = false;
                                                res.message = "Internal server error . please try again later.";
                                            }
                                        }else
                                        {
                                            res.status = false;
                                            res.message = "Internal server error . please try again later.";

                                        }
                                    }
                                    else
                                    {
                                        res.status = false;

                                        if (mm.StatusCode.Contains("|"))
                                        {
                                            string[] StatusCodeList = mm.StatusCode.Split('|');
                                            foreach (string code in StatusCodeList)
                                            {
                                                if (!string.IsNullOrEmpty(code))
                                                    res.message = res.message + "," + await _sender.Send(new GetStatusDescription(code));
                                            }
                                        }
                                        else
                                        {
                                            res.message = await _sender.Send(new GetStatusDescription(mm.StatusCode));
                                        }





                                    }

                                }
                                else
                                {
                                    res.status = false;
                                    res.message = "Internal server error . please try again later.";


                                }
                            }
                            else
                            {
                                res.status = false;
                                res.message = "Internal server error . please try again later.";
                            }
                        }

                    }
                    else
                    {
                        res.status = false;
                        res.message = "Internal server error . please try again later.";
                    }
                    return res;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public static string GenerateXMLString(GenerateXMLResponseModel data)
        {
            string xmlString = string.Empty;
            if (data != null)
            {
                xmlString = "<collection><BillType>" + data.BillType + "</BillType><FinYear2>" + data.FinYear2 + "</FinYear2><FinYear1>" + data.FinYear1 + "</FinYear1><BeneficiaryCount>" + data.BeneficiaryCount + "</BeneficiaryCount><PayeeCount>" + data.PayeeCount + "</PayeeCount><BulkFlag>" + data.BulkFlag + "</BulkFlag><BillCreationDate>" + data.BillCreationDate.ToString("dd-MM-yyyy") + "</BillCreationDate><PayYear>" + data.PayYear + "</PayYear><BillPortalName>" + data.BillPortalName + "</BillPortalName><PaybillId>" + data.PayBillId + "</PaybillId><BifurcatedGISDedMap/><GISClassCount>0</GISClassCount><DDOCode>" + data.DDOCode + "</DDOCode><DetailHead>" + data.DetailHead + "</DetailHead><SchemeCode>" + data.SchemeCode + "</SchemeCode><FormId>" + data.FormID + "</FormId><GrossAmount>" + data.GrossAmount + "</GrossAmount><TotalDeduction>" + data.TotalDeduction + "</TotalDeduction><PayMonth>" + data.PayMonth + "</PayMonth><PaymentMode>" + data.PaymentMode + "</PaymentMode><TotalGISDeduction>0</TotalGISDeduction><BifurcatedBillPayeeMap><BifurcatedPayeeDetailsMap><RegisteredPayee>Y</RegisteredPayee><PayeeName>" + data.PayeeName + "</PayeeName><PayeeCode>" + data.PayeeCode + "</PayeeCode><PayeeAmount>" + data.GrossAmount + "</PayeeAmount><BankIFSC>" + data.IFSCCode + "</BankIFSC><AccountNo>" + data.AccountNumber + "</AccountNo></BifurcatedPayeeDetailsMap></BifurcatedBillPayeeMap><SNAPayment>" + data.SNAPayment + "</SNAPayment></collection>";
            }
            return xmlString;
        }
    }


}














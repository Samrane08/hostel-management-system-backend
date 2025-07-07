
using BeamsCheckBalance;

namespace DDO_Service.API.Features.GetSchemes
{

    public record CheckDDO_BalanceQuery(CheckDDO_BalanceQueryRequestModel model) : IQuery<CheckDDO_BalanceModel>;
    public record CheckDDO_BalanceResult(int statusCode, bool status, string message, CheckDDO_BalanceModel Data);
    public record CheckDDO_BalanceErrorResponse(int statusCode, bool status, string message);

    public class CheckDDO_BalanceQueryValidator : AbstractValidator<CheckDDO_BalanceQuery>
    {
        public CheckDDO_BalanceQueryValidator()
        {
            RuleFor(x => x.model.Schemecode).NotEmpty().WithMessage("Scheme code can't be empty.");
            RuleFor(x => x.model.DDOCode).NotEmpty().WithMessage("DDOCode can't be empty.");

        }

    }
    public class InsertBatchWiseBillGeneration : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/ddo/check-balance",
                async (CheckDDO_BalanceQueryRequestModel req, ISender sender, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
                {
                    try
                    {
                      
                        var RS = await sender.Send(new GetFinancialYearQueryBySchemeId(Convert.ToInt32(req.financialYearId)));
                        if (!string.IsNullOrEmpty(RS))
                        {
                         
                            req.Year1 = Convert.ToInt32(RS.Split(":")[0]);
                            req.Year2 = Convert.ToInt32(RS.Split(":")[1]);
                            req.SchemeDDOMapID = 0;
                            req.DDOCode = req.DDOCode;
                            req.Schemecode = req.Schemecode;
                            req.DetailsHead = claimsService.GetdetailHead();
                            req.CreatedBy = claimsService.GetUserId();
                            var InsertCheckBalReq = await sender.Send(new CheckDDO_BalanceQueryRequest(req));
                            if (InsertCheckBalReq > 0)
                            {
                                req.CheckReqId = InsertCheckBalReq;
                                var reslt = await sender.Send(new CheckDDO_BalanceQuery(req));
                              
                                if (reslt != null) return    Results.Ok(new CheckDDO_BalanceResult(StatusCodes.Status200OK, true, "DDO balance fetched successfully", reslt));
                                else return Results.Ok(new CheckDDO_BalanceErrorResponse(StatusCodes.Status200OK, false, "Unabale to check ddo balance"));
                            }
                            else
                            {
                                return Results.Ok(new CheckDDO_BalanceErrorResponse(StatusCodes.Status200OK, false, "Unabale to check ddo balance"));
                            }
                        }
                        else
                        {
                            return Results.Ok(new CheckDDO_BalanceErrorResponse(StatusCodes.Status200OK, false, "Unabale to check ddo balance"));

                        }
                    }
                    catch (Exception)
                    {
                        return Results.BadRequest(new CheckDDO_BalanceErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("CheckDDO_Balance")
                 .RequireAuthorization();
        }
    }

    internal class CheckDDO_BalanceQueryHandler : IQueryHandler<CheckDDO_BalanceQuery, CheckDDO_BalanceModel>
    {

        private readonly IDapper _dapper;
        public CheckDDO_BalanceQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<CheckDDO_BalanceModel> Handle(CheckDDO_BalanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var chk = new CheckDDO_BalanceModel();
                chk.StatusCode = "00";
                chk.CurrentTimestamp = "2025-06-30 11:22:51";
                chk.CurrentMonthBudget = "100000";
                chk.CurrentMonthExp = "50000";
                chk.CurrentMonthBalace = "500000";
                chk.DistributedFlag = "F";
                chk.NegativeExp = "0";
                chk.TotalBudget = "100000";
                chk.TotalExp = "50000";
                chk.TotalBalance = "0"; //  main 
                chk.StateBudget = "2000000";
                chk.StateExp = "1000000";
                chk.StateBalance = "1000000";




                var MyParam = new DynamicParameters();
                MyParam.Add("p_ChkBalReqID", request.model.CheckReqId, DbType.Int32);
                MyParam.Add("p_DDOcode", request.model.DDOCode, DbType.String);
                MyParam.Add("p_Schemecode", request.model.Schemecode, DbType.String);
                MyParam.Add("p_SchemeDDOMapID", 0, DbType.Int32);
                MyParam.Add("p_StatusCode", chk.StatusCode, DbType.String);
                MyParam.Add("p_CurrTimeStamp", chk.CurrentTimestamp, DbType.String);
                MyParam.Add("p_CurrMonthBudget", chk.CurrentMonthBudget, DbType.String);
                MyParam.Add("p_CurrMonthExp", chk.CurrentMonthExp, DbType.String);
                MyParam.Add("p_CurrMonthBalance", chk.CurrentMonthBalace, DbType.String);
                MyParam.Add("p_DistributedFlag", chk.DistributedFlag, DbType.String);
                MyParam.Add("p_NegativeExp", chk.NegativeExp, DbType.String);
                MyParam.Add("p_totalBudget", chk.TotalBudget, DbType.String);
                MyParam.Add("p_totalExp", chk.TotalExp, DbType.String);
                MyParam.Add("p_totalBalance", chk.TotalBalance, DbType.String);
                MyParam.Add("p_stateBudget", chk.StateBudget, DbType.String);
                MyParam.Add("p_stateExp", chk.StateExp, DbType.String);
                MyParam.Add("p_stateBalance", chk.StateBalance, DbType.String);
                MyParam.Add("p_IsActive", true, DbType.Boolean);
                MyParam.Add("p_CreatedBy", request.model.UserId, DbType.String);
                MyParam.Add("p_UpdatedBy", request.model.UserId, DbType.String);
                var result = await Task.FromResult(_dapper.Insert<CheckDDO_BalanceModel>("InsertDDOCheckAvailableBalanceResponse", MyParam, commandType: System.Data.CommandType.StoredProcedure));
                if (result is not null)
                {
                    result.BeamsScheme_Code = request.model.Schemecode;
                    if (result.StatusCode == "0")
                    {
                        if (result.DistributedFlag == "D")
                        {
                            result.DistributedFlag = "Distributed scheme";
                        }
                        else
                        {
                            result.DistributedFlag = "Undistributed scheme";
                        }
                        if (result.NegativeExp == "Y")
                        {
                            result.NegativeExp = "Negative Expenditure is Allowed";
                        }
                        else
                        {
                            result.NegativeExp = "Negative Expenditure is Not Allowed";
                        }
                    }
                    return result;
                }
                else
                {
                    return null;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

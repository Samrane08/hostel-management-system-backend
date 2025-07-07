
namespace DDO_Service.API.Features.GetFinancialYear
{

    public record GetFinancialYearQuery(string Deptid) : IQuery<List<DropdownModel>>;
    public record GetFinancialYearResult(int statusCode, bool status, string message, List<DropdownModel> Data);
    public record GetFinancialYearErrorResponse(int statusCode, bool status, string message);
    public class GetFinancialYear : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/ddo/get-financialyear",
                async (ISender sender, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
                {
                    try
                    {

                        var deptId = claimsService.GetDeptId();
                        var reslt = await sender.Send(new GetFinancialYearQuery(deptId));
                        

                        if (reslt.Any()) return Results.Ok(new GetFinancialYearResult(StatusCodes.Status200OK, true, "financial year list", reslt));
                        else return Results.Ok(new GetFinancialYearErrorResponse(StatusCodes.Status200OK, false, "financialYear list not found"));
                    }
                    catch (Exception)
                    {
                        return Results.BadRequest(new GetFinancialYearErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("GetFinancialYear")
                 .RequireAuthorization();
        }
    }

    internal class GetFinancialYearQueryHandler : IQueryHandler<GetFinancialYearQuery, List<DropdownModel>>
    {

        private readonly IDapper _dapper;
        public GetFinancialYearQueryHandler(IDapper dapper)
        {
           _dapper = dapper;
        }
        public async Task<List<DropdownModel>> Handle(GetFinancialYearQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("P_DeptID", request.Deptid, DbType.String);
                var result = await Task.FromResult(_dapper.GetAll<DropdownModel>("usp_Getfinancialyear", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

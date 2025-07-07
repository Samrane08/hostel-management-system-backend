namespace DDO_Service.API.Features.GetSchemes
{

    public record GetSchemeQuery(string UserId,string Deptid) : IQuery<List<DropdownModel>>;
    public record GetSchemeResult(int statusCode, bool status, string message, List<DropdownModel> Data);
    public record GetSchemeErrorResponse(int statusCode, bool status, string message);
    public class GetScheme : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/ddo/get-scheme",
                async (ISender sender, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
                {
                    try
                    {
                        var userid = claimsService.GetUserId();
                        var deptId = claimsService.GetDeptId();
                        if (string.IsNullOrEmpty(userid))
                        {
                            return Results.Unauthorized();

                        }
                        var reslt = await sender.Send(new GetSchemeQuery(userid,deptId));


                        if (reslt.Any()) return Results.Ok(new GetSchemeResult(StatusCodes.Status200OK, true, "scheme list", reslt));
                        else return Results.Ok(new GetSchemeErrorResponse(StatusCodes.Status200OK, false, "scheme not found"));
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(new GetSchemeErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("GetScheme")
                .RequireAuthorization();
             
        }
    }

    internal class GetSchemeQueryHandler : IQueryHandler<GetSchemeQuery, List<DropdownModel>>
    {

        private readonly IDapper _dapper;
        public GetSchemeQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<List<DropdownModel>> Handle(GetSchemeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("P_UserID", request.UserId, DbType.String);
                myparam.Add("P_DeptID", request.Deptid, DbType.String);
                var result = await Task.FromResult(_dapper.GetAll<DropdownModel>("USP_GetSchemeList", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

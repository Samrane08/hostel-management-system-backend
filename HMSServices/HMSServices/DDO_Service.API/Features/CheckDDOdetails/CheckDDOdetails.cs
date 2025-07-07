
namespace DDO_Service.API.Features.GetSchemes
{

    public record CheckDDOdetailsQuery(CheckDDOdetailsRequestModel m) : IQuery<DDODetailsModel>;
    public record CheckDDOdetailsResult(int statusCode, bool status, string message, DDODetailsModel Data);
    public record CheckDDOdetailsErrorResponse(int statusCode, bool status, string message);

    public class CheckDDOdetailsQueryValidator : AbstractValidator<CheckDDOdetailsQuery>
    {
        public CheckDDOdetailsQueryValidator()
        {
            RuleFor(x => x.m.SchemeId)
                .NotEmpty().WithMessage("SchemeId can't be empty.")
                       .GreaterThan(0).WithMessage("SchemeId must be greater than 0.");



        }

    }
    public class CheckDDOdetails : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/ddo/details",
                async (CheckDDOdetailsRequestModel m, ISender sender, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
                {
                    try
                    {
                        m.UserId = claimsService.GetUserId();
                        m.DeptID = claimsService.GetDeptId();
                        
                        if (string.IsNullOrEmpty(m.UserId))
                        {
                            return Results.Unauthorized();

                        }
                        var reslt = await sender.Send(new CheckDDOdetailsQuery(m));

                        if (reslt != null) return Results.Ok(new CheckDDOdetailsResult(StatusCodes.Status200OK, true, "DDO balance fetched successfully", reslt));
                        else return Results.Ok(new CheckDDOdetailsErrorResponse(StatusCodes.Status200OK, false, "Unabale to check ddo balance"));
                    }
                    catch (Exception)
                    {
                        return Results.BadRequest(new CheckDDOdetailsErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("CheckDDOdetails");
              //  .RequireAuthorization();
        }
    }

    internal class CheckDDOdetailsQueryHandler : IQueryHandler<CheckDDOdetailsQuery, DDODetailsModel>
    {

        private readonly IDapper _dapper;
        public CheckDDOdetailsQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<DDODetailsModel> Handle(CheckDDOdetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_SchemeID", request.m.SchemeId, DbType.Int32);
                myparam.Add("p_UserId", request.m.UserId, DbType.String);
                myparam.Add("p_DeptID", request.m.DeptID, DbType.String);

                var result = await Task.FromResult(_dapper.Get<DDODetailsModel>("usp_GetDDODetails", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

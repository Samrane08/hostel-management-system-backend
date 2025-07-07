

namespace Grievance.API.Features.GetApplicationDetails
{

    public record GetApplicationDetailsQuery(string SearchBy) : IQuery<List<Dictionary<string, object>>>;
    public record GetApplicationDetailsResult(int statusCode, bool status, string message, List<Dictionary<string, object>> Data);
    public record GetApplicationDetailsErrorResponse(int statusCode, bool status, string message);

    public class GetApplicationDetails : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/grievance/GetApplicationDetails",
                async (string SearchBy, ISender sender) =>
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(SearchBy))
                        {
                            var query = new GetApplicationDetailsQuery(SearchBy);
                            var result = await sender.Send(query);

                            if (result != null && result.Any())
                                return Results.Ok(new GetApplicationDetailsResult(StatusCodes.Status200OK, true, "Application details found", result));
                            else
                                return Results.Ok(new GetApplicationDetailsErrorResponse(StatusCodes.Status200OK, false, "Application details not found"));
                        }
                        else
                        {
                            return Results.Ok(new GetApplicationDetailsErrorResponse(StatusCodes.Status200OK, false, "Please enter valid search parameters"));
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log exception (ex) if logging is set up
                        return Results.BadRequest(new GetApplicationDetailsErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }
                })
                .WithName("GetApplicationDetails")
                .Produces<GetApplicationDetailsResult>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Application Details")
                .WithDescription("Retrieves application details based on the provided search parameter.");
        }
    }

    internal class GetApplicationDetailsQueryHandler : IQueryHandler<GetApplicationDetailsQuery, List<Dictionary<string, object>>>
    {
        private readonly IDapper _dapper;

        public GetApplicationDetailsQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<List<Dictionary<string, object>>> Handle(GetApplicationDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("SearchBy", request.SearchBy, DbType.String);
                var result = await _dapper.GetAllAsDictionaryAsync("Usp_GetApplicationDetailsForSupport", myparam, commandType: System.Data.CommandType.StoredProcedure);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}



namespace Grievance.API.Features.GetGrievanceList
{
    public record GetGrievanceListQuery(string UserId) : IQuery<List<GetGrievanceModel>>;
    public record GetGrievanceListResult(int statusCode, bool status, string message, List<GetGrievanceModel> Data);
    public record GetGrievanceListErrorResponse(int statusCode, bool status, string message);

    public class GetGrievanceList : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/grievance/GetGrievance",
                async (string UserId, ISender sender) =>
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(UserId))
                        {
                            var query = new GetGrievanceListQuery(UserId);
                            var result = await sender.Send(query);

                            if (result != null && result.Any())
                                return Results.Ok(new GetGrievanceListResult(StatusCodes.Status200OK, true, "Grievance found", result));
                            else
                                return Results.Ok(new GetGrievanceListErrorResponse(StatusCodes.Status200OK, false, "No grievances found"));
                        }
                        else
                        {
                            return Results.BadRequest(new GetGrievanceListErrorResponse(StatusCodes.Status400BadRequest, false, "Please enter a valid UserId"));
                        }
                    }
                    catch (Exception)
                    {
                        // Log exception if logging is set up
                        return Results.BadRequest(new GetGrievanceListErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }
                })
                .WithName("GetGrievanceList")
                .Produces<GetGrievanceListResult>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Retrieve Grievance List")
                .WithDescription("Retrieves a list of grievances based on the provided UserId.");
        }
    }

    internal class GetGrievanceListQueryHandler : IQueryHandler<GetGrievanceListQuery, List<GetGrievanceModel>>
    {
        private readonly IDapper _dapper;

        public GetGrievanceListQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<List<GetGrievanceModel>> Handle(GetGrievanceListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_UserId", request.UserId, DbType.String);
                var result = await Task.FromResult(_dapper.GetAll<GetGrievanceModel>("usp_GetGrievance", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception)
            {
                // Log exception if logging is set up
                throw;
            }
        }
    }
}

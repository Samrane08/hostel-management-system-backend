

namespace Grievance.API.Features.GetGrievanceStatus
{
    public record GetGrievanceStatusQuery() : IQuery<List<DropdownModel>>;
    public record GetGrievanceStatusResult(int statusCode, bool status, string message, List<DropdownModel> Data);
    public record GetGrievanceStatusErrorResponse(int statusCode, bool status, string message);
    public class GetGrievanceStatus : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/grievance/ticket-Status",
                async (ISender sender) =>
                {
                    try
                    {

                        var reslt = await sender.Send(new GetGrievanceStatusQuery());

                        if (reslt.Any()) return Results.Ok(new GetGrievanceStatusResult(StatusCodes.Status200OK, true, "Grievance status found", reslt));
                        else return Results.Ok(new GetGrievanceStatusErrorResponse(StatusCodes.Status200OK, false, "not found"));
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(new GetGrievanceStatusErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("GetGrievanceStatus")
                .Produces<GetGrievanceStatusResult>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("GetGrievanceStatus")
                .WithDescription("GetGrievanceStatus");
        }
    }

    internal class GetGrievanceStatusQueryHandler : IQueryHandler<GetGrievanceStatusQuery, List<DropdownModel>>
    {
        private readonly IDapper _dapper;
        public GetGrievanceStatusQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<List<DropdownModel>> Handle(GetGrievanceStatusQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await Task.FromResult(_dapper.GetAll<DropdownModel>("usp_GetTicketStatus", null, commandType: System.Data.CommandType.StoredProcedure));

                return result;
            }
            catch (Exception) { throw; }
        }
    }
}

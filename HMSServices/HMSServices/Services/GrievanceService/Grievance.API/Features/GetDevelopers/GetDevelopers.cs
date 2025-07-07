

namespace Grievance.API.Features.GetDevelopers
{

    public record GetDevelopersQuery() : IQuery<List<DropdownModel>>;
    public record GetDevelopersResult(int statusCode, bool status, string message, List<DropdownModel> Data);
    public record GetDevelopersErrorResponse(int statusCode, bool status, string message);
    public class GetDevelopers : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/grievance/Developers",
                async (ISender sender) =>
                {
                    try
                    {

                        var reslt = await sender.Send(new GetDevelopersQuery());

                        if (reslt.Any()) return Results.Ok(new GetDevelopersResult(StatusCodes.Status200OK, true, "Developers found", reslt));
                        else return Results.Ok(new GetDevelopersErrorResponse(StatusCodes.Status200OK, false, "No developers found"));
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(new GetDevelopersErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("GetDevelopers")
                .Produces<GetDevelopersResult>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("GetDevelopers")
            .WithDescription("GetDevelopers");
        }
    }

    internal class GetGrievanceCategoryQueryHandler : IQueryHandler<GetDevelopersQuery, List<DropdownModel>>
    {

        private readonly IDapper _dapper;
        public GetGrievanceCategoryQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<List<DropdownModel>> Handle(GetDevelopersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await Task.FromResult(_dapper.GetAll<DropdownModel>("usp_GetDevelopers", null, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

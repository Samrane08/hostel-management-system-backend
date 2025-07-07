

namespace Grievance.API.Features.GetGrievancetype
{

    public record GetGrievancetypeQuery() : IQuery<List<DropdownModel>>;
    public record GetGrievancetypeResult(int statusCode, bool status, string message, List<DropdownModel> Data);
    public record GetGrievancetypeErrorResponse(int statusCode, bool status, string message);
    public class GetGrievancetype : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/grievance/type",
                async (ISender sender) =>
                {
                    try
                    {

                        var reslt = await sender.Send(new GetGrievancetypeQuery());

                        if (reslt.Any()) return Results.Ok(new GetGrievancetypeResult(StatusCodes.Status200OK, true, "Grievance type found", reslt));
                        else return Results.Ok(new GetGrievancetypeErrorResponse(StatusCodes.Status200OK, false, "not found"));
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(new GetGrievancetypeErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("GetGrievancetype")
                .Produces<GetGrievancetypeResult>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("GetGrievancetype")
            .WithDescription("GetGrievancetype");
        }
    }

    internal class GetGrievanceCategoryQueryHandler : IQueryHandler<GetGrievancetypeQuery, List<DropdownModel>>
    {
        private readonly IDapper _dapper;
        public GetGrievanceCategoryQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<List<DropdownModel>> Handle(GetGrievancetypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await Task.FromResult(_dapper.GetAll<DropdownModel>("usp_Grievancetype", null, commandType: System.Data.CommandType.StoredProcedure));

                return result;
            }
            catch (Exception) { throw; }
        }
    }
}


namespace Grievance.API.Features.GetGrievanceCategory
{

    public record GetGrievanceCategoryQuery() : IQuery<List<DropdownModel>>;
    public record GetGrievanceCategoryResult(int statusCode, bool status, string message, List<DropdownModel> Data);
    public record GetGrievanceCategoryErrorResponse(int statusCode, bool status, string message);
    public class GetGrievanceCategory : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/grievance/Category",
                async (ISender sender) =>
                {
                    try
                    {

                        var reslt = await sender.Send(new GetGrievanceCategoryQuery());

                        if (reslt.Any()) return Results.Ok(new GetGrievanceCategoryResult(StatusCodes.Status200OK, true, "Grievance category list", reslt));
                        else return Results.Ok(new GetGrievanceCategoryErrorResponse(StatusCodes.Status200OK, false, "not found"));
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(new GetGrievanceCategoryErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("GetGrievanceCategory")
                .Produces<GetGrievanceCategoryResult>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("GetGrievanceCategory")
            .WithDescription("GetGrievanceCategory");
        }
    }

    internal class GetGrievanceCategoryQueryHandler : IQueryHandler<GetGrievanceCategoryQuery, List<DropdownModel>>
    {
        private readonly IDapper _dapper;
        public GetGrievanceCategoryQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<List<DropdownModel>> Handle(GetGrievanceCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await Task.FromResult(_dapper.GetAll<DropdownModel>("usp_GrievanceCategory", null, commandType: System.Data.CommandType.StoredProcedure));

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

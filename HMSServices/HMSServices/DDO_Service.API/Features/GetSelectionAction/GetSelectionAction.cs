namespace DDO_Service.API.Features.GetSelectionAction
{

    public record GetSelectionActionQuery() : IQuery<List<DropdownModel>>;
    public record GetSelectionActionResult(int statusCode, bool status, string message, List<DropdownModel> Data);
    public record GetSelectionActionErrorResponse(int statusCode, bool status, string message);
    public class GetSelectionAction : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/ddo/get-selectionaction",
                async (ISender sender) =>
                {
                    try
                    {

                        var reslt = await sender.Send(new GetSelectionActionQuery());

                        if (reslt.Any()) return Results.Ok(new GetSelectionActionResult(StatusCodes.Status200OK, true, "selectionaction list", reslt));
                        else return Results.Ok(new GetSelectionActionErrorResponse(StatusCodes.Status200OK, false, "selectionaction not found"));
                    }
                    catch (Exception)
                    {
                        return Results.BadRequest(new GetSelectionActionErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("GetSelectionAction")
                 .RequireAuthorization();
               
        }
    }

    internal class GetSelectionActionQueryHandler : IQueryHandler<GetSelectionActionQuery, List<DropdownModel>>
    {

        private readonly IDapper _dapper;
        public GetSelectionActionQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<List<DropdownModel>> Handle(GetSelectionActionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await Task.FromResult(_dapper.GetAll<DropdownModel>("usp_GetActionType", null, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

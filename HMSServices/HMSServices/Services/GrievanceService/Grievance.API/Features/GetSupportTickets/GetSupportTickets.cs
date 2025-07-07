

namespace Grievance.API.Features.GetSupportTickets
{

    public record GetSupportTicketsQuery(int PageNumber, int PageSize, string OrderBy, string? SearchBy) : IQuery<List<GetGrievanceModel>>;
    public record GetSupportTicketsResult(int statusCode, bool status, string message, List<GetGrievanceModel> Data);
    public record GetSupportTicketsErrorResponse(int statusCode, bool status, string message);
    public class GetSupportTickets : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/grievance/SupportGetGrievance",
                async (int PageNumber, int PageSize, string OrderBy, string? SearchBy, ISender sender) =>
                {
                    try
                    {
                        string srcText = "";
                        if (!string.IsNullOrEmpty(SearchBy))
                        {
                            if (SearchBy.ToLower().Contains("tkt"))
                                srcText = SearchBy.Substring(3);
                            else
                                srcText = SearchBy;

                        }
                        else
                            srcText = "No";

                        var reslt = await sender.Send(new GetSupportTicketsQuery(PageNumber, PageSize, OrderBy, srcText));

                        if (reslt != null && reslt.Any()) return Results.Ok(new GetSupportTicketsResult(StatusCodes.Status200OK, true, "data found", reslt));
                        else return Results.Ok(new GetSupportTicketsErrorResponse(StatusCodes.Status200OK, false, "no data avilable at the moment"));
                    }


                    catch (Exception)
                    {
                        return Results.BadRequest(new GetSupportTicketsErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("GetSupportTickets")
                .Produces<GetSupportTicketsResult>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("GetSupportTickets")
            .WithDescription("GetSupportTickets");
        }
    }

    internal class GetGrievanceByIdQueryHandler : IQueryHandler<GetSupportTicketsQuery, List<GetGrievanceModel>>
    {
        private readonly IDapper _dapper;
        public GetGrievanceByIdQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<List<GetGrievanceModel>> Handle(GetSupportTicketsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_PageNumber", request.PageNumber, DbType.Int32);
                myparam.Add("p_PageSize", request.PageSize, DbType.Int32);
                myparam.Add("p_OrderBy", request.OrderBy, DbType.String);
                myparam.Add("p_SearchTerm", request.SearchBy, DbType.String);
                var result = await Task.FromResult(_dapper.GetAll<GetGrievanceModel>("usp_SupportGetGrievance", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception) { throw; }
        }
    }
}

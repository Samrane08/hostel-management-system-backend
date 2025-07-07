

namespace Grievance.API.Features.GetGrievanceById
{

    public record GetGrievanceByIdQuery(int Id) : IQuery<GetGrievanceByIdModel>;
    public record GetGrievanceByIdResult(int statusCode, bool status, string message, GetGrievanceByIdModel Data);
    public record GetGrievanceByIdErrorResponse(int statusCode, bool status, string message);
    public class GetGrievanceById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/grievance/GetGrievanceById/{Id:int}",
                async (int Id, ISender sender) =>
                {
                    try
                    {
                        if (Id > 0)
                        {

                            var reslt = await sender.Send(new GetGrievanceByIdQuery(Id));

                            if (reslt != null ) return Results.Ok(new GetGrievanceByIdResult(StatusCodes.Status200OK, true, "Grievance details found", reslt));
                            else return Results.Ok(new GetGrievanceByIdErrorResponse(StatusCodes.Status200OK, false, "Grievance details not found"));
                        }
                        else
                        {
                            return Results.Ok(new GetGrievanceByIdErrorResponse(StatusCodes.Status200OK, false, "Please enter valid grievance id"));
                        }
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(new GetGrievanceByIdErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("GetGrievanceById")
                .Produces<GetGrievanceByIdResult>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("GetGrievanceById")
                .WithDescription("GetGrievanceById");
        }
    }

    internal class GetGrievanceByIdQueryHandler : IQueryHandler<GetGrievanceByIdQuery, GetGrievanceByIdModel>
    {
        private readonly IDapper _dapper;
        public GetGrievanceByIdQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<GetGrievanceByIdModel> Handle(GetGrievanceByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("_Id", request.Id, DbType.Int32);
                var result = await Task.FromResult(_dapper.Get<GetGrievanceByIdModel>("usp_GetGrievanceById", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex) { throw; }
        }
    }
}

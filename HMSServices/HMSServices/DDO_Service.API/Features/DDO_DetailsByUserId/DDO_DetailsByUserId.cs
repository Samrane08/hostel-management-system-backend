namespace DDO_Service.API.Features.DDO_DetailsByUserId
{

    public record DDO_DetailsByUserIdQuery(CommonRequest m) : IQuery<DDOModel>;
    public record DDO_DetailsByUserIdResult(DDOModel Data);
    public record DDO_DetailsByUserIdErrorResponse(int statusCode, bool status, string message);
    public class DDO_DetailsByUserId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {


            app.MapPost("/ddo/ddo-by-id",
               async (CommonRequest request, ISender sender, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
               {
                   try
                   {


                       if (string.IsNullOrEmpty(request.UserId) && string.IsNullOrEmpty(claimsService.GetUserId()))
                       {
                           return Results.Unauthorized();

                       }
                       if (string.IsNullOrEmpty(request.UserId))
                           request.UserId = claimsService.GetUserId();
                       var reslt = await sender.Send(new DDO_DetailsByUserIdQuery(request));

                       if (reslt != null) return Results.Ok(new DDO_DetailsByUserIdResult(reslt));
                       else return Results.Ok(new DDO_DetailsByUserIdResult(new DDOModel()));
                   }
                   catch (Exception)
                   {
                       return Results.BadRequest(new DDO_DetailsByUserIdErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                   }

               })
               .WithName("DDO_DetailsByUserId");
              


        }
    }

    internal class CheckDDO_BalanceQueryHandler : IQueryHandler<DDO_DetailsByUserIdQuery, DDOModel>
    {

        private readonly IDapper _dapper;
        public CheckDDO_BalanceQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<DDOModel> Handle(DDO_DetailsByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {


                var myparam = new DynamicParameters();
                myparam.Add("p_UserId", request.m.UserId, DbType.String);

                var result = await Task.FromResult(_dapper.Get<DDOModel>("usp_GetDDODetailsByUserID", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

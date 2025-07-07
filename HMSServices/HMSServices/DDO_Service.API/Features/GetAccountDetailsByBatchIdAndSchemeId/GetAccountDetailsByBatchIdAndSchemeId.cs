
namespace DDO_Service.API.Features.GetAccountDetailsByBatchIdAndSchemeId;




public record GetAccountDetailsByBatchIdAndSchemeIdQuery(DebitAccountDetails m) : IQuery<DebitAccountModel>;
public record GetAccountDetailsByBatchIdAndSchemeIdResult(int statusCode, bool status, DebitAccountModel Data);
public record GetAccountDetailsByBatchIdAndSchemeIdErrorResponse(int statusCode, bool status, string message);

public class GetAccountDetailsByBatchIdAndSchemeIdQueryValidator : AbstractValidator<GetAccountDetailsByBatchIdAndSchemeIdQuery>
{
    public GetAccountDetailsByBatchIdAndSchemeIdQueryValidator()
    {
        RuleFor(x => x.m.BatchId).NotEmpty().WithMessage("BatchId can't be empty.")
            .MinimumLength(6).WithMessage("Batch Id can't be less than 6 characters");
        RuleFor(x => x.m.SchemeId).NotEmpty().WithMessage("SchemeId can't be empty.")
            .GreaterThan(0).WithMessage("Scheme Id must be greater than 0");


    }

}
public class GetAccountDetailsByBatchIdAndSchemeId : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {


        app.MapPost("/ddo/get-Scheme-account-details",
           async (DebitAccountDetails request, ISender sender, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
           {
               try
               {


                   var reslt = await sender.Send(new GetAccountDetailsByBatchIdAndSchemeIdQuery(request));

                   if (reslt != null) return Results.Ok(new GetAccountDetailsByBatchIdAndSchemeIdResult(StatusCodes.Status200OK, true, reslt));
                   else return Results.Ok(new GetAccountDetailsByBatchIdAndSchemeIdErrorResponse(StatusCodes.Status200OK, false, "Data is not found"));
               }
               catch (Exception)
               {
                   return Results.Ok(new GetAccountDetailsByBatchIdAndSchemeIdErrorResponse(StatusCodes.Status200OK, false, "Internal server error"));
               }

           })
           .WithName("GetAccountDetailsByBatchIdAndSchemeId")
           .RequireAuthorization();



    }
}

internal class GetAccountDetailsByBatchIdAndSchemeIdQueryHandler : IQueryHandler<GetAccountDetailsByBatchIdAndSchemeIdQuery, DebitAccountModel>
{

    private readonly IDapper _dapper;
    public GetAccountDetailsByBatchIdAndSchemeIdQueryHandler(IDapper dapper)
    {
        _dapper = dapper;
    }
    public async Task<DebitAccountModel> Handle(GetAccountDetailsByBatchIdAndSchemeIdQuery request, CancellationToken cancellationToken)
    {
        try
        {


            var myparam = new DynamicParameters();
            myparam.Add("p_SchemeId", request.m.SchemeId, DbType.Int32);
            myparam.Add("p_BatchID", request.m.BatchId, DbType.String);

            var result = await Task.FromResult(_dapper.Get<DebitAccountModel>("usp_GetAccountDetailsByBatchIdAndSchemeId", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }
}



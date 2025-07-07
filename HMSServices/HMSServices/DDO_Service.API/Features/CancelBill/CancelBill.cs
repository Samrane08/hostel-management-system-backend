
namespace DDO_Service.API.Features.CancelBill;

public record CancelBillCommand(CancelBillCommandModel model) : ICommand<int>;
public record CancelBillResult(int statusCode, bool status, string message);
public record CancelBillErrorResponse(int statusCode, bool status, string message);
public class CancelBillCommandValidator : AbstractValidator<CancelBillCommand>
{
    public CancelBillCommandValidator()
    {
        RuleFor(x => x.model.BatchID).NotEmpty()
            .WithMessage("BatchId must be a positive integer.").
            MinimumLength(6)
            .WithMessage("Invalid batch Id");

    }


}
public class CancelBill : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/ddo/cancel-bill",
            async (CancelBillCommandModel request, ISender sender, HttpContext context, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
            {
                try
                {
                    request.UserId = claimsService.GetUserId();
                    request.IpAddress = context.Connection.RemoteIpAddress?.ToString();
                    var reslt = await sender.Send(new CancelBillCommand(request));

                    if (reslt == 1) return Results.Ok(new CancelBillResult(StatusCodes.Status200OK, true, "CancelBill cancelled successfully"));
                    else return Results.Ok(new CancelBillErrorResponse(StatusCodes.Status200OK, false, "Unabale to CancelBill"));
                }
                catch (Exception)
                {
                    return Results.BadRequest(new CancelBillErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                }

            })
            .WithName("CancelBill")
            .RequireAuthorization();

    }
}

internal class CancelBillCommandHandler : ICommandHandler<CancelBillCommand, int>
{

    private readonly IDapper _dapper;
    public CancelBillCommandHandler(IDapper dapper)
    {
        _dapper = dapper;
    }

    public async Task<int> Handle(CancelBillCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("p_BatchID", request.model.BatchID, DbType.String);
            myparam.Add("p_UpdatedBy", request.model.UserId, DbType.String);
            myparam.Add("p_schemeId", request.model.SchemeId, DbType.Int32);
            myparam.Add("p_IpAddress", request.model.IpAddress, DbType.String);

            int Iscompleted = await Task.FromResult(_dapper.Update<int>("usp_UpdateBatchwiseBillGenerationDetails", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return Iscompleted;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}


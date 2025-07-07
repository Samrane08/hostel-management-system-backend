
namespace DDO_Service.API.Features.AuthorizationLetter;
public record AuthorizationLetterCommand(CancelBillCommandModel model) : ICommand<string>;
public record AuthorizationLetterResult(bool status, string data);
public record AuthorizationLetterErrorResponse(int statusCode, bool status, string message);
public class AuthorizationLetterValidator : AbstractValidator<AuthorizationLetterCommand>
{
    public AuthorizationLetterValidator()
    {
        RuleFor(x => x.model.BatchID).NotEmpty()
        .WithMessage("BatchId must be a positive integer.").MinimumLength(6)
        .WithMessage("Invalid batch Id");

    }


}
public class AuthorizationLetter : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/ddo/download-authorizationLetter",
            async (CancelBillCommandModel request, ISender sender) =>
            {
                try
                {

                    var reslt = await sender.Send(new AuthorizationLetterCommand(request));

                    if (!string.IsNullOrEmpty(reslt))
                    {

                        return Results.Ok(new AuthorizationLetterResult(true, reslt));
                    }
                    else
                        return Results.Ok(new AuthorizationLetterErrorResponse(StatusCodes.Status200OK, false, "Unabale to find file"));
                }
                catch (Exception)
                {
                    return Results.BadRequest(new AuthorizationLetterErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                }

            })
            .WithName("AuthorizationLetter");
        //.RequireAuthorization();

    }
}

internal class AuthorizationLetterCommandHandler : ICommandHandler<AuthorizationLetterCommand, string>
{

    private readonly IDapper _dapper;
    public AuthorizationLetterCommandHandler(IDapper dapper)
    {
        _dapper = dapper;
    }

    public async Task<string> Handle(AuthorizationLetterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("p_BatchID", request.model.BatchID, DbType.String);


            string fileData = await Task.FromResult(_dapper.Get<string>("usp_GetAuthorizationFile", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return fileData;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


}



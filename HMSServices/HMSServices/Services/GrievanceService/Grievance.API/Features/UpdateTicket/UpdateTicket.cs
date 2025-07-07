namespace Grievance.API.Features.UpdateTicket
{
    public record UpdateTicketCommand(UpdateTicketModel model) : ICommand<int>;
    public record UpdateTicketResult(int statusCode, bool status, string message, int Data);
    public record UpdateTicketErrorResponse(int statusCode, bool status, string message);
    public class UpdateTicket : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/grievance/update-ticket",
                async (UpdateTicketCommand request, ISender sender) =>
                {
                    try
                    {

                        var reslt = await sender.Send(request);

                        if (reslt > 0) return Results.Ok(new UpdateTicketResult(StatusCodes.Status200OK, true, "Ticket Updated successfully", reslt));
                        else return Results.Ok(new UpdateTicketErrorResponse(StatusCodes.Status200OK, false, "Some erroroccured while updating ticket"));
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(new UpdateTicketErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("UpdateTicket")
                .Produces<UpdateTicketResult>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("UpdateTicket")
                .WithDescription("UpdateTicket");
        }
    }

    internal class UpdateTicketCommandHandler : ICommandHandler<UpdateTicketCommand, int>
    {
        private readonly IDapper _dapper;
        public UpdateTicketCommandHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<int> Handle(UpdateTicketCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command.model != null)
                {
                    var myparam = new DynamicParameters();
                    myparam.Add("p_Id", command.model.Id, DbType.Int32);
                    myparam.Add("p_SupportStatus", command.model.SupportStatus, DbType.Int32);
                    myparam.Add("p_DeveloperId", command.model.DeveloperId, DbType.Int32);
                    myparam.Add("p_SupportRemarks", command.model.SupportRemarks, DbType.String);
                    myparam.Add("p_UpdatedBy", command.model.UserId, DbType.String);
                    myparam.Add("p_DeveloperStatus", command.model.DeveloperStatus, DbType.Int32);
                    myparam.Add("p_IsDeveloper", command.model.IsDeveloper, DbType.Boolean);
                    var result = await Task.FromResult(_dapper.Update<int>("usp_UpdateGrievance", myparam, commandType: System.Data.CommandType.StoredProcedure));
                    return result;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}


namespace Grievance.API.Features.CreateGrievance
{
    public record CreateGrievanceCommand(GrievanceModel model): ICommand<int>;
    public record CreateGrievanceResult(int statusCode,bool status,string message,int Data);
    public record GrievanceErrorResponse(int statusCode, bool status,string message);
    public class CreateGrievance : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/grievance/SaveGrievance",
                async (CreateGrievanceCommand request, ISender sender) =>
                {
                    try
                    {

                        var reslt = await sender.Send(request);

                       if (reslt > 0) return Results.Ok(new CreateGrievanceResult(StatusCodes.Status200OK, true, "Ticket created successfully , We will update you soon .....", reslt));
                        else return Results.Ok(new GrievanceErrorResponse(StatusCodes.Status200OK, false, "Can't create more than 3 Open ticket"));
                    }catch(Exception)
                    {
                        return Results.BadRequest(new GrievanceErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("CreateGrievance")
                .Produces<CreateGrievanceResult>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Grievance")
                .WithDescription("Create Grievance");
        }
    }

    internal class CreateGrievanceCommandHandler : ICommandHandler<CreateGrievanceCommand, int>
     {
        private readonly IDapper _dapper;
       public CreateGrievanceCommandHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<int> Handle(CreateGrievanceCommand command, CancellationToken cancellationToken)
         {
            try
                
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_UserId", command.model.UserId, DbType.String);
                myparam.Add("p_Name", command.model.Name, DbType.String);
                myparam.Add("p_MobileNo", command.model.MobileNo, DbType.String);
                myparam.Add("p_EmailID", command.model.EmailID, DbType.String);
                myparam.Add("p_DistrictID", command.model.DistrictID, DbType.Int32);
                myparam.Add("p_TalukaID", command.model.TalukaID, DbType.Int32);
                myparam.Add("p_CategoryID", command.model.GCategory, DbType.Int32);
                myparam.Add("p_GTypeID", command.model.GSuggestionType, DbType.Int32);
                myparam.Add("p_AcademicYearID", command.model.AcademicYear, DbType.Int32);
                myparam.Add("p_FileId", command.model.FileId, DbType.String);
                myparam.Add("p_Description", command.model.Description, DbType.String);
                myparam.Add("p_CreatedBy", command.model.UserId, DbType.String);
                myparam.Add("p_UpdatedBy", command.model.UpdatedBy, DbType.String);
                myparam.Add("p_ApplicationNo", command.model.ApplicationNo, DbType.String);
                var result = await Task.FromResult(_dapper.Insert<int>("InsertGrievance", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }



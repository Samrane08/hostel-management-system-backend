

namespace DDO_Service.API.Features.DDODSCEnrolment
{

    public record DDODSCEnrolmentCommand( DDODSCEnrolmentCommandModel model) : ICommand<int>;
    public record DDODSCEnrolmentResult(int statusCode, bool status, string message);
    public record DDODSCEnrolmentErrorResponse(int statusCode, bool status, string message);
    public class DDODSCEnrolment : ICarterModule
    {
        List<string> validExtensions = new List<string> { ".pfx", ".cert",".cer" };

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/ddo/Insert-DDODSCEnrolment",
                async (HttpRequest request, ISender sender, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
                {
                    try
                    {
                        if (!request.HasFormContentType || !request.Form.Files.Any())
                        {
                            return Results.BadRequest("Please upload valid file.");
                        }

                        var form = await request.ReadFormAsync();
                        var file = form.Files.GetFile("File");
                        var pfxFilePassword = form["PFXFilePassword"];
                        var fileExtension = Path.GetExtension(file.FileName).ToLower();


                        if (file == null || file.Length == 0 || string.IsNullOrEmpty(pfxFilePassword) || !validExtensions.Contains(fileExtension))
                        {
                            return Results.Ok(new DDODSCEnrolmentResult(StatusCodes.Status400BadRequest, false, "Invalid file upload"));
                        }

                        var result = CommonFunc.CheckWheathervalidpfxfile(file, pfxFilePassword);
                        if (result.status == 1)
                        {
                            result.UserId = claimsService.GetUserId();
                            if (string.IsNullOrEmpty(result.UserId))
                            {
                                return Results.BadRequest(new { message = "Unauthorize access" });
                            }
                            result.PFXFilePassword = pfxFilePassword;
                            var reslt = await sender.Send(new DDODSCEnrolmentCommand(result));

                            if (reslt > 0) return Results.Ok(new DDODSCEnrolmentResult(StatusCodes.Status200OK, true, "File uploaded successfully"));
                            else return Results.Ok(new DDODSCEnrolmentErrorResponse(StatusCodes.Status200OK, false, "Unable to upload file"));
                        }
                        else
                        {
                            return Results.Ok(new DDODSCEnrolmentErrorResponse(StatusCodes.Status200OK, false, result.StatusMessage));
                        }
                    }
                    catch (Exception)
                    {
                        return Results.BadRequest(new DDODSCEnrolmentErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("DDODSCEnrolment")
                .RequireAuthorization();
           
        }
    }
 
    internal class DDODSCEnrolmentCommandHandler : ICommandHandler<DDODSCEnrolmentCommand, int>
    {

        private readonly IDapper _dapper;
        public DDODSCEnrolmentCommandHandler(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<int> Handle(DDODSCEnrolmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var myparam = new DynamicParameters();
              //  myparam.Add("p_MaximumAmountofDebitTransaction", request.model.MaximumAmountofDebitTransaction, DbType.Int32);
                myparam.Add("p_PFXFilePassword", request.model.PFXFilePassword, DbType.String);
                myparam.Add("p_PFXFile", request.model.PFXFile, DbType.Binary);
                myparam.Add("p_BatchID", request.model.BatchID, DbType.Int32);
                //  myparam.Add("p_DSCStatus", "ip2423556363", DbType.String);
                myparam.Add("p_RequestFor", string.IsNullOrEmpty(request.model.RequestFor) ? "REG" : request.model.RequestFor, DbType.String);
                myparam.Add("p_CreatedBy", request.model.UserId, DbType.String);

                var data = await Task.FromResult( _dapper.Insert<int>("InsertIntoTblDDODSCEnrolment_Details", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

}

namespace DDO_Service.API.Features.DDO_DetailsByUserId;


    public record DDOPFXFileQuery(string UserId) : IQuery<List<pfxviewDDOModel>>;
    public record DDOPFXFileResult(List<pfxviewDDOModel> Data);
    public record DDOPFXFileErrorResponse(int statusCode, bool status, string message);
    public class DDOPFXFile : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {


            app.MapGet("/ddo/pfxfile-info",
               async (ISender sender, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
               {
                   try
                   {
                       string UserId = claimsService.GetUserId();

                       if (string.IsNullOrEmpty(UserId))
                       {
                           return Results.Unauthorized();

                       }
                     
                         
                       var reslt = await sender.Send(new DDOPFXFileQuery(UserId));

                       if (reslt != null) 
                           return Results.Ok(new DDOPFXFileResult(reslt));
                       else 
                           return Results.Ok(new DDOPFXFileResult(new List<pfxviewDDOModel>()));
                   }
                   catch (Exception)
                   {
                       return Results.BadRequest(new DDO_DetailsByUserIdErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                   }

               })
               .WithName("DDOPFXFile")
                  .RequireAuthorization();


    }
    }

    internal class DDOPFXFileHandler : IQueryHandler<DDOPFXFileQuery, List<pfxviewDDOModel>>
    {

        private readonly IDapper _dapper;
        public DDOPFXFileHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<List<pfxviewDDOModel>> Handle(DDOPFXFileQuery request, CancellationToken cancellationToken)
        {
            try
            {

                  List<pfxviewDDOModel> mm=new List<pfxviewDDOModel>();
                 var m =new pfxviewDDOModel();

                var myparam = new DynamicParameters();
                myparam.Add("p_UserId", request.UserId, DbType.String);
               var result = await Task.FromResult(_dapper.Get<pfxDDOModel>("new_GetPFXFileInfo", myparam, commandType: System.Data.CommandType.StoredProcedure));
            if (result != null)
            {
                m.DDOID = result.DDOID;
                m.DDO_Code = result.DDO_Code;
                m.DDO_Name = result.DDO_Name;
                m.DetailHead= result.DetailHead;
                m.pfxExpiry = CommonFunc.GetPfxFileExpiryDate(result.PFXFile, result.PFXFilePassword);
                mm.Add(m);
            }
      
                return mm;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }


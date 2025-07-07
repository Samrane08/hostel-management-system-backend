
namespace DDO_Service.API.Features.GetBeamsSchemeCode
{

    public record GetBeamsSchemeCodeQuery(int schemeId,string deptId) : IQuery<List<DropdownModel>>;
    public record GetBeamsSchemeCodeResult(int statusCode, bool status, string message, List<DropdownModel> Data);
    public record GetBeamsSchemeCodeErrorResponse(int statusCode, bool status, string message);

    public class GetBeamsSchemeCodeQueryValidator : AbstractValidator<GetBeamsSchemeCodeQuery>
    {
        public GetBeamsSchemeCodeQueryValidator()
        {
            RuleFor(x => x.schemeId).NotEmpty().WithMessage("schemeId can't be empty.")
                .GreaterThan(0).WithMessage("Scheme Id must be greater than 0");


        }

    }
    public class GetScheme : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/ddo/get-beamsschemecode/{schemeId:int}",
                async (int schemeId, ISender sender, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
                {
                    try
                    {

                        var deptId = claimsService.GetDeptId();
                        var reslt = await sender.Send(new GetBeamsSchemeCodeQuery(schemeId, deptId));

                        if (reslt.Any()) return Results.Ok(new GetBeamsSchemeCodeResult(StatusCodes.Status200OK, true, "Scheme Beams code list", reslt));
                        else return Results.Ok(new GetBeamsSchemeCodeErrorResponse(StatusCodes.Status200OK, false, "Scheme Beams code list not found"));
                    }
                    catch (Exception)
                    {
                        return Results.BadRequest(new GetBeamsSchemeCodeErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("GetBeamsSchemeCode")
                .RequireAuthorization();

        }
    }

    internal class GetInstallmentTypeQueryHandler : IQueryHandler<GetBeamsSchemeCodeQuery, List<DropdownModel>>
    {

        private readonly IDapper _dapper;
        public GetInstallmentTypeQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<List<DropdownModel>> Handle(GetBeamsSchemeCodeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_SchemeID", request.schemeId, DbType.Int32);
                myparam.Add("p_DeptId", request.deptId, DbType.String);
                var result = await Task.FromResult(_dapper.GetAll<DropdownModel>("USP_GetBEAMSSchemeCode", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

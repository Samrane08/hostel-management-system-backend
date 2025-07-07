

namespace DDO_Service.API.Features.GetFinancialYear
{

    public record GetInstallmentTypeQuery(int schemeId,String DeptId) : IQuery<List<DropdownModel>>;
    public record GetInstallmentTypeResult(int statusCode, bool status, string message, List<DropdownModel> Data);
    public record GetInstallmentTypeErrorResponse(int statusCode, bool status, string message);

    public class GetInstallmentTypeQueryValidator : AbstractValidator<GetInstallmentTypeQuery>
    {
        public GetInstallmentTypeQueryValidator()
        {
            RuleFor(x => x.schemeId).NotEmpty().WithMessage("Scheme Id can't be empty.")
                .GreaterThan(0).WithMessage("Scheme Id must be greater than 0");


        }

    }
    public class GetScheme : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/ddo/get-installmentype/{schemeId:int}",
                async (int schemeId,ISender sender, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
                {
                    try
                    {
                        var deptId = claimsService.GetDeptId();
                        var reslt = await sender.Send(new GetInstallmentTypeQuery(schemeId, deptId));

                        if (reslt.Any()) return Results.Ok(new GetInstallmentTypeResult(StatusCodes.Status200OK, true, "installment type list", reslt));
                        else return Results.Ok(new GetInstallmentTypeErrorResponse(StatusCodes.Status200OK, false, "installment type not found"));
                    }
                    catch (Exception)
                    {
                        return Results.BadRequest(new GetInstallmentTypeErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("GetInstallmentType")
                .RequireAuthorization();
        }
    }

    internal class GetInstallmentTypeQueryHandler : IQueryHandler<GetInstallmentTypeQuery, List<DropdownModel>>
    {

        private readonly IDapper _dapper;
        public GetInstallmentTypeQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<List<DropdownModel>> Handle(GetInstallmentTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_SchemeID", request.schemeId, DbType.Int32);
                myparam.Add("p_DeptId", request.DeptId, DbType.Int32);
                var result = await Task.FromResult(_dapper.GetAll<DropdownModel>("usp_GetInstallmentType", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

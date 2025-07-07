
using System.Dynamic;

namespace DDO_Service.API.Features.GetAllotmentList
{

    public record GetAllotmentListQuery(GetAllotmentListQueryModel model) : IQuery<ExpandoObject>;
    public record GetAllotmentListResult(int statusCode, bool status, string message, ExpandoObject Data);
    public record GetAllotmentListErrorResponse(int statusCode, bool status, string message);

    public class GetAllotmentListQueryValidator : AbstractValidator<GetAllotmentListQuery>
    {
        public GetAllotmentListQueryValidator()
        {
            RuleFor(x => x.model.Installment).NotEmpty().WithMessage("Installment no can't be empty.")
                .GreaterThan(0).WithMessage("Installment no must be greater than 0");
            RuleFor(x => x.model.SchemeID).NotEmpty().WithMessage("SchemeID can't be empty.")
                .GreaterThan(0).WithMessage("Scheme Id must be greater than 0");
            RuleFor(x => x.model.FinancialYearID).NotEmpty().WithMessage("Financial Year ID can't be empty.")
                .GreaterThan(0).WithMessage("Financial Year ID must be greater than 0");


        }

    }
    public class GetAllotmentList : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/ddo/get-allotmentlist",
                async (GetAllotmentListQueryModel request, ISender sender, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
                {
                    try
                    {
                        request.ddo_code = claimsService.GetDDOCode();
                        request.deptId = claimsService.GetDeptId();
                        if (!request.dynamicSearchConditions.Contains("a.ApplicationNo"))
                            request.dynamicSearchConditions = request.dynamicSearchConditions.Replace("ApplicationNo", "a.ApplicationNo");

                        var reslt = await sender.Send(new GetAllotmentListQuery(request));

                        if (reslt != null) return Results.Ok(new GetAllotmentListResult(StatusCodes.Status200OK, true, "allotment  list", reslt));
                        else return Results.Ok(new GetAllotmentListErrorResponse(StatusCodes.Status200OK, false, "allotment list not found"));

                    }
                    catch (Exception)
                    {
                        return Results.BadRequest(new GetAllotmentListErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("GetAllotmentList")
                .RequireAuthorization();
        }
    }

    internal class GetAllotmentListQueryHandler : IQueryHandler<GetAllotmentListQuery, ExpandoObject>
    {

        private readonly IDapper _dapper;
        public GetAllotmentListQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<ExpandoObject> Handle(GetAllotmentListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                dynamic dynamic = new ExpandoObject();


                var dictionaryList = new List<Dictionary<string, object>>();
                var myparam = new DynamicParameters();
                myparam.Add("p_Installment", request.model.Installment, DbType.Int32);
                myparam.Add("p_schemeID", request.model.SchemeID, DbType.Int32);
                myparam.Add("p_financialYearID", request.model.FinancialYearID, DbType.Int32);
                myparam.Add("p_pageNumber", request.model.pageNumber, DbType.Int32);
                myparam.Add("p_pageSize", request.model.pageSize, DbType.Int32);
                myparam.Add("p_searchConditions", request.model.dynamicSearchConditions, DbType.String);
                myparam.Add("p_ddoAmount", Convert.ToDecimal(request.model.ddoTotalAmount), DbType.Decimal);
                myparam.Add("p_specifiedLimit", Convert.ToDecimal(request.model.specifiedLimit), DbType.Int32);
                //  myparam.Add("p_specifiedLimit", Convert.ToDecimal(request.model.specifiedLimit), DbType.Int32);
                myparam.Add("p_DeptId", request.model.deptId, DbType.String);
                myparam.Add("p_ddoCode", request.model.ddo_code, DbType.String);
                var result = await _dapper.MultiResult("usp_GetAllotedApplications_usingTemp", myparam, commandType: System.Data.CommandType.StoredProcedure);
                if (result.Count > 0)
                {
                    Console.WriteLine(result[0]);
                    if (result[0] != null )
                    {
                        var data = JsonConvert.SerializeObject(result[0].FirstOrDefault());
                        dynamic.tblConfig = JsonConvert.DeserializeObject<TableUtilityModel>(data);
                    }
                    if (result[1] != null)
                    {

                        foreach (var row in result[1])
                        {
                            var dict = (IDictionary<string, object>)row;
                            dictionaryList.Add(new Dictionary<string, object>(dict));
                        }
                        dynamic.allotmenttbl = dictionaryList;
                    }
                }
                return dynamic;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

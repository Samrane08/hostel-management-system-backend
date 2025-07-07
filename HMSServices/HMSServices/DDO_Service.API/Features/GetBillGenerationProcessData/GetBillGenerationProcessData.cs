
using System.Dynamic;

namespace DDO_Service.API.Features.GetBillGenerationProcessData;


public record GetBillGenerationProcessDataQuery(GetBillGenerationProcessDataQueryModel model) : IQuery<ExpandoObject>;
public record GetBillGenerationProcessDataResult(int statusCode, bool status, string message, ExpandoObject data);
public record GetBillGenerationProcessDataErrorResponse(int statusCode, bool status, string message);

public class GetBillGenerationProcessDataQueryValidator : AbstractValidator<GetBillGenerationProcessDataQuery>
{
    public GetBillGenerationProcessDataQueryValidator()
    {
        RuleFor(x => x.model.p_SchemeID).NotEmpty().WithMessage("Scheme Id can't be empty.")
            .GreaterThan(0).WithMessage("Scheme Id must be greater than 0");
        RuleFor(x => x.model.p_InstallmentFlag).NotEmpty().WithMessage("Installment no can't be empty.")
            .GreaterThan(0).WithMessage("Installment no must be greater than 0");
        RuleFor(x => x.model.p_financialYearID).NotEmpty().WithMessage("Financial year Id can't be empty.")
            .GreaterThan(0).WithMessage("Financial year Id must be greater than 0");
       

    }

}
public class GetBillGenerationProcessData : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/ddo/process-bill",
            async (GetBillGenerationProcessDataQueryModel request, ISender sender, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
            {
                try
                {
                    request.UserId=claimsService.GetUserId();
                    request.DDO_Code=claimsService.GetDDOCode();
                    if (!string.IsNullOrEmpty(request.DDO_Code) && !string.IsNullOrEmpty(request.UserId))
                    {
                        var reslt = await sender.Send(new GetBillGenerationProcessDataQuery(request));

                        if (reslt.Any()) return Results.Ok(new GetBillGenerationProcessDataResult(StatusCodes.Status200OK, true, "fetched bill-generation-data  successfully", reslt));
                        else return Results.Ok(new GetBillGenerationProcessDataErrorResponse(StatusCodes.Status200OK, false, "No bill-generation-data found"));
                    }
                    else
                    {
                        return Results.Unauthorized();
                    }
                }
                catch (Exception)
                {
                    return Results.BadRequest(new GetBillGenerationProcessDataErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                }

            })
            .WithName("GetBillGenerationProcessData")
           .RequireAuthorization();
    }
}

internal class GetBillGenerationProcessDataQueryHandler : IQueryHandler<GetBillGenerationProcessDataQuery, ExpandoObject>
{

    private readonly IDapper _dapper;
    public GetBillGenerationProcessDataQueryHandler(IDapper dapper)
    {
        _dapper = dapper;
    }

    public async Task<ExpandoObject> Handle(GetBillGenerationProcessDataQuery request, CancellationToken cancellationToken)
    {
        try
        {
            dynamic dynamic = new ExpandoObject();
            var dictionaryList = new List<Dictionary<string, object>>();
        
                var MyParam = new DynamicParameters();
                MyParam.Add("p_SchemeID", request.model.p_SchemeID, DbType.Int32);
                MyParam.Add("p_DDOCode", request.model.DDO_Code, DbType.String);
                MyParam.Add("p_InstallmentFlag", request.model.p_InstallmentFlag, DbType.Int32);
                MyParam.Add("p_financialYearID", request.model.p_financialYearID, DbType.Int32);
                MyParam.Add("searchTerm", request.model.searchTerm, DbType.String);
                MyParam.Add("p_Page", request.model.pazeNumber, DbType.Int32);
                MyParam.Add("p_PageSize", request.model.pazeSize, DbType.Int32);
                var result = await _dapper.MultiResult("USP_GetBillGenerationProcessData", MyParam, commandType: System.Data.CommandType.StoredProcedure);// await Task.FromResult(_dapper.GetAll<GetBillGenerationProcessDataResponseModel>("USP_GetBillGenerationProcessData", MyParam, commandType: System.Data.CommandType.StoredProcedure));
                if (result.Count > 0)
                {
                    if (result[0] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[0].FirstOrDefault());
                        dynamic.tblConfig = JsonConvert.DeserializeObject<TableFeatureModel>(data);
                    }
                    if (result[1] != null)
                    {

                        foreach (var row in result[1])
                        {
                            var dict = (IDictionary<string, object>)row;
                            dictionaryList.Add(new Dictionary<string, object>(dict));
                        }
                        dynamic.billgenerationdata = dictionaryList;
                    }
                }
                return dynamic;
           
        }
        catch(Exception)
        {
            throw; 
        
        }
    }
}



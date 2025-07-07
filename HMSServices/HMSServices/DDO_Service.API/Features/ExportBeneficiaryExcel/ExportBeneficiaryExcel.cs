
namespace DDO_Service.API.Features.ExportBeneficiaryExcel;

public record ExportBeneficiaryExcelQuery(ExportBeneficiaryExcelQueryModel model) : IQuery<List<Dictionary<string, object>>>;
public record ExportBeneficiaryExcelResult(int statusCode, bool status, List<Dictionary<string, object>> Data);
public record ExportBeneficiaryExcelErrorResponse(int statusCode, bool status, string message);

public class ExportBeneficiaryExcelQueryValidator : AbstractValidator<ExportBeneficiaryExcelQuery>
{
    public ExportBeneficiaryExcelQueryValidator()
    {
        RuleFor(x => x.model.BatchID).NotEmpty().WithMessage("Scheme Id can't be empty.")
            .MinimumLength(6).WithMessage("Batch Id can not be leass than 6 character");
        RuleFor(x => x.model.SchemeId).NotEmpty().WithMessage("Scheme Id can't be empty.")
            .GreaterThan(0).WithMessage("Scheme Id must be greater than 0");



    }

}


public class ExportBeneficiaryExcel : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/ddo/export-beneficiary-excel",
            async (ExportBeneficiaryExcelQueryModel request, ISender sender, HttpContext context) =>
            {

                try
                {

                    var data = await sender.Send(new ExportBeneficiaryExcelQuery(request));


                    if (data.Count > 0)
                    {

                        return Results.Ok(new ExportBeneficiaryExcelResult(StatusCodes.Status200OK, true, data));
                    }
                    else
                    {
                        return Results.Ok(new ExportBeneficiaryExcelErrorResponse(StatusCodes.Status200OK, false, "No excel data found for this batch id"));
                    }
                }
                catch (Exception)

                {
                    return Results.BadRequest(new ExportBeneficiaryExcelErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                }
            })
        .WithName("ExportBeneficiaryExcel")
        .RequireAuthorization();
    }

    internal class ExportBeneficiaryExcelQueryHandler : IQueryHandler<ExportBeneficiaryExcelQuery, List<Dictionary<string, object>>>
    {

        private readonly IDapper _dapper;
        public ExportBeneficiaryExcelQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<List<Dictionary<string, object>>> Handle(ExportBeneficiaryExcelQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_BatchID", request.model.BatchID, DbType.String);
                myparam.Add("p_SchemeId", request.model.SchemeId, DbType.Int32);
                var data = await _dapper.GetAllAsDictionaryAsync("usp_GetBenefeciaryDetailsByBatchID", myparam, commandType: System.Data.CommandType.StoredProcedure);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}




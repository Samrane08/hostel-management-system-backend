
namespace DDO_Service.API.Features.InsertBatchWiseBillGeneration;

public record InsertBatchWiseBillGenerationCommand(InsertBatchWiseBillGenerationCommandModel model) : IQuery<int>;
public record InsertBatchWiseBillGenerationResult(int statusCode, bool status, string message);
public record InsertBatchWiseBillGenerationErrorResponse(int statusCode, bool status, string message);

public class InsertBatchWiseBillGenerationCommandValidator : AbstractValidator<InsertBatchWiseBillGenerationCommand>
{
    public InsertBatchWiseBillGenerationCommandValidator()
    {
        RuleFor(x => x.model.SchemeID).NotEmpty().WithMessage("Scheme Id can't be empty.")
            .GreaterThan(0).WithMessage("Scheme Id must be greater than 0");
        RuleFor(x => x.model.DDLDDOACtionsValue).NotEmpty()
            .WithMessage("DDLDDOACtionsValue can't be empty.");
        RuleFor(x => x.model.InstallmentFlag).NotEmpty().WithMessage("InstallmentFlag can't be empty.")
          .GreaterThan(0).WithMessage("InstallmentFlag must be greater than 0");
        RuleFor(x => x.model.FinancialYearID).NotEmpty().WithMessage("FinancialYearID can't be empty.")
          .GreaterThan(0).WithMessage("FinancialYearID must be greater than 0");
        RuleFor(x => x.model.DDOCode).NotEmpty().WithMessage("DDO Code can't be empty.");
        RuleFor(x => x.model.SchemeCode).NotEmpty().WithMessage("SchemeCode can't be empty.");
        RuleFor(x => x.model.TotalNoBefAllocatedCnt).NotEmpty().WithMessage("TotalNoBefAllocatedCnt can't be empty.")
            .GreaterThan(0).WithMessage("TotalNoBefAllocatedCnt must be greater than 0");
        RuleFor(x => x.model.AllocatedAmount).NotEmpty().WithMessage("AllocatedAmount can't be empty.")
     .GreaterThan(0).WithMessage("AllocatedAmount must be greater than 0");
        RuleFor(x => x.model.OfficePaymentNumber).NotEmpty()
            .WithMessage("OfficePaymentNumber can't be empty.").MinimumLength(3).WithMessage("OfficePaymentNumber must be greater than 3 characters");


    }

}


public class InsertBatchWiseBillGeneration : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/ddo/insert-batchwise-bill",
            async (InsertBatchWiseBillGenerationCommandModel request, ISender sender, HttpContext context, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
            {
                try
                {
                    request.IPAddress= context.Connection.RemoteIpAddress?.ToString();
                   var rs= await sender.Send(new GetFinancialYearQueryBySchemeId(request.FinancialYearID));
                    request.Year1 = Convert.ToInt32(rs.Split(":")[0]);
                    request.Year2 = Convert.ToInt32(rs.Split(":")[1]);
                    request.UserId = claimsService.GetUserId();
                    if (request.Year1 > 0 && request.Year2 > 0 && !string.IsNullOrEmpty(request.UserId) && claimsService.GetDDOCode() == request.DDOCode)
                    {
                        var reslt = await sender.Send(new InsertBatchWiseBillGenerationCommand(request));

                        if (reslt > 0) return Results.Ok(new InsertBatchWiseBillGenerationResult(StatusCodes.Status200OK, true, "insert-batchwise-bill  successfully"));
                        else return Results.Ok(new InsertBatchWiseBillGenerationErrorResponse(StatusCodes.Status200OK, false, "Unabale to insert insert-batchwise-bill"));
                    }
                    else
                    {
                        return Results.Ok(new InsertBatchWiseBillGenerationErrorResponse(StatusCodes.Status200OK, false, "Unabale to insert insert-batchwise-bill"));
                    }
                }
                catch (Exception)
                {
                    return Results.Ok(new InsertBatchWiseBillGenerationErrorResponse(StatusCodes.Status200OK, false, "Internal server error,Please try again later"));
                }

            })
            .WithName("InsertBatchWiseBillGeneration")
            .RequireAuthorization();
          
    }
}

internal class InsertBatchWiseBillGenerationCommandHandler : IQueryHandler<InsertBatchWiseBillGenerationCommand, int>
{

    private readonly IDapper _dapper;
    public InsertBatchWiseBillGenerationCommandHandler(IDapper dapper)
    {
        _dapper = dapper;
    }
    public async Task<int> Handle(InsertBatchWiseBillGenerationCommand request, CancellationToken cancellationToken)
    {
        try
        {
           

            var MyParam = new DynamicParameters();
            MyParam.Add("p_SchemeID", request.model.SchemeID, DbType.Int32);
            MyParam.Add("p_DDOCode", request.model.DDOCode, DbType.String);
            MyParam.Add("p_BeamsSchemeCode", request.model.SchemeCode, DbType.String);
            MyParam.Add("p_BeamsSubSchemeCode", null, DbType.String);
            MyParam.Add("p_TotalNoBefAllocatedCnt", request.model.TotalNoBefAllocatedCnt, DbType.Int32);
            MyParam.Add("p_AllocatedAmount", request.model.AllocatedAmount, DbType.Decimal);
            MyParam.Add("p_BatchID", CommonFunc.GenerateBatchID(), DbType.String);
            MyParam.Add("p_FinYear2", request.model.Year2, DbType.Int32);
            MyParam.Add("p_FinYear1", request.model.Year1, DbType.Int32);
            MyParam.Add("p_PayYear", DateTime.Now.Year, DbType.Int32);
            MyParam.Add("p_PayMonth", DateTime.Now.Month, DbType.Int32);
            MyParam.Add("p_CreatedBy", request.model.UserId, DbType.String);
            MyParam.Add("p_OfficePaymentNumber", request.model.OfficePaymentNumber, DbType.String);
            MyParam.Add("p_InstallmentFlag", request.model.InstallmentFlag, DbType.Int32);
            MyParam.Add("p_FinancialYearID", request.model.FinancialYearID, DbType.Int32);
            MyParam.Add("p_IPAddress", request.model.IPAddress, DbType.String);
            MyParam.Add("p_BillGenerationIPAddress",request.model.IPAddress, DbType.String);
            MyParam.Add("p_BillSubmitIPAddress", request.model.IPAddress, DbType.String);
            MyParam.Add("p_IsProcessed", 0, DbType.Boolean);
            MyParam.Add("p_IsCancel", 0, DbType.Boolean);
            MyParam.Add("p_IsActive", 1, DbType.Boolean);
            MyParam.Add("p_TreasuryTokenNo", null, DbType.String);
            MyParam.Add("p_Installment", request.model.InstallmentFlag, DbType.Int32);
            MyParam.Add("p_Installment", request.model.InstallmentFlag, DbType.Int32);
            MyParam.Add("p_IsOtherFeeWithOut", 0, DbType.Boolean);
            MyParam.Add("p_BillType", request.model.DDLDDOACtionsValue == "2" ? "DDOAccount" : "BEAMS", DbType.String);
            var result = await Task.FromResult(_dapper.Insert<int>("usp_Insert_BEAMSBatchWiseBillGeneration_Details", MyParam, commandType: System.Data.CommandType.StoredProcedure));
            return result;

        }
        catch (Exception ex)
        {
            throw;
        }
    }

}



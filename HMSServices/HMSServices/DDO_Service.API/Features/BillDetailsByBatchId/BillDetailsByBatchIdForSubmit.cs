
namespace DDO_Service.API.Features.BillDetailsByBatchId
{

    public record BillDetailsByBatchIdForSubmitQuery(string BatchId) : IQuery<BillDetailsModel>;
    public record BillDetailsByBatchIdForSubmitResult(int statusCode, bool status, string message, BillDetailsModel Data);
    public record BillDetailsByBatchIdForSubmitErrorResponse(int statusCode, bool status, string message);
    public class BillDetailsByBatchIdForSubmitQueryValidator : AbstractValidator<BillDetailsByBatchIdForSubmitQuery>
    {
        public BillDetailsByBatchIdForSubmitQueryValidator()
        {
            RuleFor(x => x.BatchId).NotEmpty().WithMessage("BatchId can not be empty.")
                .MinimumLength(6)
            .WithMessage("Invalid batch Id");

        }


    }
    public class BillDetailsByBatchIdForSubmit : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/ddo/get-bill-details-by-batchId-forSubmit",
                async (string BatchId, ISender sender, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
                {
                    try
                    {

                        var reslt = await sender.Send(new BillDetailsByBatchIdForSubmitQuery(BatchId));
                        var resultdata = reslt.Adapt<BillDetailsModel>();
                        resultdata.ddo_code = claimsService.GetDDOCode();

                        if (reslt != null) return Results.Ok(new BillDetailsByBatchIdForSubmitResult(StatusCodes.Status200OK, true, "Bill details fetched successfully", resultdata));
                        else return Results.Ok(new BillDetailsByBatchIdForSubmitErrorResponse(StatusCodes.Status200OK, false, "Unabale to bill details  successfully"));
                    }
                    catch (Exception)
                    {
                        return Results.BadRequest(new BillDetailsByBatchIdForSubmitErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("BillDetailsByBatchIdForSubmitQuery")
               .RequireAuthorization();
        }
    }

    internal class BillDetailsByBatchIdForSubmitQueryHandler : IQueryHandler<BillDetailsByBatchIdForSubmitQuery, BillDetailsModel>
    {

        private readonly IDapper _dapper;
        public BillDetailsByBatchIdForSubmitQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<BillDetailsModel> Handle(BillDetailsByBatchIdForSubmitQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_BatchID", request.BatchId, DbType.String);
                var result = await Task.FromResult(_dapper.Get<BillDetailsModel>("USP_GetBillDetailsByBatchId", myparam, commandType: System.Data.CommandType.StoredProcedure));

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

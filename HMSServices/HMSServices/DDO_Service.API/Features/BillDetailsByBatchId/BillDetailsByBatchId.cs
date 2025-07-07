using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace DDO_Service.API.Features.BillDetailsByBatchId
{

    public record BillDetailsByBatchIdQuery(string BatchId) : IQuery<BillDetailsModel>;
    public record BillDetailsByBatchIdResult(int statusCode, bool status, string message, BillDetailsModel Data);
    public record BillDetailsByBatchIdErrorResponse(int statusCode, bool status, string message);
    public class BillDetailsByBatchIdCommandValidator : AbstractValidator<BillDetailsByBatchIdQuery>
    {
        public BillDetailsByBatchIdCommandValidator()
        {
            RuleFor(x => x.BatchId).NotEmpty().WithMessage("BatchId can not be empty.");

        }


    }
    public class BillDetailsByBatchId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/ddo/get-bill-details-by-batchId",
                async (string BatchId, ISender sender, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
                {
                    try
                    {

                        var reslt = await sender.Send(new BillDetailsByBatchIdQuery(BatchId));
                        var resultdata = reslt.Adapt<BillDetailsModel>();
                        resultdata.ddo_code = claimsService.GetDDOCode();

                        if (reslt != null) return Results.Ok(new BillDetailsByBatchIdResult(StatusCodes.Status200OK, true, "Bill details fetched successfully", resultdata));
                        else return Results.Ok(new BillDetailsByBatchIdErrorResponse(StatusCodes.Status200OK, false, "Unabale to bill details  successfully"));
                    }
                    catch (Exception)
                    {
                        return Results.BadRequest(new BillDetailsByBatchIdErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("BillDetailsByBatchId")
               .RequireAuthorization();
        }
    }

    internal class BillDetailsByBatchIdQueryHandler : IQueryHandler<BillDetailsByBatchIdQuery, BillDetailsModel>
    {

        private readonly IDapper _dapper;
        public BillDetailsByBatchIdQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<BillDetailsModel> Handle(BillDetailsByBatchIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_BatchID", request.BatchId, DbType.String);
                var result = await Task.FromResult(_dapper.Get<BillDetailsModel>("USP_GetBillDetailsByBatchId_test", myparam, commandType: System.Data.CommandType.StoredProcedure));
                if (result is not null && result.MTR_additionalFields is not null)
                {
                    var additionalFields = result.MTR_additionalFields.Split('$');
                    if (additionalFields.Length > 0)
                    {
                        if (additionalFields.Length > 1)
                            result.MTR_TreasuryName = additionalFields[1];

                        if (additionalFields.Length > 2)
                        {
                            var Arr = additionalFields[2].Split('|');
                            if (Arr.Length > 2)
                                result.MTR_DemandNo = Arr[2];

                            if (Arr.Length > 1)
                                result.MTR_AdminDept = Arr[0] + "|" + Arr[1];
                        }

                        if (additionalFields.Length > 3)
                            result.MTR_MajorHead = additionalFields[3];

                        if (additionalFields.Length > 4)
                            result.MTR_MinorHead = additionalFields[4];

                        if (additionalFields.Length > 5)
                            result.MTR_subhead = additionalFields[5];

                        if (additionalFields.Length > 6)
                            result.MTR_DetailHead = additionalFields[6];

                        if (additionalFields.Length > 7)
                            result.MTR_SubDetailHead = additionalFields[7];

                        if (additionalFields.Length > 8)
                            result.MTR_SchemeCode = additionalFields[8];

                        if (additionalFields.Length > 9)
                        {
                            result.MTR_AccountHolder = additionalFields[10];
                            var additionalFieldsSubFields = additionalFields[9].Split('|');
                            if (additionalFieldsSubFields.Length > 0)
                                result.MTR_AccountHolder = additionalFieldsSubFields[0];

                            if (additionalFieldsSubFields.Length > 1)
                                result.MTR_PanNo = additionalFieldsSubFields[1];

                            if (additionalFieldsSubFields.Length > 2)
                                result.MTR_IFSCCode = additionalFieldsSubFields[2];

                            if (additionalFieldsSubFields.Length > 3)
                                result.MTR_BankName = additionalFieldsSubFields[3];

                            if (additionalFieldsSubFields.Length > 4)
                                result.MTR_BankBranchName = additionalFieldsSubFields[4];

                            if (additionalFieldsSubFields.Length > 5)
                                result.MTR_AccountNo = additionalFieldsSubFields[5];
                        }
                    }

                }
                result.AllocatedAmountInWords = CommonFunc.ConvertAmountToWords(Convert.ToDecimal(result.AllocatedAmount));
                result.MTR_InwordsExtraOneRuppes = CommonFunc.ConvertAmountToWords(Convert.ToDecimal(result.AllocatedAmount) + 1);
                result.PayMonthWord = BuildingBlocks.Utility.Utility.GetMonthName(Convert.ToInt32(result.PayMonth), result.PayYear);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

using DDO_Service.API.Features.BillDetailsByBatchId;




namespace DDO_Service.API.Features.AuthorizationLetter;
public record DownloadMTRQuery(CancelBillCommandModel model) : IQuery<string>;
public record DownloadMTRResult(bool status, string data);
public record DownloadMTRErrorResponse(int statusCode, bool status, string message);
public class DownloadMTRValidator : AbstractValidator<DownloadMTRQuery>
{
    public DownloadMTRValidator()
    {
        RuleFor(x => x.model.BatchID).NotEmpty()
        .WithMessage("BatchId must be a positive integer.").MinimumLength(6)
        .WithMessage("Invalid batch Id");

    }


}
public class DownloadMTR : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/ddo/download-mtr",
            async (CancelBillCommandModel request, ISender sender) =>
            {
                try
                {

                    var reslt = await sender.Send(new DownloadMTRQuery(request));

                    if (!string.IsNullOrEmpty(reslt))
                    {
                        var billdetails = await sender.Send(new BillDetailsByBatchIdQuery(request.BatchID));
                        var billdetailsmodel = billdetails.Adapt<BillDetailsModel>();
                        string base64 = await BuildingBlocks.Utility.Utility.ConvertHtmlToPdf(reslt, request.BatchID, billdetailsmodel);
                        return Results.Ok(new DownloadMTRResult(true, base64));
                    }
                    else
                        return Results.Ok(new DownloadMTRErrorResponse(StatusCodes.Status200OK, false, "Unabale to find file"));
                }
                catch (Exception)
                {
                    return Results.Ok(new DownloadMTRErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                }

            })
            .WithName("DownloadMTR")
            .RequireAuthorization();

    }
}

internal class DownloadMTRQueryHandler : IQueryHandler<DownloadMTRQuery, string>
{

    private readonly IDapper _dapper;
    public DownloadMTRQueryHandler(IDapper dapper)
    {
        _dapper = dapper;
    }

    public async Task<string> Handle(DownloadMTRQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("p_BtachId", request.model.BatchID, DbType.String);
            myparam.Add("p_docName", "MTR45", DbType.String);


            string fileData = await Task.FromResult(_dapper.Get<string>("usp_GetDocHtml", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return fileData;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


}



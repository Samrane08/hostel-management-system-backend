

namespace DDO_Service.API.Features.InsertCreditToPool
{
    public record InsertCreditToPoolCommand(InsertCreditToPoolModel model) : IQuery<string>;
    public record InsertCreditToPoolResult(int statusCode, bool status, string message);
    public record InsertCreditToPoolErrorResponse(int statusCode, bool status, string message);

    public class InsertCreditToPoolCommandValidator : AbstractValidator<InsertCreditToPoolCommand>
    {
        public InsertCreditToPoolCommandValidator()
        {
            RuleFor(x => x.model.BillNumber).NotEmpty().WithMessage("BillNumber can't be empty.")
                .MinimumLength(6).WithMessage("BillNumber must be 6 character");
            RuleFor(x => x.model.TransactionDate).NotEmpty()
                .WithMessage("TransactionDate can't be empty.");
            RuleFor(x => x.model.CreditAmount).NotEmpty().WithMessage("CreditAmount can't be empty.")
              .GreaterThan(0).WithMessage("CreditAmount must be greater than 0");
            RuleFor(x => x.model.CreditPoolAccountNo).NotEmpty().WithMessage("CreditPoolAccountNo can't be empty.")
              .MinimumLength(8).WithMessage("CreditPoolAccountNo min 8 characters");
            RuleFor(x => x.model.CreditPoolBranch).NotEmpty().WithMessage("CreditPoolBranch can't be empty.");
            RuleFor(x => x.model.UTRNo).NotEmpty().WithMessage("UTRNo can't be empty.")
                .MinimumLength(8).WithMessage("UTRNo must be at least 8 characters");
            RuleFor(x => x.model.RemitterAccountNo).NotEmpty().WithMessage("RemitterAccountNo can't be empty.")
                .MinimumLength(8).WithMessage("RemitterAccountNo must be at least 8 character");
            RuleFor(x => x.model.SchemeId).NotEmpty().WithMessage("SchemeId can't be empty.")
         .GreaterThan(0).WithMessage("SchemeId must be greater than 0");
            RuleFor(x => x.model.CreditPoolBankName).NotEmpty()
                .WithMessage("CreditPoolBankName can't be empty.");


        }

    }

    public class InsertCreditToPool : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/ddo/insert-creditPoolAccount",
                async (InsertCreditToPoolModel request, ISender sender, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
                {
                    try
                    {
                       
                            request.UserId = claimsService.GetUserId();
                            var reslt = await sender.Send(new InsertCreditToPoolCommand(request));

                            if (reslt == "success") return Results.Ok(new InsertCreditToPoolResult(StatusCodes.Status200OK, true, "Credit pool data saved successfully"));
                            else return Results.Ok(new InsertCreditToPoolErrorResponse(StatusCodes.Status200OK, false, "Unabale to save credit pool data"));
                        
                    }
                    catch (Exception)
                    {
                        return Results.BadRequest(new InsertCreditToPoolErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("InsertCreditToPool")
                .RequireAuthorization();
              
        }
    }

    internal class InsertCreditToPoolCommandHandler : IQueryHandler<InsertCreditToPoolCommand, string>
    {

        private readonly IDapper _dapper;
        public InsertCreditToPoolCommandHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<string> Handle(InsertCreditToPoolCommand request, CancellationToken cancellationToken)
        {
            try
            {
              

                var MyParam = new DynamicParameters();
                MyParam.Add("P_DealerCode", request.model.BillNumber, DbType.String);
                MyParam.Add("P_PostDate", request.model.TransactionDate.ToString("yyyy-MM-dd"), DbType.String);
                MyParam.Add("P_Amount", request.model.CreditAmount, DbType.String);
                MyParam.Add("P_AccountNo", request.model.CreditPoolAccountNo, DbType.String);
                MyParam.Add("P_BranchCode", request.model.CreditPoolBranch, DbType.String);
                MyParam.Add("P_JournalNo", request.model.UTRNo, DbType.String);
                MyParam.Add("P_RemitterAccountNo", request.model.RemitterAccountNo, DbType.String);
                MyParam.Add("P_SchemeId", request.model.SchemeId, DbType.Int32);
                MyParam.Add("P_BankName", request.model.CreditPoolBankName, DbType.String);
                MyParam.Add("P_UserId", request.model.UserId, DbType.String);

                var result = await Task.FromResult(_dapper.Insert<string>("USP_InsertBillToPoolAccountSummaryDetails_With_Status_Updation", MyParam, commandType: System.Data.CommandType.StoredProcedure));
                return result;

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

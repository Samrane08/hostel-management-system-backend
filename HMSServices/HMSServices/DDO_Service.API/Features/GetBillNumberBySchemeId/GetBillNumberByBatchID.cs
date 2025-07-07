
using Microsoft.AspNetCore.Mvc;


namespace DDO_Service.API.Features.GetBillNumberBySchemeId
{
   

    public record GetBillNumberByBatchIDQuery(int SchemeId,string UserId) : IQuery<List<DropdownModel>>;
    public record GetBillNumberByBatchIDResult(int statusCode, bool status, string message, List<DropdownModel> Data);
    public record GetBillNumberByBatchIDErrorResponse(int statusCode, bool status, string message);

    public class GetBillNumberByBatchIDQueryValidator : AbstractValidator<GetBillNumberByBatchIDQuery>
    {
        public GetBillNumberByBatchIDQueryValidator()
        {
            RuleFor(x => x.SchemeId).NotEmpty().WithMessage("Scheme Id can't be empty.")
                .GreaterThan(0).WithMessage("Scheme Id must be greater than 0");


        }

    }
    public class GetBillNumberByBatchID : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/ddo/get-GetBillNumber",
                async ([FromBody] int SchemeId, ISender sender, DDO_Service.API.Features.Token.IUserClaimsService claimsService) =>
                {
                    try
                    {
                        string _userId = claimsService.GetUserId();
                        if (string.IsNullOrEmpty(_userId))
                        {
                            return Results.Unauthorized();
                        }
                        else {
                            var reslt = await sender.Send(new GetBillNumberByBatchIDQuery(SchemeId, _userId));

                            if (reslt.Any()) return Results.Ok(new GetBillNumberByBatchIDResult(StatusCodes.Status200OK, true, "Bill found successfully", reslt));
                            else return Results.Ok(new GetBillNumberByBatchIDErrorResponse(StatusCodes.Status200OK, false, "No bill found"));
                        }
                    }
                    catch (Exception)
                    {
                        return Results.BadRequest(new GetBillNumberByBatchIDErrorResponse(StatusCodes.Status400BadRequest, false, "Internal server error"));
                    }

                })
                .WithName("GetBillNumberByBatchID")
                .RequireAuthorization();
             
        }
    }

    internal class GetBillNumberByBatchIDQueryHandler : IQueryHandler<GetBillNumberByBatchIDQuery, List<DropdownModel>>
    {

        private readonly IDapper _dapper;
        public GetBillNumberByBatchIDQueryHandler(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<List<DropdownModel>> Handle(GetBillNumberByBatchIDQuery request, CancellationToken cancellationToken)
        {
            try
            {
               
                var MyParam = new DynamicParameters();
                MyParam.Add("p_SchemeId", request.SchemeId, DbType.Int32);
                MyParam.Add("p_UserId", request.UserId, DbType.String);


                var result = await Task.FromResult(_dapper.GetAll<DropdownModel>("usp_GetBillBySchemeId", MyParam, commandType: System.Data.CommandType.StoredProcedure));
                return result;
               
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}

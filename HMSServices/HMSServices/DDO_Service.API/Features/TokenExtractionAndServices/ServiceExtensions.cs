using DDO_Service.API.Features.AuthorizationLetter;
using DDO_Service.API.Features.BillDetailsByBatchId;
using DDO_Service.API.Features.CancelBill;

using DDO_Service.API.Features.ExportBeneficiaryExcel;
using DDO_Service.API.Features.GenerateXML;
using DDO_Service.API.Features.GetAccountDetailsByBatchIdAndSchemeId;
using DDO_Service.API.Features.GetAllotmentList;
using DDO_Service.API.Features.GetBeamsSchemeCode;
using DDO_Service.API.Features.GetBillGenerationProcessData;
using DDO_Service.API.Features.GetBillNumberBySchemeId;
using DDO_Service.API.Features.GetFinancialYear;
using DDO_Service.API.Features.GetSchemes;
using DDO_Service.API.Features.InsertBatchWiseBillGeneration;
using DDO_Service.API.Features.InsertCreditToPool;
using DDO_Service.API.Features.Token;
using DDO_Service.API.Features.ValidationBehavior;


namespace DDO_Service.API.Features.TokenExtractionAndServices
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterAppServices(this IServiceCollection services)
        {
            // Register your services here
            services.AddScoped<IUserClaimsService, UserClaimsService>();
            services.AddValidatorsFromAssemblyContaining<CancelBillCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<CheckDDO_BalanceQueryValidator>();
            services.AddValidatorsFromAssemblyContaining<AuthorizationLetterValidator>();
            services.AddValidatorsFromAssemblyContaining<BillDetailsByBatchIdCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<BillDetailsByBatchIdForSubmitQueryValidator>();
            services.AddValidatorsFromAssemblyContaining<CheckDDOdetailsQueryValidator>();
            services.AddValidatorsFromAssemblyContaining<CheckDDO_BalanceQueryRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<GetFinancialYearQueryBySchemeIdValidator>();
            services.AddValidatorsFromAssemblyContaining<ExportBeneficiaryExcelQueryValidator>();
            services.AddValidatorsFromAssemblyContaining<GenerateXMLQueryValidator>();
            services.AddValidatorsFromAssemblyContaining<GetBeamsSchemeCodeQueryValidator>();
            services.AddValidatorsFromAssemblyContaining<GetAccountDetailsByBatchIdAndSchemeIdQueryValidator>();
            services.AddValidatorsFromAssemblyContaining<GetBillGenerationProcessDataQueryValidator>();
            services.AddValidatorsFromAssemblyContaining<GetAllotmentListQueryValidator>();
            services.AddValidatorsFromAssemblyContaining<GetBillNumberByBatchIDQueryValidator>();
            services.AddValidatorsFromAssemblyContaining<GetInstallmentTypeQueryValidator>();
            services.AddValidatorsFromAssemblyContaining<InsertBatchWiseBillGenerationCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<InsertCreditToPoolCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<GetDDODetailsBySchemeIdAndUserIdValidator>();
            services.AddValidatorsFromAssemblyContaining<InsertBEAMSXMLBillGenerationRequestDetailsCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<InsertBEAMSXMLBillGenerationResponseDetailsCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<InsertMTR45DetailsCommandValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));



            return services;
        }
    }
}

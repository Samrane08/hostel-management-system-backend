using Helper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Model.Common;
using NotificationService.Helper;
using NotificationService.Service;
using Quartz;
using Repository.Interface;
using Service.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddJsonOptions(option => { option.JsonSerializerOptions.PropertyNamingPolicy = null; });
builder.Services.AddSignalR();
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var conconcurrentJobKey = new JobKey("ConconcurrentJob");
    q.AddJob<ConconcurrentJob>(opts => opts.WithIdentity(conconcurrentJobKey));
    q.AddTrigger(opts => opts
     .ForJob(conconcurrentJobKey)
     .WithIdentity("ConconcurrentJob-trigger")
     .WithSimpleSchedule(x => x
     .WithIntervalInSeconds(30)
     .RepeatForever()));

    var nonConconcurrentJobKey = new JobKey("NonConconcurrentJob");
    q.AddJob<NonConconcurrentJob>(opts => opts.WithIdentity(nonConconcurrentJobKey));
    q.AddTrigger(opts => opts
     .ForJob(nonConconcurrentJobKey)
     .WithIdentity("NonConconcurrentJob-trigger")
     .WithSimpleSchedule(x => x
     .WithIntervalInSeconds(10)
     .RepeatForever()));
});
builder.Services.AddQuartzHostedService(option => option.WaitForJobsToComplete = true);
builder.Services.AddCors();
builder.Services.AddService(builder.Configuration);
builder.Services.AddTransient<ITokenHelper,TokenHelper>();
var jwtAppSettingOptions = builder.Configuration.GetSection(nameof(JwtIssuerOptions));
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddTokenAuthentication(jwtAppSettingOptions);
builder.Services.Configure<JwtIssuerOptions>(options =>
{
    options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
    options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
    options.ValidFor = Convert.ToInt32(jwtAppSettingOptions[nameof(JwtIssuerOptions.ValidFor)]);
    options.ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.ValidIssuer)];
    options.ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.ValidAudience)];
    options.IssuerSigningKey = jwtAppSettingOptions[nameof(JwtIssuerOptions.IssuerSigningKey)];
    options.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtAppSettingOptions[nameof(JwtIssuerOptions.IssuerSigningKey)])), SecurityAlgorithms.HmacSha256);
});
builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecks();
builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("NotificationService", new OpenApiInfo { Title = "Notification Service API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference =new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id= "Bearer"
                }
            },
            new string[]{ }
        }
    });
});
builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSetting"));
builder.Services.Configure<SMSSetting>(builder.Configuration.GetSection("SMSSetting"));
builder.Services.Configure<EnvironmentSetting>(builder.Configuration.GetSection("EnvironmentSetting"));

var app = builder.Build();
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

        ExceptionLogging.LogException(Convert.ToString(exceptionHandlerFeature));


        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An error occurred.");
    });
});
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/NotificationService/swagger.json", "My API Version: 1.0"));
app.UseMiddleware<SignalRClientMiddleware>();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/healthz");
app.MapHub<NotifyHub>("/Notify");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try { await services.AddSeed(); }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
    }
}
app.Run();
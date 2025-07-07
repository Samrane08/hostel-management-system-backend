using BuildingBlocks.DataAccess;
using BuildingBlocks.Interfaces;
using Carter;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new() { Title = "Grievance Api", Version = "v1" });
});
builder.Services.AddCarter();
builder.Services.AddScoped<IDapper, Dapperr>();
//builder.Services.AddScoped<IGrievance, GrievanceImp>();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});


var app = builder.Build();


app.MapCarter();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint(
    "/swagger/v1/swagger.json", "v1"));
app.Run();

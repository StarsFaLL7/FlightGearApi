using Application;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using webapi.Middlewares;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Flight Gear API", Version = "v1" });
    
});

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddCors();

builder.Services.AddExceptionHandler<FlightGearExceptionHandler>();
builder.Services.TryAddApplicationLayer();
builder.Services.TryAddInfrastructure();

var app = builder.Build();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options.AllowAnyOrigin());

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
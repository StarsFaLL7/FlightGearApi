using FlightGearApi.Domain.FlightGearCore;
using FlightGearApi.Domain.Logging;
using FlightGearApi.Infrastructure;
using FlightGearApi.Infrastructure.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Flight Gear API", Version = "v1" });

    // Путь к XML-комментариям
    var xmlPath = Path.Combine(AppContext.BaseDirectory, "FlightGearApi.xml");
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddControllers();

builder.Services.AddSingleton<IoManager>();
builder.Services.AddSingleton<FlightGearLauncher>();
builder.Services.AddSingleton<ConnectionListener>();
builder.Services.AddSingleton<FlightGearManipulator>();
builder.Services.AddSingleton<ExportParametersManager>();
builder.Services.AddSingleton<IPostgresDatabase, PostgresDatabase>();

builder.Services.AddControllersWithViews();

builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowReactApp",
        b => b
            .WithOrigins("http://localhost:44409", "http://localhost:5109", "http://localhost:7110")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

StaticLogger.StartNewLog();

app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.Run();
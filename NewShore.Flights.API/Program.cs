using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewShore.Flights.API.Middlewares;
using NewShore.Flights.Application.Contracts;
using NewShore.Flights.Application.Services;
using NewShore.Flights.Domain.Contracts.Repositories;
using NewShore.Flights.Domain.DTO.Response;
using NewShore.Flights.Domain.Entities;
using NewShore.Flights.Infrastructure.ContextDB;
using NewShore.Flights.Infrastructure.ExternalServices;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

using var log = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
    //.WriteTo.File()
    .CreateLogger();

builder.Services.AddSingleton<Serilog.ILogger>(log);

//builder.Services.AddDbContext<DbContext, FlightsDBContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("flightsSql")));

builder.Services.AddScoped<IGenericRepository<DTOFlightResponse>, FlightsRepository>();
builder.Services.AddScoped<IJourneyApplicationService, JourneyApplicationService>();
builder.Services.AddScoped<IJourneyHelper, JourneyHelper>();

var configuration = builder.Configuration;

builder.Services.AddHttpClient<IGenericRepository<DTOFlightResponse>, FlightsRepository>(client =>
{
    client.BaseAddress = new Uri(configuration.GetValue<string>("FlightsEndpoint:baseUrl"));
    client.Timeout = TimeSpan.FromSeconds(20);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors(builder =>
{
    builder
          .WithOrigins("http://localhost:4200", "https://localhost:4200")
          .SetIsOriginAllowedToAllowWildcardSubdomains()
          .AllowAnyHeader()
          .AllowCredentials()
          .WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS")
          .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));

});

log.Debug("App running");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionsMiddleware>();

app.MapControllers();

app.Run();

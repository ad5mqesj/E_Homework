using FluentValidation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Diagnostics.Tracing;

using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Validators;

using E_Homework;
using E_Homework.Controllers;
using E_Homework.Providers.Implementation;
using E_Homework.Providers.Interface;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);


// Add health ervices to the container.
var redinessService = new ReadinessCheck();
redinessService.StartupCompleted = false;

//configuration
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

CommonSettings settings = config.GetRequiredSection(CommonSettings.sectionName).Get<CommonSettings>();
builder.Services.AddSingleton<CommonSettings>(settings);

//Authentication
#if NORMALAUTH
builder.Services.AddAuthentication(sharedoptions =>
{
    sharedoptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
           .AddJwtBearer(options =>
           {
               options.Authority = settings.AdInstance;
               options.TokenValidationParameters.ValidateAudience = false;
               options.TokenValidationParameters.IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(options.Authority).Validate;
               options.RequireHttpsMetadata = false;
#if DEBUG
               options.IncludeErrorDetails = true;
#endif
           });
builder.Services.AddAuthorization();
#endif

//Add logging

builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);
var AIServiceOptions = new ApplicationInsightsServiceOptions()
{
    ConnectionString = settings.AIConnectionString
};
builder.Services.AddApplicationInsightsTelemetry(AIServiceOptions);
builder.Services.Configure<AzureFileLoggerOptions>(builder.Configuration.GetSection("AzureLogging"));

builder.Services.AddLogging(logger =>
{
#if DEBUG
    logger.AddDebug();
#endif
    logger.AddApplicationInsights();
    logger.AddAzureWebAppDiagnostics();
});


// Add services to the container.
builder.Services.AddSingleton<ReadinessCheck>(redinessService);
builder.Services.AddScoped<IDataConverter>(factory =>
{
    var _logger = new LoggerFactory().CreateLogger<DataConverter>();
    return new DataConverter(_logger);
});


//builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddHealthChecks()
    .AddCheck<ReadinessCheck>("Startup", tags: new[] { "ready" });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHealthChecks("/health");
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("ready")
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => true
});

#if NORMALAUTH
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
#endif

app.MapControllers();

redinessService.StartupCompleted = true;
app.Run();

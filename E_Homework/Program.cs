using FluentValidation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Diagnostics.Tracing;

using E_Homework.Controllers;
using E_Homework.Providers.Implementation;
using E_Homework.Providers.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add health ervices to the container.
var redinessService = new ReadinessCheck();
redinessService.StartupCompleted = false;

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

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

redinessService.StartupCompleted = true;
app.Run();

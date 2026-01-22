using CustomerPlatform.Application.Commands.CreateCustomer;
using CustomerPlatform.Application.Services;
using CustomerPlatform.Application.Validators;
using CustomerPlatform.Domain.Interfaces;
using CustomerPlatform.Infrastructure.Contexts;
using CustomerPlatform.Infrastructure.Messaging;
using CustomerPlatform.Infrastructure.Repositories;
using CustomerPlatform.Infrastructure.Search;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nest;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog para enviar logs ao Loki
var lokiUrl = builder.Configuration["Loki:Url"] ?? "http://localhost:3100";
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.GrafanaLoki(
        uri: lokiUrl,
        labels: new[] { new LokiLabel { Key = "job", Value = "customer-platform" } },
        propertiesAsLabels: new[] { "level" })
    .Enrich.WithProperty("Application", "CustomerPlatform.Api")
    .Enrich.FromLogContext()
    .CreateLogger();

// Usar Serilog como provider de logging
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IMessagePublisher, RabbitMQMessagePublisher>();
builder.Services.AddScoped<IDocumentValidationService, DocumentValidationService>();

// Elasticsearch
builder.Services.AddSingleton<IElasticClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return ElasticsearchClientFactory.CreateClient(configuration);
});
builder.Services.AddScoped<IElasticsearchIndexService, ElasticsearchIndexService>();
builder.Services.AddScoped<ISearchService, ElasticsearchSearchService>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly);
    cfg.LicenseKey = string.Empty;
});
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerCommandValidator>();
builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));

builder.Services.AddDbContext<CustomerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// Configuração do OpenTelemetry com Resource para identificação
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService("customer-platform-api", serviceVersion: "1.0.0")
        .AddAttributes(new Dictionary<string, object>
        {
            ["deployment.environment"] = builder.Environment.EnvironmentName
        }))
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .AddPrometheusExporter()) // Habilita o endpoint /metrics
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Garantir que os logs sejam enviados ao fechar a aplicação
app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

app.Run();
using CustomerPlatform.Application.Commands.CreateCustomer;
using CustomerPlatform.Application.Commands.SearchCustomer;
using CustomerPlatform.Application.Commands.UpdateCustomer;
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

var builder = WebApplication.CreateBuilder(args);

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
    typeof(ValidationBehavior<,>)
);


builder.Services.AddDbContext<CustomerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


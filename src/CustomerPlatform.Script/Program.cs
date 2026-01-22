using CustomerPlatform.Application.Commands.CreateCustomer;
using CustomerPlatform.Application.Services;
using CustomerPlatform.Domain.Enums;
using CustomerPlatform.Domain.Interfaces;
using CustomerPlatform.Infrastructure.Contexts;
using CustomerPlatform.Infrastructure.Messaging;
using CustomerPlatform.Infrastructure.Repositories;
using CustomerPlatform.Infrastructure.Search;
using CustomerPlatform.Script;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nest;

var services = new ServiceCollection();

var configuration = new ConfigurationBuilder()
    .AddInMemoryCollection(new Dictionary<string, string?>
    {
        { "ConnectionStrings:Postgres", "Host=localhost;Port=5432;Database=customerplatform;Username=admin;Password=admin123" },
        { "RabbitMQ:HostName", "localhost" },
        { "RabbitMQ:Port", "5672" },
        { "RabbitMQ:UserName", "guest" },
        { "RabbitMQ:Password", "guest" },
        { "Elasticsearch:Uri", "http://localhost:9200" },
        { "Elasticsearch:DefaultIndex", "customers" },
        { "Logging:LogLevel:Default", "Information" },
        { "Logging:LogLevel:Microsoft", "Warning" },
        { "Logging:LogLevel:System", "Warning" },
        { "Logging:LogLevel:LuckyPennySoftware.MediatR.License", "None" }
    })
    .Build();

services.AddSingleton<IConfiguration>(configuration);

services.AddLogging(builder => 
{
    builder.AddConsole();
    builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
    builder.AddConfiguration(configuration.GetSection("Logging"));
});

services.AddDbContext<CustomerDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("Postgres")));

services.AddScoped<ICustomerRepository, CustomerRepository>();
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddSingleton<IMessagePublisher, RabbitMQMessagePublisher>();
services.AddScoped<IDocumentValidationService, DocumentValidationService>();
services.AddScoped<IElasticsearchIndexService, ElasticsearchIndexService>();


services.AddSingleton<IElasticClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return ElasticsearchClientFactory.CreateClient(config);
});
services.AddScoped<IElasticsearchIndexService, ElasticsearchIndexService>();
services.AddScoped<ISearchService, ElasticsearchSearchService>();
services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateCustomerHandler).Assembly);
    cfg.LicenseKey = string.Empty;
});

var serviceProvider = services.BuildServiceProvider();

IElasticsearchIndexService elasticsearchIndexService = serviceProvider.GetRequiredService<IElasticsearchIndexService>();
await elasticsearchIndexService.DeleteAllAsync();

Console.WriteLine("Iniciando processamento da massa Pessoa Física...");
var massaDeDadosPF = DadosFakePessoaFisica.GerarMassaDeDados(50);

foreach (var item in massaDeDadosPF)
{
    using var scope = serviceProvider.CreateScope();
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    
    var tipo = item.GetType();
    var email = tipo.GetProperty("email")?.GetValue(item)?.ToString() ?? string.Empty;
    var telefone = tipo.GetProperty("telefone")?.GetValue(item)?.ToString() ?? string.Empty;
    var nome = tipo.GetProperty("nome")?.GetValue(item)?.ToString() ?? string.Empty;
    var cpf = tipo.GetProperty("cpf")?.GetValue(item)?.ToString() ?? string.Empty;
    var dataNascimento = (DateTime)(tipo.GetProperty("dataNascimento")?.GetValue(item) ?? DateTime.Now);
    
    var enderecoObj = tipo.GetProperty("endereco")?.GetValue(item);
    var enderecoTipo = enderecoObj?.GetType();
    var logradouro = enderecoTipo?.GetProperty("logradouro")?.GetValue(enderecoObj)?.ToString() ?? string.Empty;
    var numero = enderecoTipo?.GetProperty("numero")?.GetValue(enderecoObj)?.ToString() ?? string.Empty;
    var complemento = enderecoTipo?.GetProperty("complemento")?.GetValue(enderecoObj)?.ToString();
    var cep = enderecoTipo?.GetProperty("cep")?.GetValue(enderecoObj)?.ToString() ?? string.Empty;
    var cidade = enderecoTipo?.GetProperty("cidade")?.GetValue(enderecoObj)?.ToString() ?? string.Empty;
    var estado = enderecoTipo?.GetProperty("estado")?.GetValue(enderecoObj)?.ToString() ?? string.Empty;

    var command = new CreateCustomerCommand
    {
        Email = email,
        Telefone = telefone,
        Endereco = new CustomerPlatform.Domain.ValueObjects.Endereco(
            logradouro,
            numero,
            cep,
            cidade,
            estado,
            complemento),
        CPF = cpf,
        DataNascimento = dataNascimento.ToUniversalTime(),
        Nome = nome,
        TipoCliente = TipoCliente.PessoaFisica
    };

    try
    {
        await mediator.Send(command);
        Console.WriteLine($"[OK] Processado: {nome}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERRO] {nome}: {ex.Message}");
    }
}

Console.WriteLine("Iniciando processamento da massa Pessoa Jurídica...");
var massaDeDadosPJ = DadosFakePessoaJuridica.GerarMassaDeDados(50);

foreach (var item in massaDeDadosPJ)
{
    using var scope = serviceProvider.CreateScope();
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    
    var tipo = item.GetType();
    var email = tipo.GetProperty("email")?.GetValue(item)?.ToString() ?? string.Empty;
    var telefone = tipo.GetProperty("telefone")?.GetValue(item)?.ToString() ?? string.Empty;
    var razaoSocial = tipo.GetProperty("razaoSocial")?.GetValue(item)?.ToString() ?? string.Empty;
    var nomeFantasia = tipo.GetProperty("nomeFantasia")?.GetValue(item)?.ToString() ?? string.Empty;
    var cnpj = tipo.GetProperty("cnpj")?.GetValue(item)?.ToString() ?? string.Empty;
    
    var enderecoObj = tipo.GetProperty("endereco")?.GetValue(item);
    var enderecoTipo = enderecoObj?.GetType();
    var logradouro = enderecoTipo?.GetProperty("logradouro")?.GetValue(enderecoObj)?.ToString() ?? string.Empty;
    var numero = enderecoTipo?.GetProperty("numero")?.GetValue(enderecoObj)?.ToString() ?? string.Empty;
    var complemento = enderecoTipo?.GetProperty("complemento")?.GetValue(enderecoObj)?.ToString();
    var cep = enderecoTipo?.GetProperty("cep")?.GetValue(enderecoObj)?.ToString() ?? string.Empty;
    var cidade = enderecoTipo?.GetProperty("cidade")?.GetValue(enderecoObj)?.ToString() ?? string.Empty;
    var estado = enderecoTipo?.GetProperty("estado")?.GetValue(enderecoObj)?.ToString() ?? string.Empty;

    var command = new CreateCustomerCommand
    {
        Email = email,
        Telefone = telefone,
        Endereco = new CustomerPlatform.Domain.ValueObjects.Endereco(
            logradouro,
            numero,
            cep,
            cidade,
            estado,
            complemento),
        CNPJ = cnpj,
        RazaoSocial = razaoSocial,
        NomeFantasia = nomeFantasia,
        TipoCliente = TipoCliente.PessoaJuridica
    };

    try
    {
        await mediator.Send(command);
        Console.WriteLine($"[OK] Processado: {razaoSocial}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERRO] {razaoSocial}: {ex.Message}");
    }
}



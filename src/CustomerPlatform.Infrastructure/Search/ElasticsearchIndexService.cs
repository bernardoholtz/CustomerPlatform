using CustomerPlatform.Domain.DTOs;
using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nest;

namespace CustomerPlatform.Infrastructure.Search
{
    public class ElasticsearchIndexService : IElasticsearchIndexService
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<ElasticsearchIndexService> _logger;
        private const string IndexName = "customers";

        public ElasticsearchIndexService(
            IElasticClient elasticClient,
            ILogger<ElasticsearchIndexService> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        public async Task EnsureIndexExistsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Verificando se índice {IndexName} existe", IndexName);
                var existsResponse = await _elasticClient.Indices.ExistsAsync(IndexName, ct: cancellationToken);

                if (!existsResponse.Exists)
                {
                    _logger.LogInformation("Índice {IndexName} não existe. Criando...", IndexName);
                    var createIndexResponse = await _elasticClient.Indices.CreateAsync(IndexName, c => c
                        .Map<CustomerIndexDocument>(m => m
                            .Properties(p => p
                                .Keyword(k => k.Name(n => n.Id))
                                .Keyword(k => k.Name(n => n.TipoCliente))
                                .Text(t => t
                                    .Name(n => n.Nome)
                                    .Analyzer("standard")
                                    .Fields(f => f
                                        .Keyword(k => k.Name("keyword"))
                                        .Text(tt => tt.Name("fuzzy").Analyzer("standard"))
                                    )
                                )
                                .Text(t => t
                                    .Name(n => n.RazaoSocial)
                                    .Analyzer("standard")
                                    .Fields(f => f
                                        .Keyword(k => k.Name("keyword"))
                                        .Text(tt => tt.Name("fuzzy").Analyzer("standard"))
                                    )
                                )
                                .Text(t => t
                                    .Name(n => n.NomeFantasia)
                                    .Analyzer("standard")
                                    .Fields(f => f
                                        .Keyword(k => k.Name("keyword"))
                                        .Text(tt => tt.Name("fuzzy").Analyzer("standard"))
                                    )
                                )
                                .Keyword(k => k.Name(n => n.Documento))
                                .Text(t => t
                                    .Name(n => n.Email)
                                    .Analyzer("standard")
                                    .Fields(f => f
                                        .Keyword(k => k.Name("keyword"))
                                    )
                                )
                                .Text(t => t
                                    .Name(n => n.Telefone)
                                    .Analyzer("standard")
                                    .Fields(f => f
                                        .Keyword(k => k.Name("keyword"))
                                    )
                                )
                                .Date(d => d.Name(n => n.DataCriacao))
                                .Date(d => d.Name(n => n.DataAtualizacao))
                                .Object<EnderecoIndexDocument>(o => o
                                    .Name(n => n.Endereco)
                                    .Properties(pp => pp
                                        .Text(tt => tt.Name(nn => nn.Logradouro).Analyzer("standard"))
                                        .Keyword(k => k.Name(nn => nn.Numero))
                                        .Text(tt => tt.Name(nn => nn.Complemento).Analyzer("standard"))
                                        .Text(tt => tt.Name(nn => nn.Cidade).Analyzer("standard"))
                                        .Keyword(k => k.Name(nn => nn.Estado))
                                        .Keyword(k => k.Name(nn => nn.CEP))
                                    )
                                )
                            )
                        )
                        .Settings(s => s
                            .Analysis(a => a
                                .Analyzers(an => an
                                    .Standard("standard", st => st
                                        .StopWords("_none_")
                                    )
                                )
                            )
                        ),
                        cancellationToken);

                    if (createIndexResponse.IsValid)
                    {
                        _logger.LogInformation("Índice {IndexName} criado com sucesso", IndexName);
                    }
                    else
                    {
                        // Verifica se o erro é porque o índice já existe (race condition)
                        if (createIndexResponse.ServerError?.Error?.Type == "resource_already_exists_exception")
                        {
                            _logger.LogInformation(
                                "Índice {IndexName} já existe (criado por outra thread). Continuando...",
                                IndexName);
                        }
                        else
                        {
                            _logger.LogError(
                                "Erro ao criar índice {IndexName}: {Error}",
                                IndexName,
                                createIndexResponse.DebugInformation);
                            throw new Exception($"Erro ao criar índice: {createIndexResponse.DebugInformation}");
                        }
                    }
                }
                else
                {
                    _logger.LogDebug("Índice {IndexName} já existe", IndexName);
                }
            }
            catch (Exception ex)
            {
                // Se for erro de índice já existente, apenas loga e continua
                if (ex.Message.Contains("resource_already_exists_exception") || 
                    ex.Message.Contains("already exists"))
                {
                    _logger.LogInformation(
                        "Índice {IndexName} já existe. Continuando...",
                        IndexName);
                    return;
                }

                _logger.LogError(
                    ex,
                    "Erro ao verificar/criar índice {IndexName}",
                    IndexName);
                throw;
            }
        }

        public async Task IndexCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Iniciando indexação do cliente {CustomerId}", customer.Id);
                await EnsureIndexExistsAsync(cancellationToken);

                var document = MapToIndexDocument(customer);

                var response = await _elasticClient.IndexAsync(
                    document,
                    i => i.Index(IndexName).Id(customer.Id),
                    cancellationToken);

                if (response.IsValid)
                {
                    _logger.LogInformation("Cliente {CustomerId} indexado com sucesso", customer.Id);
                }
                else
                {
                    _logger.LogError(
                        "Erro ao indexar cliente {CustomerId}: {Error}",
                        customer.Id,
                        response.DebugInformation);
                    throw new Exception($"Erro ao indexar cliente: {response.DebugInformation}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao indexar cliente {CustomerId}",
                    customer.Id);
                throw;
            }
        }

        public async Task DeleteCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Iniciando remoção do cliente {CustomerId} do índice", customerId);
                var response = await _elasticClient.DeleteAsync<CustomerIndexDocument>(
                    customerId,
                    d => d.Index(IndexName),
                    cancellationToken);

                if (response.IsValid)
                {
                    _logger.LogInformation("Cliente {CustomerId} removido do índice com sucesso", customerId);
                }
                else if (response.Result == Result.NotFound)
                {
                    _logger.LogWarning("Cliente {CustomerId} não encontrado no índice", customerId);
                }
                else
                {
                    _logger.LogError(
                        "Erro ao remover cliente {CustomerId} do índice: {Error}",
                        customerId,
                        response.DebugInformation);
                    throw new Exception($"Erro ao remover cliente do índice: {response.DebugInformation}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao remover cliente {CustomerId} do índice",
                    customerId);
                throw;
            }
        }

        private static CustomerIndexDocument MapToIndexDocument(Customer customer)
        {
            var document = new CustomerIndexDocument
            {
                Id = customer.Id,
                Email = customer.Email,
                Telefone = customer.Telefone,
                DataCriacao = customer.DataCriacao,
                DataAtualizacao = customer.DataAtualizacao,
                Endereco = new EnderecoIndexDocument
                {
                    Logradouro = customer.Endereco.Logradouro,
                    Numero = customer.Endereco.Numero,
                    Complemento = customer.Endereco.Complemento,
                    Cidade = customer.Endereco.Cidade,
                    Estado = customer.Endereco.Estado,
                    CEP = customer.Endereco.CEP
                }
            };

            switch (customer)
            {
                case ClientePessoaFisica pf:
                    document.TipoCliente = "PessoaFisica";
                    document.Nome = pf.Nome;
                    document.Documento = pf.CPF;
                    break;

                case ClientePessoaJuridica pj:
                    document.TipoCliente = "PessoaJuridica";
                    document.Nome = pj.NomeFantasia;
                    document.RazaoSocial = pj.RazaoSocial;
                    document.NomeFantasia = pj.NomeFantasia;
                    document.Documento = pj.CNPJ;
                    break;
            }

            return document;
        }

        public async Task DeleteAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogWarning("Iniciando remoção de todos os clientes do índice {IndexName}", IndexName);
                var response = await _elasticClient.Indices.DeleteAsync("customers");

                if (response.IsValid)
                {
                    _logger.LogInformation("Todos os clientes removidos com sucesso do índice {IndexName}", IndexName);
                }
                else
                {
                    _logger.LogError(
                        "Erro ao remover todos os clientes: {Error}",
                        response.DebugInformation);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao remover todos os clientes do índice {IndexName}",
                    IndexName);
                throw;
            }
        }
    }
}

using CustomerPlatform.Domain.DTOs;
using CustomerPlatform.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Nest;

namespace CustomerPlatform.Infrastructure.Search
{
    public class ElasticsearchSearchService : ISearchService
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<ElasticsearchSearchService> _logger;
        private const string IndexName = "customers";

        public ElasticsearchSearchService(
            IElasticClient elasticClient,
            ILogger<ElasticsearchSearchService> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        public async Task<CustomerSearchResponse> SearchAsync(
            CustomerSearchRequest request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug(
                    "Iniciando busca no Elasticsearch. Filtros: Nome={Nome}, CPF={CPF}, CNPJ={CNPJ}, Page={Page}, PageSize={PageSize}",
                    request.Nome,
                    request.CPF,
                    request.CNPJ,
                    request.Page,
                    request.PageSize);

                var searchDescriptor = new SearchDescriptor<CustomerIndexDocument>()
                    .Index(IndexName)
                    .From((request.Page - 1) * request.PageSize)
                    .Size(request.PageSize)
                    .Query(q => BuildQuery(request, q))
                    .Sort(s => s
                        .Descending(SortSpecialField.Score)
                        .Descending(d => d.DataCriacao));

                var response = await _elasticClient.SearchAsync<CustomerIndexDocument>(
                    searchDescriptor,
                    cancellationToken);

                if (!response.IsValid)
                {
                    _logger.LogError(
                        "Erro na busca no Elasticsearch: {Error}",
                        response.DebugInformation);
                    throw new Exception($"Erro na busca: {response.DebugInformation}");
                }

            var results = response.Hits.Select(hit => new CustomerSearchResult
            {
                Id = hit.Source.Id,
                TipoCliente = hit.Source.TipoCliente,
                Nome = hit.Source.Nome,
                RazaoSocial = hit.Source.RazaoSocial,
                NomeFantasia = hit.Source.NomeFantasia,
                Documento = hit.Source.Documento,
                Email = hit.Source.Email,
                Telefone = hit.Source.Telefone,
                DataCriacao = hit.Source.DataCriacao,
                DataAtualizacao = hit.Source.DataAtualizacao,
                Score = hit.Score ?? 0,
                Endereco = hit.Source.Endereco != null ? new EnderecoSearchResult
                {
                    Logradouro = hit.Source.Endereco.Logradouro,
                    Numero = hit.Source.Endereco.Numero,
                    Complemento = hit.Source.Endereco.Complemento,
                    Cidade = hit.Source.Endereco.Cidade,
                    Estado = hit.Source.Endereco.Estado,
                    CEP = hit.Source.Endereco.CEP
                } : null
            }).ToList();

                var searchResponse = new CustomerSearchResponse
                {
                    Results = results,
                    Total = (int)response.Total,
                    Page = request.Page,
                    PageSize = request.PageSize
                };

                _logger.LogDebug(
                    "Busca no Elasticsearch concluída. Total encontrado: {Total}",
                    searchResponse.Total);

                return searchResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao realizar busca no Elasticsearch. Filtros: Nome={Nome}, CPF={CPF}, CNPJ={CNPJ}",
                    request.Nome,
                    request.CPF,
                    request.CNPJ);
                throw;
            }
        }

        private static QueryContainer BuildQuery(
            CustomerSearchRequest request,
            QueryContainerDescriptor<CustomerIndexDocument> q)
        {
            var queries = new List<QueryContainer>();

            // Busca fuzzy por Nome/Razão Social
            if (!string.IsNullOrWhiteSpace(request.Nome))
            {
                queries.Add(q.Bool(b => b
                    .Should(
                        // Fuzzy search no nome (PF)
                        q.Fuzzy(f => f
                            .Field(fld => fld.Nome)
                            .Value(request.Nome)
                            .Fuzziness(Fuzziness.Auto)
                            .Boost(2.0)
                        ),
                        // Fuzzy search na razão social (PJ)
                        q.Fuzzy(f => f
                            .Field(fld => fld.RazaoSocial)
                            .Value(request.Nome)
                            .Fuzziness(Fuzziness.Auto)
                            .Boost(2.0)
                        ),
                        // Fuzzy search no nome fantasia (PJ)
                        q.Fuzzy(f => f
                            .Field(fld => fld.NomeFantasia)
                            .Value(request.Nome)
                            .Fuzziness(Fuzziness.Auto)
                            .Boost(1.5)
                        ),
                        // Match phrase para busca exata (maior relevância)
                        q.MatchPhrase(m => m
                            .Field(fld => fld.Nome)
                            .Query(request.Nome)
                            .Boost(3.0)
                        ),
                        q.MatchPhrase(m => m
                            .Field(fld => fld.RazaoSocial)
                            .Query(request.Nome)
                            .Boost(3.0)
                        ),
                        // Match para busca parcial
                        q.Match(m => m
                            .Field(fld => fld.Nome)
                            .Query(request.Nome)
                            .Operator(Operator.Or)
                            .Fuzziness(Fuzziness.Auto)
                            .Boost(1.0)
                        )
                    )
                    .MinimumShouldMatch(1)
                ));
            }

            // Busca exata por CPF
            if (!string.IsNullOrWhiteSpace(request.CPF))
            {
                queries.Add(q.Term(t => t
                    .Field(f => f.Documento)
                    .Value(request.CPF.Replace(".", "").Replace("-", ""))
                    .Boost(5.0)
                ));
            }

            // Busca exata por CNPJ
            if (!string.IsNullOrWhiteSpace(request.CNPJ))
            {
                queries.Add(q.Term(t => t
                    .Field(f => f.Documento)
                    .Value(request.CNPJ
                        .Replace(".", "")
                        .Replace("-", "")
                        .Replace("/", ""))
                    .Boost(5.0)
                ));
            }

            // Busca por Email (exata ou parcial, case-insensitive)
            // Usar MatchPhrase no campo analisado: Wildcard não funciona em text (tokenizado).
            // MatchPhrase usa o analyzer (lowercase) e faz match de frase/substring.
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var emailTrimmed = request.Email.Trim();
                queries.Add(q.MatchPhrase(m => m
                    .Field(f => f.Email)
                    .Query(emailTrimmed)
                    .Boost(2.0)
                ));
            }

            // Busca parcial por Telefone
            if (!string.IsNullOrWhiteSpace(request.Telefone))
            {
                queries.Add(q.Wildcard(w => w
                    .Field(f => f.Telefone)
                    .Value($"*{request.Telefone}*")
                    .Boost(2.0)
                ));
            }

            // Se não houver nenhum filtro, retorna todos os documentos
            if (queries.Count == 0)
            {
                return q.MatchAll();
            }

            // Combina todas as queries com AND (todos os filtros devem ser satisfeitos)
            return q.Bool(b => b.Must(queries.ToArray()));
        }
    }
}

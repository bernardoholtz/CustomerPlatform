using Microsoft.Extensions.Configuration;
using Nest;

namespace CustomerPlatform.Infrastructure.Search
{
    /// <summary>
    /// Factory para criar instância do Elasticsearch client
    /// </summary>
    public static class ElasticsearchClientFactory
    {
        public static IElasticClient CreateClient(IConfiguration configuration)
        {
            var uri = configuration["Elasticsearch:Uri"] ?? "http://localhost:9200";
            var defaultIndex = configuration["Elasticsearch:DefaultIndex"] ?? "customers";

            var settings = new ConnectionSettings(new Uri(uri))
                .DefaultIndex(defaultIndex)
                .EnableApiVersioningHeader()
                .PrettyJson()
                .RequestTimeout(TimeSpan.FromSeconds(30));

            return new ElasticClient(settings);
        }
    }
}

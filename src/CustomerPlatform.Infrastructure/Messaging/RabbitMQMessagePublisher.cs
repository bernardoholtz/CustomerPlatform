using CustomerPlatform.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;


namespace CustomerPlatform.Infrastructure.Messaging
{
    /// <summary>
    /// Implementação do publisher de mensagens usando RabbitMQ
    /// </summary>
    public class RabbitMQMessagePublisher : IMessagePublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitMQMessagePublisher> _logger;
        private const string queueName = "EventosCliente";

        public RabbitMQMessagePublisher(
            IConfiguration configuration,
            ILogger<RabbitMQMessagePublisher> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var hostName = configuration["RabbitMQ:HostName"] ?? "localhost";
            var port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672");
            var userName = configuration["RabbitMQ:UserName"] ?? "guest";
            var password = configuration["RabbitMQ:Password"] ?? "guest";

            var factory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password,
   
            };

            try
            {
                _connection = factory.CreateConnection();
                _logger.LogInformation("Conexão com RabbitMQ estabelecida com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao conectar com RabbitMQ");
                throw;
            }
        }

        public async Task PublishAsync<T>( T message, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                using var _channel = _connection.CreateModel();
                // Declara a fila se não existir
                _channel.QueueDeclare(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true; // Garante que a mensagem será persistida

                _channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: queueName,
                    basicProperties: properties,
                    body: body);

                _logger.LogInformation("Mensagem publicada na fila {QueueName}: {Message}", queueName, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao publicar mensagem na fila {QueueName}", queueName);
                throw;
            }
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}

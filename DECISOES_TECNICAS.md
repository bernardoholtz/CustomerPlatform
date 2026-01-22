## 🏗️ Decisões Técnicas

## Banco de Dados: PotGreSql
Para banco de dados de milhões de registros que necessita de forte consistência transacional e relacionamento a fim de evitar duplicatas, optei pelo PostgreSQL.
Além disso, ele escala muito bem verticalmente e possui alta disponibilidade.
MongoDB, apesar de ter boa performance, possui consistência transacional limitada.
O Sql Server se aproxima mais do Postgre, mas acaba saindo mais custoso.

## Mensageria: Rabbit
Considerando a natureza do desafio (Deduplicação e Notificações), o RabbitMQ é a escolha mais equilibrada:
Garantia de Entrega: Essencial para o processo de deduplicação. Se um cliente for criado, o evento ClienteCriado precisa ser processado pelo Worker de deduplicação.
Roteamento Flexível: Com o sistema de Exchanges e Queues, você pode facilmente enviar o mesmo evento para diferentes consumidores (um para o Worker de Deduplicação e outro para um serviço de e-mail, por exemplo).
Curva de Aprendizado vs. Poder: Diferente do Kafka (que é excelente para trilhões de eventos e streaming), o RabbitMQ é mais simples de configurar em um docker-compose para um desafio técnico, oferecendo todas as garantias de resiliência necessárias (Ack/Nack, Retries, DLQ).

## Motor de Busca Probabilística: Elasticsearch

Performance em Larga Escala: Ele indexa os dados de forma invertida, permitindo buscas em milissegundos mesmo com milhões de linhas.

Algoritmos de Similaridade Nativos: Ele já implementa internamente algoritmos como BM25 e suporte a fuzziness (baseado na distância de Levenshtein), facilitando a busca por "Joao Silv" e retornando "João Silva".

Sincronização: Postgres como Escrita e o Elasticsearch como "Base de Consulta" (Leitura).

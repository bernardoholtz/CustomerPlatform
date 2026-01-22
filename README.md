# Desafio Técnico: Plataforma de Cadastro de Clientes

## Realize o passo a passo abaixo

1- Fazer o clone da Api utilizando o comando abaixo:
https://github.com/bernardoholtz/CustomerPlatform.git

2-Fazer o clone do Worker no mesmo diretório com o comando abaixo:
https://github.com/bernardoholtz/Duplicatas.git

Você terá os dois diretórios
../CustomerPlatform
../Duplicatas


3-Navegar até o diretório cd CustomerPlatform e executar o comando abaixo para criar os containers Docker:
 docker compose up

4)Executar o comando abaixo para restaurar dependências:
dotnet restore


5)Digite cd.. para retornar ao diretório src e navegue até cd CustomerPlatform.Script
Executar o comando abaixo para rodar script de geração de dados fakes para clientes pessoa física e jurídica:
dotnet run

Esse script, criará clientes para testes e por consequência disso, novos mensagens para fila Rabbit serão enviadas. 

6) Retorne até src, navegue até cd CustomerPlatform.Api e execute comando abaixo para iniciar a Api:
dotnet run

7)Em outro terminal, navegue até Duplicatas e execute o comando abaixo para restaurar pacotes:
dotnet restore

8) Navegue até cd Duplicatas.Worker e execute comando abaixo para iniciar o Worker:
dotnet run

O Worker irá consumir a fila dos cadastros gerados pelo script e gerar eventos de duplicidade caso necessário.

9) Para acessar Api, entre no link abaixo:
http://localhost:5000/swagger/index.html

10) Para visualização de filas no Rabbit, acessar link abaixo:
http://localhost:15672/#/

11)Para monitoração de logs e métricas, acessar link abaixo:
http://localhost:3000/dashboards

-------------------------------------------------------------------------------------------

## Resumo do Projeto:

O projeto possui uma api com os endpoint de cadastro de clientes, edição de clientes, busca avançada e listagem de duplicados.

No momento do cadastro ou edição, os dados do cliente são enviados até uma base ElasticSearch além de ser disparados eventos de mensageria ao Rabbit ("CienteAtualizado","ClienteCriado")

A busca avançada, pode ser combinada entre vários campos, sendo que o campo nome foi aplicado algoritmo Fuzzy para consulta.

O worker consome a fila do rabbit e compara cada evento com a base ElasticSearch a fim de encontrar possíveis duplicatas. Foi aplicado técnica Levenshtein na consulta.

Logs e métricas podem ser visualizados no dashboards feito com Grafana. A métricas são geradas Prometheus e os logs utilizando ILogger.

Foi implementado testes unitários e de integração nas duas aplicações (Api e Worker)



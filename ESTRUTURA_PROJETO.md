# 📁 Estrutura do Projeto

## Visão Geral

Este desafio fornece uma estrutura base para avaliar a proficiência de candidatos no uso de ferramentas de IA para desenvolvimento .NET.

## Estrutura de Pastas

```
Desafio-IA-DotNet/
├── src/
│   ├── CustomerPlatform.Api/           # Projeto da API Web
│   │   ├── Controllers/                 # Controllers REST
│   │   ├── Properties/                  # Configurações de execução
│   │   ├── Program.cs                   # Ponto de entrada
│   │   └── appsettings.json            # Configurações
│   ├── CustomerPlatform.Domain/         # Camada de domínio
│   │   └── Entities/                    # Entidades do domínio
│   │       └── Customer.cs              # Modelo base de cliente
│   └── CustomerPlatform.Infrastructure/ # Camada de infraestrutura (vazia)
├── tests/
│   └── CustomerPlatform.Tests/          # Projeto de testes
│       └── CustomerTests.cs             # Testes exemplo
├── CustomerPlatform.sln                 # Solution .NET
├── docker-compose.exemplo.yml           # Exemplo de infraestrutura
├── nuget.config                         # Configuração do NuGet
├── .gitignore                           # Arquivos ignorados pelo Git
├── README.md                            # Introdução ao desafio
├── DESAFIO.md                           # Enunciado completo
├── CRITERIOS_AVALIACAO.md              # Como será avaliado
├── TEMPLATE_ENTREGA.md                  # Template para prompts
├── DECISOES_TECNICAS.md                 # Template para justificativas
└── COMO_EXECUTAR.md                     # Instruções de execução
```

## Arquivos Importantes

### Para o Candidato

1. **README.md** - Leia primeiro! Introdução e regras
2. **DESAFIO.md** - Requisitos funcionais e técnicos detalhados
3. **CRITERIOS_AVALIACAO.md** - Como será avaliado (importante!)
4. **TEMPLATE_ENTREGA.md** - Como documentar os prompts (obrigatório)
5. **DECISOES_TECNICAS.md** - Template para justificar escolhas
6. **COMO_EXECUTAR.md** - Como rodar o projeto base

### Para o Avaliador

1. **CRITERIOS_AVALIACAO.md** - Rubrica de avaliação detalhada
2. **README.md** - Visão geral do desafio
3. **Arquivos do candidato:**
   - `PROMPTS_UTILIZADOS.md` - Prompts documentados
   - `DECISOES_TECNICAS.md` - Justificativas preenchidas
   - `README.md atualizado` - Com instruções de execução

## Projeto Base Fornecido

### O que JÁ está implementado

✅ Estrutura de solution com 4 projetos
✅ Entidades de domínio base (Customer, ClientePF, ClientePJ)
✅ API minimalista com Swagger
✅ Health check endpoint
✅ Projeto de testes com xUnit
✅ Exemplo de Docker Compose (PostgreSQL + ElasticSearch + Kafka)
✅ Configuração do NuGet
✅ .gitignore configurado

### O que o candidato DEVE implementar

❌ Lógica de cadastro de clientes
❌ Integração com banco de dados (escolha do candidato)
❌ Motor de busca probabilística (escolha do candidato)
❌ Sistema de mensageria (escolha do candidato)
❌ Lógica de deduplicação
❌ Testes automatizados completos
❌ Observabilidade (logs estruturados)
❌ Documentação de prompts
❌ Justificativas técnicas

## Tecnologias Usadas no Projeto Base

- .NET 8
- ASP.NET Core Web API
- xUnit para testes
- Swagger/OpenAPI

## Tecnologias Sugeridas (Candidato Escolhe)

- **Banco de Dados:** PostgreSQL, SQL Server, MongoDB
- **Busca:** ElasticSearch, Solr, Azure Cognitive Search
- **Mensageria:** Kafka, RabbitMQ, Azure Service Bus

## Como Testar o Projeto Base

```bash
# Compilar
dotnet build

# Executar testes
dotnet test

# Rodar API
dotnet run --project src/CustomerPlatform.Api

# Acessar Swagger
https://localhost:5001/swagger
```

## Tempo Estimado

**Não especificado intencionalmente** - candidato gerencia seu tempo.

Sugestão implícita no conteúdo: 6-8 horas para implementação completa.

## Pontos de Avaliação Principais

1. **Uso de IA (30%)** - CRÍTICO
2. **Arquitetura (25%)** - Muito importante
3. **Implementação (20%)** - Importante
4. **Testes (10%)** - Importante
5. **Documentação (10%)** - Importante
6. **Observabilidade (5%)** - Desejável

## Contato

Para dúvidas sobre o desafio, consulte os arquivos de documentação fornecidos.


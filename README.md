# Desafio Técnico: Plataforma de Cadastro de Clientes

## 🎯 Bem-vindo!

Este desafio técnico tem como objetivo avaliar sua **proficiência no uso de ferramentas de codificação assistida por IA** (GitHub Copilot, Cursor, ChatGPT, etc.) para desenvolvimento de soluções .NET de qualidade enterprise.

**Importante:** O uso de ferramentas de IA é **OBRIGATÓRIO** e parte fundamental da avaliação. Você deverá documentar todos os prompts utilizados durante o desenvolvimento.

## 📋 Contexto

Você trabalhará em um cenário baseado em problemas reais de negócio: **sistema de cadastro de clientes com deduplicação inteligente e busca avançada**.

Para detalhes completos do problema e requisitos, consulte o arquivo [DESAFIO.md](DESAFIO.md).

## 🛠️ Pré-requisitos

### Obrigatórios
- **.NET 8 SDK** instalado
- **Ferramentas de IA** configuradas (GitHub Copilot, Cursor, ChatGPT ou similar)
- Git para versionamento

### Escolhas Técnicas Obrigatórias

Você **DEVE** escolher e implementar:

1. **Banco de Dados** - relacional ou não relacional
2. **Banco de Pesquisa Probabilística** - para buscas avançadas e deduplicação
3. **Sistema de Mensageria** - para eventos assíncronos

### Sugestões de Tecnologias

Recomendamos (mas não é obrigatório):
- **Banco de Dados:** PostgreSQL, SQL Server, MongoDB
- **Pesquisa Probabilística:** ElasticSearch, Solr, Azure Cognitive Search
- **Mensageria:** Kafka, RabbitMQ, Azure Service Bus, AWS SQS

**Importante:** Justifique tecnicamente suas escolhas no documento de entrega.

## 🚀 Como Executar o Projeto Base

Este repositório contém uma estrutura inicial minimalista para você começar:

```bash
# Clone ou baixe o projeto

# Navegue até a pasta
cd Desafio-IA-DotNet

# Restaure as dependências
dotnet restore

# Execute o projeto
dotnet run --project src/CustomerPlatform.Api
```

### Docker Compose (Opcional)

Fornecemos um arquivo `docker-compose.exemplo.yml` com sugestões de serviços de infraestrutura. Você pode usá-lo como referência:

```bash
# Copie e ajuste conforme necessário
cp docker-compose.exemplo.yml docker-compose.yml

# Suba os serviços
docker-compose up -d
```

## 📝 Regras do Desafio

### 1. Uso Obrigatório de IA

- ✅ **USE** ferramentas de IA para escrever código, criar testes, documentação, etc.
- ✅ **DOCUMENTE** todos os prompts utilizados (veja TEMPLATE_ENTREGA.md)
- ✅ **REFINE** seus prompts e documente as iterações

### 2. Qualidade Técnica

- Implemente as funcionalidades descritas em [DESAFIO.md](DESAFIO.md)
- Siga boas práticas de desenvolvimento .NET
- Escreva testes automatizados
- Implemente observabilidade básica

### 3. Escolhas Tecnológicas

- Escolha as tecnologias que julgar mais adequadas
- **Justifique tecnicamente** cada escolha
- Documente trade-offs e decisões arquiteturais

## 📦 Como Entregar

### 1. Código Fonte

Envie o projeto completo, incluindo:
- Todo o código-fonte
- Testes implementados
- Arquivos de configuração
- Scripts de execução (se houver)

### 2. Documentação

**Obrigatório:**
- `README.md` atualizado com instruções de execução
- `DECISOES_TECNICAS.md` - justificativa de todas as escolhas tecnológicas
- `PROMPTS_UTILIZADOS.md` - lista completa de prompts (use o template fornecido)

### 3. Instruções de Execução

Seu projeto **DEVE** ser executável facilmente. Inclua:
- Passos claros para configurar o ambiente
- Como executar os testes
- Como subir a aplicação
- Como acessar endpoints/documentação da API

### 4. Formato de Entrega

- Repositório Git (GitHub, GitLab, Bitbucket) **OU**
- Arquivo ZIP com todo o projeto

## 🆘 Dúvidas?

- Consulte o [DESAFIO.md](DESAFIO.md) para requisitos detalhados
- Veja o [TEMPLATE_ENTREGA.md](TEMPLATE_ENTREGA.md) para formato de documentação dos prompts
- Use sua criatividade e conhecimento técnico para tomar decisões
- Entre em contato com o gestor da vaga via LinkedIn (Daniel Silva Moreira)

## ⚡ Dica Final

Este desafio avalia sua capacidade de:
- Usar IA como ferramenta de produtividade
- Tomar decisões arquiteturais fundamentadas
- Comunicar escolhas técnicas de forma clara
- Entregar software funcional e bem estruturado

**Boa sorte! 🚀**


# 🎯 Desafio: Sistema de Cadastro de Clientes com Deduplicação Inteligente

## 📖 Contexto de Negócio

Você foi contratado para desenvolver um sistema crítico de **Plataforma de Cadastro de Clientes** para uma empresa de mobilidade. O sistema precisa gerenciar milhões de clientes (Pessoa Física e Pessoa Jurídica), com capacidade de busca avançada e detecção inteligente de duplicações.

### Problema Real

Atualmente, a empresa enfrenta:
- **Cadastros duplicados** causando inconsistências operacionais
- **Buscas lentas** em grandes volumes de dados
- **Falta de rastreabilidade** de alterações nos cadastros
- **Processos manuais** de identificação de duplicatas

## 🎯 Objetivo

Desenvolver uma solução backend robusta que permita:
1. Cadastro eficiente de clientes (PF e PJ)
2. Busca avançada com pesquisa probabilística
3. Deduplicação assíncrona e inteligente
4. Notificação de eventos via mensageria
5. Observabilidade completa do sistema

## 🏗️ Arquitetura Esperada

### Componentes Principais

1. **API Backend**
   - Expor operações de cadastro, consulta e atualização
   - GraphQL OU REST (você escolhe)
   - Validações de negócio

2. **Camada de Persistência**
   - Armazenamento principal dos dados
   - Você escolhe: relacional ou não relacional

3. **Motor de Busca Probabilística**
   - Indexação de clientes para buscas rápidas
   - Suporte a fuzzy search, busca por similaridade
   - Você escolhe a tecnologia

4. **Sistema de Mensageria**
   - Publicação de eventos de domínio
   - Processamento assíncrono
   - Você escolhe a tecnologia

5. **Processamento Assíncrono (Opcional)**
   - Worker Service para deduplicação
   - Consumo de eventos
   - Diferencial técnico

## 📋 Requisitos Funcionais

### 1. Cadastro de Clientes

#### Cliente Pessoa Física (PF)
- Nome completo
- CPF (único)
- Email
- Telefone
- Data de nascimento
- Endereço (logradouro, número, complemento, CEP, cidade, estado)

#### Cliente Pessoa Jurídica (PJ)
- Razão social
- Nome fantasia
- CNPJ (único)
- Email corporativo
- Telefone
- Endereço (logradouro, número, complemento, CEP, cidade, estado)

**Regras:**
- CPF e CNPJ devem ser únicos no sistema
- Validação de formato de documentos
- Email deve ser validado
- Todos os campos obrigatórios devem ser informados

### 2. Busca Avançada

Implementar busca com motor de pesquisa probabilística por:
- Nome completo / Razão social (com fuzzy search)
- CPF / CNPJ (busca exata)
- Email (busca parcial)
- Telefone (busca parcial)
- Combinações de filtros

**Requisitos:**
- Suportar busca com erros de digitação
- Retornar resultados ordenados por relevância
- Paginação de resultados
- Performance para grandes volumes

### 3. Deduplicação Inteligente

Implementar lógica de detecção de clientes duplicados baseada em:
- Similaridade de nomes (Levenshtein, Soundex, ou similar)
- Documentos similares (com dígitos trocados)
- Emails similares
- Telefones iguais
- Detecção assíncrona (não bloquear o cadastro)
- Processar via eventos de mensageria
- Gerar lista de possíveis duplicatas com score de similaridade (realizar instert da analise em SuspeitaDuplicidade. Repositorio ja encontra-se criado)


### 4. Eventos de Domínio

Publicar eventos via mensageria para:
- `ClienteCriado` - quando um novo cliente é cadastrado
- `ClienteAtualizado` - quando dados são alterados
- `DuplicataSuspeita` - quando duplicata é detectada

**Estrutura dos Eventos:**
```json
{
  "eventId": "uuid",
  "eventType": "ClienteCriado",
  "timestamp": "2026-01-02T10:30:00Z",
  "data": {
    "clienteId": "uuid",
    "tipoCliente": "PF",
    "documento": "12345678900",
    "nome": "João Silva"
  }
}
```

## 🔧 Requisitos Técnicos

### Obrigatórios

1. ✅ **API Funcional** (GraphQL ou REST - você escolhe)
2. ✅ **Banco de Dados** (relacional ou não relacional - você escolhe e justifica)
3. ✅ **Banco de Pesquisa Probabilística** (você escolhe e justifica)
4. ✅ **Sistema de Mensageria** (você escolhe e justifica)
5. ✅ **Lógica de Deduplicação** (algoritmo a definir)
6. ✅ **Forma de Executar o Projeto** (Docker Compose, scripts, ou outro)
7. ✅ **Testes Automatizados** (unitários + integração)
8. ✅ **Observabilidade Básica** (logs estruturados + health checks)
9. ✅ **Documento com Justificativa Técnica** de todas as escolhas

### Sugestões de Tecnologias

**Recomendamos (mas você pode escolher outras):**

- **Banco de Dados:** PostgreSQL, SQL Server, MongoDB
- **Pesquisa Probabilística:** ElasticSearch, Solr, Azure Cognitive Search
- **Mensageria:** Kafka, RabbitMQ, Azure Service Bus, AWS SQS

## 🌟 Diferenciais (Opcional)

Pontos extras serão dados para:

### Arquitetura e Design
- ⭐ Worker Service separado para processamento assíncrono
- ⭐ Padrões avançados (CQRS, Repository, Unit of Work)
- ⭐ Separação clara de responsabilidades (Clean Architecture)
- ⭐ Injeção de dependências bem estruturada

### Deduplicação Sofisticada
- ⭐ Múltiplos algoritmos (Levenshtein, Soundex, Jaro-Winkler)
- ⭐ Score ponderado de similaridade
- ⭐ Machine Learning para detecção (bonus!)

### Qualidade de Código
- ⭐ Cobertura de testes > 90%
- ⭐ Testes de integração com containers (Testcontainers)
- ⭐ Testes de performance

### Observabilidade
- ⭐ Logs estruturados com contexto
- ⭐ Métricas customizadas
- ⭐ Distributed tracing
- ⭐ Health checks detalhados

### API
- ⭐ Documentação interativa (Swagger/GraphQL Playground)
- ⭐ Versionamento de API
- ⭐ Rate limiting
- ⭐ Autenticação/Autorização

### Resiliência
- ⭐ Retry policies (Polly)
- ⭐ Circuit breaker
- ⭐ Timeout policies
- ⭐ Graceful shutdown

## 📦 Estrutura de Entrega

### 1. Código Funcional

O projeto deve:
- ✅ Compilar sem erros
- ✅ Executar com instruções claras
- ✅ Ter todos os requisitos obrigatórios implementados

### 2. Documentação

Você deve entregar:
- `README.md` - instruções de execução
- `DECISOES_TECNICAS.md` - justificativa de escolhas tecnológicas
- `PROMPTS_UTILIZADOS.md` - todos os prompts de IA utilizados (use o template)

### 3. Infraestrutura

Forneça:
- Scripts de setup (se necessário)
- Configuração de ambiente (docker-compose, kubernetes, etc)
- Seed de dados (opcional, mas útil para demonstração)

## 🎯 Cenários de Teste

Para facilitar a avaliação, implemente pelo menos estes cenários:

### Cenário 1: Cadastro Simples
```
POST /clientes (ou mutation no GraphQL)
{
  "tipo": "PF",
  "nome": "João Silva",
  "cpf": "12345678900",
  "email": "joao@email.com",
  "telefone": "11999999999"
}
Esperado: Cliente cadastrado, evento publicado
```

### Cenário 2: Busca com Fuzzy Search
```
GET /clientes?nome=Joao Silv (ou query no GraphQL)
Esperado: Retornar "João Silva" mesmo com erro de digitação
```

### Cenário 3: Deduplicação
```
1. Cadastrar "João Silva" - CPF 12345678900
2. Cadastrar "Joao Silva" - CPF 12345678901 (documento diferente)
Esperado: Sistema detecta duplicata suspeita e gera evento
```

## 💡 Dicas

1. **Comece Simples**: Implemente o mínimo viável primeiro, depois adicione diferenciais
2. **Use IA Estrategicamente**: Documente como a IA ajudou em decisões complexas
3. **Justifique Tudo**: Cada escolha técnica deve ter um "porquê"
4. **Teste Constantemente**: Garanta que tudo funciona antes de entregar
5. **Documente Bem**: Um bom README vale ouro

**Boa sorte! 🚀**


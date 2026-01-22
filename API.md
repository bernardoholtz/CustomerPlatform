# Customer Platform — Documentação da API

API REST para cadastro, edição, busca avançada e listagem de duplicatas de clientes (pessoa física e jurídica). Utiliza Elasticsearch para busca, RabbitMQ para mensageria e PostgreSQL para persistência.

---

## Índice

1. [Visão geral](#visão-geral)
2. [Base URL](#base-url)
3. [Endpoints](#endpoints)
   - [Health Check](#1-health-check)
   - [Cadastrar cliente](#2-cadastrar-cliente)
   - [Editar cliente](#3-editar-cliente)
   - [Busca avançada](#4-busca-avançada)
   - [Listar duplicatas](#5-listar-duplicatas)
4. [Modelos de dados](#modelos-de-dados)
5. [Códigos de status HTTP](#códigos-de-status-http)
6. [Exemplos de uso](#exemplos-de-uso)

---

## Visão geral

| Recurso        | Descrição                                                                 |
|----------------|---------------------------------------------------------------------------|
| **Cadastro**   | Criação de clientes PF ou PJ com validação de CPF/CNPJ                    |
| **Edição**     | Atualização de clientes existentes                                        |
| **Busca**      | Busca avançada com filtros (nome, CPF, CNPJ, email, telefone) e paginação |
| **Duplicatas** | Listagem de suspeitas de duplicidade em um período                        |
| **Health**     | Verificação de saúde do serviço                                           |

---

## Base URL

```
http://localhost:5000
```

Em produção, utilize a URL configurada no ambiente.

**Swagger UI:** `http://localhost:5000/swagger/index.html`


---

## Endpoints

### 1. Health Check

Verifica se a API está em execução.

**`GET`** `/api/health`

#### Resposta de sucesso

**Status:** `200 OK`

```json
{
  "status": "Healthy",
  "timestamp": "2026-01-22T14:30:00.000Z",
  "service": "CustomerPlatform.Api"
}
```

---

### 2. Cadastrar cliente

Cria um novo cliente (pessoa física ou jurídica). Os dados são persistidos no PostgreSQL, indexados no Elasticsearch e publicados no RabbitMQ (`ClienteCriado`).

**`POST`** `/api/customer`

#### Headers

| Header          | Valor            |
|-----------------|------------------|
| `Content-Type`  | `application/json` |

#### Corpo da requisição

**Pessoa física (`TipoCliente: 0`):**

| Campo           | Tipo     | Obrigatório | Descrição                    |
|-----------------|----------|-------------|------------------------------|
| `tipoCliente`   | `int`    | Sim         | `0` = PessoaFisica           |
| `email`         | `string` | Sim         | E-mail válido                |
| `telefone`      | `string` | Sim         | Telefone                     |
| `nome`          | `string` | Sim         | Nome completo                |
| `cpf`           | `string` | Sim         | CPF (apenas dígitos ou formatado) |
| `dataNascimento`| `string` | Não         | ISO 8601, ex: `1990-05-15`   |
| `endereco`      | `object` | Sim         | Ver [Endereço](#endereço)    |

**Pessoa jurídica (`TipoCliente: 1`):**

| Campo           | Tipo     | Obrigatório | Descrição                    |
|-----------------|----------|-------------|------------------------------|
| `tipoCliente`   | `int`    | Sim         | `1` = PessoaJuridica         |
| `email`         | `string` | Sim         | E-mail válido                |
| `telefone`      | `string` | Sim         | Telefone                     |
| `razaoSocial`   | `string` | Sim         | Razão social                 |
| `nomeFantasia`  | `string` | Não         | Nome fantasia                |
| `cnpj`          | `string` | Sim         | CNPJ (apenas dígitos ou formatado) |
| `endereco`      | `object` | Sim         | Ver [Endereço](#endereço)    |

**Endereço:**

| Campo        | Tipo     | Obrigatório | Descrição        |
|--------------|----------|-------------|------------------|
| `logradouro` | `string` | Sim         | Rua, avenida etc |
| `numero`     | `string` | Sim         | Número           |
| `cep`        | `string` | Sim         | CEP              |
| `cidade`     | `string` | Sim         | Cidade           |
| `estado`     | `string` | Sim         | UF (2 caracteres)|
| `complemento`| `string` | Não         | Complemento      |

#### Resposta de sucesso

**Status:** `201 Created`

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

#### Respostas de erro

| Status | Descrição                          |
|--------|------------------------------------|
| `400`  | Dados inválidos (ex.: CPF/CNPJ inválido, e-mail inválido) |
| `500`  | Erro interno do servidor           |

---

### 3. Editar cliente

Atualiza um cliente existente. Os dados são atualizados no PostgreSQL, no Elasticsearch e um evento `ClienteAtualizado` é publicado no RabbitMQ.

**`PUT`** `/api/customer`

#### Headers

| Header          | Valor            |
|-----------------|------------------|
| `Content-Type`  | `application/json` |

#### Corpo da requisição

Mesma estrutura do [Cadastrar cliente](#2-cadastrar-cliente), com o campo adicional:

| Campo         | Tipo   | Obrigatório | Descrição        |
|---------------|--------|-------------|------------------|
| `id`          | `string` (UUID) | Sim | ID do cliente a editar |

Para PF, `dataNascimento` é obrigatório na edição.

#### Resposta de sucesso

**Status:** `201 Created`

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

#### Respostas de erro

| Status | Descrição                                      |
|--------|------------------------------------------------|
| `400`  | Dados inválidos ou cliente não encontrado      |
| `500`  | Erro interno do servidor                       |

---

### 4. Busca avançada

Busca clientes com filtros opcionais e paginação. A busca é feita no Elasticsearch, com suporte a:

- **Nome / Razão social / Nome fantasia:** fuzzy search e match parcial
- **CPF / CNPJ:** busca exata (apenas dígitos)
- **E-mail:** match de frase (exato ou parcial, case-insensitive)
- **Telefone:** busca parcial

Todos os filtros enviados são combinados com **AND**. Sem filtros, retorna todos os documentos (respeitando paginação).

**`POST`** `/api/customer/search`

#### Headers

| Header          | Valor            |
|-----------------|------------------|
| `Content-Type`  | `application/json` |

#### Corpo da requisição

| Campo      | Tipo   | Obrigatório | Descrição                          |
|------------|--------|-------------|------------------------------------|
| `nome`     | `string` | Não      | Nome, razão social ou nome fantasia|
| `cpf`      | `string` | Não      | CPF (com ou sem formatação)        |
| `cnpj`     | `string` | Não      | CNPJ (com ou sem formatação)       |
| `email`    | `string` | Não      | E-mail (exato ou parcial)          |
| `telefone` | `string` | Não      | Telefone (parcial)                 |
| `page`     | `int`    | Não      | Página (default: `1`)              |
| `pageSize` | `int`    | Não      | Itens por página (default: `10`, máx: `100`) |

#### Resposta de sucesso

**Status:** `200 OK`

```json
{
  "results": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "tipoCliente": "PessoaFisica",
      "nome": "João da Silva",
      "razaoSocial": null,
      "nomeFantasia": null,
      "documento": "12345678901",
      "email": "joao@example.com",
      "telefone": "11999999999",
      "dataCriacao": "2026-01-15T10:00:00Z",
      "dataAtualizacao": "2026-01-20T14:30:00Z",
      "score": 1.5,
      "endereco": {
        "logradouro": "Rua das Flores",
        "numero": "100",
        "complemento": "Apto 42",
        "cidade": "São Paulo",
        "estado": "SP",
        "cep": "01234567"
      }
    }
  ],
  "total": 1,
  "page": 1,
  "pageSize": 10
}
```

| Campo    | Tipo     | Descrição                                |
|----------|----------|------------------------------------------|
| `results`| `array`  | Lista de clientes encontrados            |
| `total`  | `int`    | Total de registros que atendem aos filtros |
| `page`   | `int`    | Página atual                             |
| `pageSize` | `int`  | Tamanho da página                        |

Os itens em `results` seguem o modelo [CustomerSearchResult](#customersearchresult). O campo `score` indica relevância da busca.

#### Respostas de erro

| Status | Descrição                |
|--------|--------------------------|
| `400`  | Body nulo ou inválido    |
| `500`  | Erro interno do servidor |

---

### 5. Listar duplicatas

Retorna suspeitas de duplicidade detectadas em um período. Os parâmetros são informados na query string.

**`GET`** `/api/customer/duplicates`

#### Query parameters

| Parâmetro | Tipo   | Obrigatório | Formato     | Descrição        |
|-----------|--------|-------------|-------------|------------------|
| `DataIni` | `string` | Sim       | `yyyy-MM-dd` | Data inicial     |
| `DataFim` | `string` | Sim       | `yyyy-MM-dd` | Data final       |

Exemplo: `/api/customer/duplicates?DataIni=2026-01-01&DataFim=2026-01-31`

#### Resposta de sucesso

**Status:** `200 OK`

```json
[
  {
    "id": "7b2e1c9a-4d5f-4a1b-9e8c-1234567890ab",
    "idOriginal": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "customerOriginal": { ... },
    "idSuspeito": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    "customerSuspeito": { ... },
    "score": 0.92,
    "detalhesSimilaridade": "Alta similaridade em nome e documento",
    "dataDeteccao": "2026-01-22T12:00:00Z"
  }
]
```

Cada item é uma **SuspeitaDuplicidade**: par original vs. suspeito, `score` de similaridade e `detalhesSimilaridade`. Os objetos `customerOriginal` e `customerSuspeito` contêm os dados completos do cliente.

#### Respostas de erro

| Status | Descrição                |
|--------|--------------------------|
| `500`  | Erro interno do servidor |

---

## Modelos de dados

### Endereço

| Campo        | Tipo     |
|--------------|----------|
| `logradouro` | `string` |
| `numero`     | `string` |
| `cep`        | `string` |
| `cidade`     | `string` |
| `estado`     | `string` |
| `complemento`| `string` (opcional) |

### TipoCliente (enum)

| Valor | Nome           |
|-------|----------------|
| `0`   | PessoaFisica   |
| `1`   | PessoaJuridica |

### CustomerSearchResult

| Campo             | Tipo     | Descrição                    |
|-------------------|----------|------------------------------|
| `id`              | `uuid`   | ID do cliente                |
| `tipoCliente`     | `string` | `"PessoaFisica"` ou `"PessoaJuridica"` |
| `nome`            | `string` | Nome ou nome fantasia        |
| `razaoSocial`     | `string` | Razão social (PJ)            |
| `nomeFantasia`    | `string` | Nome fantasia (PJ)           |
| `documento`       | `string` | CPF ou CNPJ                  |
| `email`           | `string` | E-mail                       |
| `telefone`        | `string` | Telefone                     |
| `dataCriacao`     | `datetime` | Data de criação            |
| `dataAtualizacao` | `datetime` | Última atualização         |
| `score`           | `double` | Relevância na busca          |
| `endereco`        | `object` | Ver [Endereço](#endereço)    |

---

## Códigos de status HTTP

| Código | Significado        |
|--------|--------------------|
| `200`  | OK                 |
| `201`  | Created            |
| `400`  | Bad Request        |
| `500`  | Internal Server Error |

Em caso de erro, o corpo geralmente retorna `{ "error": "mensagem" }`.

---

## Exemplos de uso

### Cadastrar pessoa física

```bash
curl -X POST "http://localhost:5000/api/customer" \
  -H "Content-Type: application/json" \
  -d '{
    "tipoCliente": 0,
    "email": "joao@example.com",
    "telefone": "11999998888",
    "nome": "João da Silva",
    "cpf": "123.456.789-01",
    "dataNascimento": "1990-05-15",
    "endereco": {
      "logradouro": "Rua das Flores",
      "numero": "100",
      "cep": "01234567",
      "cidade": "São Paulo",
      "estado": "SP"
    }
  }'
```

### Cadastrar pessoa jurídica

```bash
curl -X POST "http://localhost:5000/api/customer" \
  -H "Content-Type: application/json" \
  -d '{
    "tipoCliente": 1,
    "email": "contato@empresa.com",
    "telefone": "1133334444",
    "razaoSocial": "Empresa XYZ Ltda",
    "nomeFantasia": "XYZ",
    "cnpj": "12.345.678/0001-90",
    "endereco": {
      "logradouro": "Av. Paulista",
      "numero": "1000",
      "cep": "01310100",
      "cidade": "São Paulo",
      "estado": "SP"
    }
  }'
```

### Busca avançada (nome + e-mail, página 1)

```bash
curl -X POST "http://localhost:5000/api/customer/search" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "João",
    "email": "joao@example.com",
    "page": 1,
    "pageSize": 10
  }'
```

### Busca apenas por CPF

```bash
curl -X POST "http://localhost:5000/api/customer/search" \
  -H "Content-Type: application/json" \
  -d '{
    "cpf": "12345678901",
    "page": 1,
    "pageSize": 10
  }'
```

### Listar duplicatas (janeiro/2026)

```bash
curl "http://localhost:5000/api/customer/duplicates?DataIni=2026-01-01&DataFim=2026-01-31"
```

---

## Observabilidade

- **Métricas (Prometheus):** `GET /metrics`
- **Logs:** Enviados ao Loki (Serilog) com `job=customer-platform`
- **Dashboards:** Grafana em `http://localhost:3000` (quando stack de observabilidade está ativa)

---

*Documentação gerada para a Customer Platform API. Para dúvidas ou sugestões, consulte o repositório do projeto.*

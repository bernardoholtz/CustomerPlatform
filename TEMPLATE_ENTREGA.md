# 📝 Template de Entrega - Documentação de Prompts

## Instruções de Uso

Este template deve ser usado para documentar **TODOS** os prompts utilizados durante o desenvolvimento do desafio.

## Formato de Documentação

Para cada prompt utilizado, preencha as informações abaixo:

---

## Prompt #[NÚMERO]

### 🎯 Contexto/Objetivo
**Descreva o que você estava tentando alcançar com este prompt.**

Exemplo: "Gerar a estrutura inicial da entidade Cliente com validações"

### 🤖 Ferramenta Utilizada
**Qual ferramenta de IA você usou?**

Opções: GitHub Copilot, Cursor, ChatGPT, Claude, Gemini, Copilot Chat, outro

### 💬 Prompt Utilizado
**Cole aqui o prompt exato que você utilizou.**

```
[COLE SEU PROMPT AQUI]
```

### 📊 Avaliação Pessoal
**Como você avalia o resultado?**

- [ ] Excelente - usei diretamente sem modificações
- [ ] Bom - fiz pequenos ajustes
- [ ] Regular - precisei modificar bastante
- [ ] Ruim - tive que refazer manualmente

---

## 📚 Exemplos de Prompts Bem Documentados

### Exemplo 1: Geração de Código

## Prompt #1

### 🎯 Contexto/Objetivo
Criar a entidade Cliente (Pessoa Física) com todas as propriedades necessárias e validações de CPF.

### 🤖 Ferramenta Utilizada
GitHub Copilot

### 💬 Prompt Utilizado
```
// Criar entidade ClientePessoaFisica
// Propriedades: Id (Guid), Nome, CPF (string, único), Email, Telefone, DataNascimento
// Incluir validações: CPF deve ter 11 dígitos, Email formato válido
// Incluir método para validar CPF
```

### 📊 Avaliação Pessoal
- [x] Bom - fiz pequenos ajustes

---

### Exemplo 2: Decisão Arquitetural

## Prompt #5

### 🎯 Contexto/Objetivo
Decidir qual banco de dados usar para armazenamento principal dos clientes, considerando os requisitos do projeto.

### 🤖 Ferramenta Utilizada
ChatGPT

### 💬 Prompt Utilizado
```
Estou desenvolvendo um sistema de cadastro de clientes para uma empresa de mobilidade.

Requisitos:
- Milhões de registros
- Consistência transacional importante (evitar duplicatas)
- Queries complexas (joins entre tabelas)
- Relacionamentos entre entidades (Cliente, Endereço, Histórico)
- Alta disponibilidade

Preciso escolher entre PostgreSQL, SQL Server, MongoDB.

Quais os trade-offs de cada opção? Qual você recomendaria e por quê?
```

### 📊 Avaliação Pessoal
- [x] Excelente - usei diretamente sem modificações

---

### Exemplo 3: Geração de Testes

## Prompt #12

### 🎯 Contexto/Objetivo
Criar testes unitários para o serviço de deduplicação de clientes.

### 🤖 Ferramenta Utilizada
Cursor

### 💬 Prompt Utilizado
```
Gerar testes unitários usando xUnit para a classe DeduplicacaoService.

Cenários a testar:
1. Dois clientes com nomes idênticos devem ser detectados como duplicatas
2. Dois clientes com nomes similares (Levenshtein < 3) devem ser detectados
3. Dois clientes com CPFs diferentes mas nomes similares: suspeita
4. Clientes completamente diferentes: sem duplicata

Use Moq para mockar dependências do repositório.
Siga padrão AAA (Arrange, Act, Assert).
```

### 📊 Avaliação Pessoal
- [x] Excelente - usei diretamente sem modificações

---

## 📋 Seu Template Começa Aqui

### Instruções Finais

1. **Copie este template** para um arquivo chamado `PROMPTS_UTILIZADOS.md` na raiz do seu projeto
2. **Documente TODOS os prompts** que você usar durante o desenvolvimento
3. **Seja honesto** - queremos ver seu processo real, incluindo erros e refinamentos
4. **Numere sequencialmente** - isso nos ajuda a entender sua jornada de desenvolvimento
5. **Inclua variedade** - mostre prompts para código, testes, documentação, decisões arquiteturais

### Categorias Sugeridas de Prompts

Organize seus prompts por categoria (opcional, mas recomendado):

- **Estrutura e Arquitetura** - decisões iniciais de design
- **Entidades e Modelos** - criação das classes de domínio
- **Lógica de Negócio** - implementação de regras
- **Persistência** - integração com banco de dados
- **API** - criação de endpoints/resolvers
- **Mensageria** - integração com sistema de mensageria
- **Busca Probabilística** - integração com motor de busca
- **Deduplicação** - implementação dos algoritmos
- **Testes** - criação de testes unitários e de integração
- **Documentação** - criação de READMEs e docs
- **Infraestrutura** - Docker, configurações, scripts

### Dicas para Bons Prompts

✅ **Seja específico** - quanto mais contexto, melhor o resultado
✅ **Use exemplos** - mostre o que você espera
✅ **Defina constraints** - especifique limitações e requisitos
✅ **Itere** - refine o prompt se o resultado não for ideal
✅ **Documente aprendizados** - o que funcionou e o que não funcionou

---

## 🎯 Começe Agora!

Use a seção abaixo para documentar seus prompts:

---

## Prompt #1

### 🎯 Contexto/Objetivo


### 🤖 Ferramenta Utilizada


### 💬 Prompt Utilizado
```

```

### ✅ Resultado Obtido


### 🔄 Refinamentos Necessários


### 📊 Avaliação Pessoal
- [ ] Excelente - usei diretamente sem modificações
- [ ] Bom - fiz pequenos ajustes
- [ ] Regular - precisei modificar bastante
- [ ] Ruim - tive que refazer manualmente

---

## Prompt #2

[Continue documentando...]

---

**Lembre-se: A qualidade da sua documentação de prompts é tão importante quanto o código que você entrega! 🚀**


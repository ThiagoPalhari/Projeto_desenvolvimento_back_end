# Lanchonetes API

API .NET 8 para gestão de rede de lanchonetes com suporte a usuários, autenticação, unidades, produtos, pedidos, estoque, fidelidade, pagamentos mock, promoções, consentimento e relatórios.

## Funcionalidades principais

- Cadastro e autenticação de usuários
- Gestão de perfis e permissões
- Cadastro de unidades e produtos
- Consulta de cardápio por unidade
- Criação, consulta e atualização de pedidos
- Controle de estoque por unidade
- Fidelidade com pontos
- Consentimento do cliente
- Promoções e campanhas
- Processamento mock de pagamentos
- Auditoria e relatórios
- Swagger/OpenAPI

## Estrutura do projeto

- Lanchonetes.Api: controllers, autenticação, Swagger e middleware
- Lanchonetes.Application: contratos e DTOs
- Lanchonetes.Domain: entidades e enums do domínio
- Lanchonetes.Infrastructure: serviços, persistência e autenticação

## Requisitos

- .NET 8 SDK
- PostgreSQL
- Docker

## Configuração

Configure a connection string e as chaves JWT em `src/Lanchonetes.Api/appsettings.json`.

Exemplo de estrutura:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=lanchonetes;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "Key": "sua-chave-secreta-muito-forte",
    "Issuer": "LanchonetesApi",
    "Audience": "LanchonetesClient",
    "ExpirationMinutes": 60
  }
}
```

## Execução

```bash
dotnet restore
dotnet build
dotnet run --project src/Lanchonetes.Api
```

## Banco de dados

Para subir o PostgreSQL localmente com Docker:

```bash
docker compose up -d
```

## Acesso à documentação

Após iniciar a API, acesse:

- Swagger UI: http://localhost:5077/swagger

## Testes e evidência

Esta API fornece um conjunto de cenários de teste para validação manual e automatizada. A coleção Postman/Insomnia está disponível em:

- `docs/postman/Lanchonetes_API_Collection.json`

### Ordem sugerida de execução

1. Subir o banco com Docker (`docker compose up -d`)
2. Ajustar os valores de conexão e JWT em `src/Lanchonetes.Api/appsettings.Development.json`
3. Iniciar a API (`dotnet run --project src/Lanchonetes.Api`)
4. Executar os testes de autenticação (`Auth`)
5. Criar unidade (`Unidades`)
6. Criar produto e movimentação de estoque (`Produtos e Estoque`)
7. Criar pedido (`Pedidos`)
8. Processar pagamento (`Pagamentos`)
9. Validar consentimento e fidelidade (`Consents e Fidelidade`)
10. Validar promoções e relatórios (`Promoções e Relatórios`)
11. Consultar auditoria (`Auditoria`)

### Variáveis de ambiente da coleção

A coleção usa as seguintes variáveis:

- `baseUrl`: URL da API, normalmente `http://localhost:5080`
- `token`: JWT gerado no login
- `adminEmail`: e-mail do usuário administrador
- `customerEmail`: e-mail do cliente
- `unitId`: identificador da unidade criada
- `productId`: identificador do produto criado
- `orderId`: identificador do pedido criado
- `paymentId`: identificador do pagamento criado
- `customerId`: identificador do cliente autenticado

### Observações de uso

- O token JWT deve ser copiado do retorno do login e colado na variável `token` da coleção.
- Para a API funcionar corretamente, precisa haver pelo menos uma role/usuário cadastrado e o banco PostgreSQL em execução.
- O fluxo recomendado inclui login do cliente e do administrador para validar autenticação e autorização.
- Em caso de erro de banco ou conexão, confirme se o Docker está em execução e se a string de conexão está correta.

## Observações


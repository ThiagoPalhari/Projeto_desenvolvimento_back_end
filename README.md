# Lanchonetes API

Estrutura inicial de uma API .NET 8 para uma rede de lanchonetes com canais APP, TOTEM, BALCAO, PICKUP e WEB.

## Projetos

- Lanchonetes.Api: HTTP, autenticação, autorização, Swagger e middleware.
- Lanchonetes.Application: DTOs e contratos de aplicação.
- Lanchonetes.Domain: entidades e enums do domínio.
- Lanchonetes.Infrastructure: persistência, segurança e implementações dos serviços.

## Configuração

Altere a connection string PostgreSQL e a chave JWT em `src/Lanchonetes.Api/appsettings.json`.

## Execução

```bash
dotnet restore
dotnet build
dotnet run --project src/Lanchonetes.Api
```

Os endpoints foram criados como esqueleto. As implementações dos serviços estão com `NotImplementedException` para serem desenvolvidas.

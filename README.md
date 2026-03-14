# PaymentsAPI

Microsserviço responsável pelo processamento (simulado) de pagamentos na plataforma FIAP Cloud Games.

## Responsabilidades

- Consumir o evento `OrderPlacedEvent` enviado pelo CatalogAPI
- Registrar o pagamento no banco de dados
- Simular o processamento financeiro (sempre retorna `Approved`)
- Publicar o evento `PaymentProcessedEvent` com o resultado

## Fluxo de eventos

```
CatalogAPI → OrderPlacedEvent → PaymentsAPI → PaymentProcessedEvent → CatalogAPI + NotificationsAPI
```

## Variáveis de ambiente

| Variável | Descrição | Exemplo |
|---|---|---|
| `ConnectionStrings__PaymentsDB` | Connection string do SQL Server | `Server=sqlserver;Database=PaymentsDB;User Id=sa;Password=...` |
| `RabbitMQ__Host` | Host do broker RabbitMQ | `rabbitmq` |
| `RabbitMQ__VHost` | Virtual host do RabbitMQ | `/` |
| `RabbitMQ__Username` | Usuário do RabbitMQ | `guest` |
| `RabbitMQ__Password` | Senha do RabbitMQ | `guest` |
| `ASPNETCORE_ENVIRONMENT` | Ambiente da aplicação | `Production` |

> **Nota:** Este serviço não expõe endpoints HTTP públicos nem exige autenticação JWT. Toda comunicação ocorre via mensageria (RabbitMQ).

## Executar localmente

```bash
# A partir da raiz da solução
docker-compose up --build paymentsapi
```

Ou via Docker isolado:

```bash
docker build -f PaymentsAPI/PaymentsAPI.Api/Dockerfile -t paymentsapi:latest .
docker run -p 5003:8080 paymentsapi:latest
```

Swagger disponível em: `http://localhost:5003/swagger` (apenas para health check / documentação interna)

## Observações

- O processamento é **simulado**: todos os pagamentos são aprovados automaticamente (`Status = "Approved"`).
- O serviço **não possui autenticação HTTP** — é um serviço interno orientado a eventos.
- Cada pagamento é persistido em `PaymentsDB` para auditoria.

## Tecnologias

- .NET 9
- Entity Framework Core 9 + SQL Server
- MassTransit 8 + RabbitMQ

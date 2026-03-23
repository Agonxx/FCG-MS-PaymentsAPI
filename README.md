# FCG-MS-PaymentsAPI

Microsserviço responsável pelo processamento (simulado) de pagamentos na plataforma **FIAP Cloud Games**.

## Responsabilidades

- Consumir o evento `OrderPlacedEvent` enviado pelo CatalogAPI
- Registrar o pagamento no banco de dados
- Simular processamento financeiro (sempre aprova)
- Publicar `PaymentProcessedEvent` com o resultado para CatalogAPI e NotificationsAPI

## Fluxo de eventos

```
CatalogAPI  → [OrderPlacedEvent]      → PaymentsAPI
PaymentsAPI → [PaymentProcessedEvent] → CatalogAPI + NotificationsAPI
```

---

## Pré-requisitos

| Ferramenta | Versão mínima |
|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/9.0) | 9.0 |
| [Docker Desktop](https://www.docker.com/products/docker-desktop/) | 24+ |
| SQL Server | 2019+ (via Docker) |
| RabbitMQ | 3.13+ (via Docker) |

---

## Variáveis de ambiente

| Variável | Descrição | Valor padrão (dev) |
|---|---|---|
| `ConnectionStrings__PaymentsDB` | Connection string do SQL Server | `Server=localhost,1433;Database=PaymentsDB;User Id=sa;Password=Sa12345678!;TrustServerCertificate=True` |
| `RabbitMQ__Host` | Host do RabbitMQ | `localhost` |
| `RabbitMQ__VHost` | Virtual host | `/` |
| `RabbitMQ__Username` | Usuário | `guest` |
| `RabbitMQ__Password` | Senha | `guest` |
| `ASPNETCORE_ENVIRONMENT` | Ambiente | `Development` |

> Este serviço **não exige JWT** — toda comunicação ocorre via mensageria.

---

## Como executar

### 1. Subir infraestrutura (SQL Server + RabbitMQ)

```bash
# SQL Server (pule se já estiver rodando)
docker run -d --name sqlserver \
  -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Sa12345678!" \
  -p 1433:1433 \
  mcr.microsoft.com/mssql/server:2019-latest

# RabbitMQ com painel de gestão (acesso em http://localhost:15672)
docker run -d --name rabbitmq \
  -p 5672:5672 -p 15672:15672 \
  rabbitmq:3-management
```

### 2. Executar via .NET CLI (desenvolvimento)

```bash
cd PaymentsAPI.Api
dotnet run
```

- API: `http://localhost:5003`
- Swagger: `http://localhost:5003/swagger`

### 3. Executar via Docker (imagem isolada)

```bash
# Build a partir da raiz do repositório
docker build -t fcg-paymentsapi:latest .

# Run
docker run -d --name paymentsapi -p 5003:8080 \
  -e "ConnectionStrings__PaymentsDB=Server=host.docker.internal,1433;Database=PaymentsDB;User Id=sa;Password=Sa12345678!;TrustServerCertificate=True" \
  -e "RabbitMQ__Host=host.docker.internal" \
  -e "RabbitMQ__Username=guest" \
  -e "RabbitMQ__Password=guest" \
  fcg-paymentsapi:latest
```

### 4. Executar via Kubernetes

```bash
kubectl apply -f k8s/
kubectl get pods -n fiapcloudgames
kubectl logs -l app=paymentsapi -n fiapcloudgames
```

---

## Verificar funcionamento

O PaymentsAPI não expõe endpoints de negócio publicamente. Para verificar que está processando eventos:

```bash
# Ver logs em tempo real (Docker)
docker logs -f paymentsapi

# Saída esperada ao processar um pagamento:
# info: Pagamento registrado | OrderId: ... | UserId: 1 | JogoId: 3 | Status: Approved
```

Acesse o painel do RabbitMQ em `http://localhost:15672` (guest/guest) para monitorar as filas.

---

## Executar testes

```bash
cd PaymentsAPI.Tests
dotnet test
```

---

## Estrutura do projeto

```
FCG-MS-PaymentsAPI/
├── Dockerfile
├── k8s/
├── PaymentsAPI.Api/           # Program.cs, Extensions, Consumer (OrderPlaced)
├── PaymentsAPI.Application/   # Services
├── PaymentsAPI.Domain/        # Entities, Interfaces, Events (inline)
├── PaymentsAPI.Infrastructure/ # EF Core, Repositories
└── PaymentsAPI.Tests/         # xUnit + Moq
```

---

## Observações

- O processamento é **simulado**: todos os pagamentos são aprovados com `Status = "Approved"`.
- Não possui autenticação HTTP — é um serviço interno orientado a eventos.
- Cada pagamento é persistido em `PaymentsDB` para auditoria.

---

## Tecnologias

- .NET 9 / ASP.NET Core 9
- Entity Framework Core 9 + SQL Server
- MassTransit 8.3.6 + RabbitMQ
- xUnit + Moq

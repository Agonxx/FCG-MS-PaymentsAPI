namespace Shared.Contracts.Events;

public record PaymentProcessedEvent(
    Guid OrderId,
    int UserId,
    int JogoId,
    string Titulo,
    decimal Preco,
    string Status,
    DateTime ProcessadoEm
);

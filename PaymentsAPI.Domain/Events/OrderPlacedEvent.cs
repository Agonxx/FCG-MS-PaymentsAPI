namespace Shared.Contracts.Events;

public record OrderPlacedEvent(
    Guid OrderId,
    int UserId,
    int JogoId,
    string Titulo,
    decimal Preco,
    DateTime CriadoEm
);

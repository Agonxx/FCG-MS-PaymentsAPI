using PaymentsAPI.Domain.Entities;

namespace PaymentsAPI.Domain.Interfaces.Services
{
    public interface IPagamentoService
    {
        Task<Pagamento> ProcessarAsync(Guid orderId, int userId, int jogoId, string titulo, decimal preco);
        Task<List<Pagamento>> GetAllAsync();
    }
}

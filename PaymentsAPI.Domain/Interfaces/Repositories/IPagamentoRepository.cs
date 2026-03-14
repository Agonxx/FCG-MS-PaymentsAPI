using PaymentsAPI.Domain.Entities;

namespace PaymentsAPI.Domain.Interfaces.Repositories
{
    public interface IPagamentoRepository
    {
        Task<bool> Registrar(Pagamento pagamento);
        Task<List<Pagamento>> GetAll();
    }
}

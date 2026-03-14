using PaymentsAPI.Domain.Entities;
using PaymentsAPI.Domain.Interfaces.Repositories;
using PaymentsAPI.Domain.Interfaces.Services;

namespace PaymentsAPI.Application.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _repo;

        public PagamentoService(IPagamentoRepository repo)
        {
            _repo = repo;
        }

        public async Task<Pagamento> ProcessarAsync(Guid orderId, int userId, int jogoId, string titulo, decimal preco)
        {
            // Pagamento simulado — sempre aprovado
            var pagamento = new Pagamento
            {
                OrderId = orderId,
                UserId = userId,
                JogoId = jogoId,
                Titulo = titulo,
                Preco = preco,
                Status = "Approved",
                ProcessadoEm = DateTime.UtcNow
            };

            await _repo.Registrar(pagamento);

            return pagamento;
        }

        public async Task<List<Pagamento>> GetAllAsync()
        {
            return await _repo.GetAll();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using PaymentsAPI.Domain.Entities;
using PaymentsAPI.Domain.Interfaces.Repositories;
using PaymentsAPI.Infrastructure.Data;

namespace PaymentsAPI.Infrastructure.Repositories
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly PaymentsDbContext _db;

        public PagamentoRepository(PaymentsDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Registrar(Pagamento pagamento)
        {
            _db.Pagamentos.Add(pagamento);
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<List<Pagamento>> GetAll()
        {
            return await _db.Pagamentos.ToListAsync();
        }
    }
}

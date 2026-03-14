using Microsoft.EntityFrameworkCore;
using PaymentsAPI.Domain.Entities;

namespace PaymentsAPI.Infrastructure.Data
{
    public class PaymentsDbContext : DbContext
    {
        public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : base(options)
        {
        }

        public DbSet<Pagamento> Pagamentos { get; set; }
    }
}

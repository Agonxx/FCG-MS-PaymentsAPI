using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentsAPI.Domain.Entities
{
    public class Pagamento
    {
        [Key] public int Id { get; set; }
        [Required] public Guid OrderId { get; set; }
        [Required] public int UserId { get; set; }
        [Required] public int JogoId { get; set; }
        [Required][MaxLength(150)] public string Titulo { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal Preco { get; set; }
        [Required][MaxLength(20)] public string Status { get; set; }
        [Required] public DateTime ProcessadoEm { get; set; } = DateTime.UtcNow;
    }
}

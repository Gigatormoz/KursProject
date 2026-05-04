using Microsoft.Graph.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProdjectApi.Domain.Models
{
    [Table("debts")]
    public class Debt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; private set; }

        [Required]
        [Column("rooms_id")]
        public int RoomsId { get; set; }
        public Rooms Room { get; set; }

        [Required]
        [Column("debtor")]
        public int DebtorId { get; set; }
        public Users Debtor { get; set; }

        [Required]
        [Column("lender")]
        public int LenderId { get; set; }
        public Users Lender { get; set; }

        [Required]
        [Column("amount")]
        public decimal Amount { get; set; }

        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("status")]
        public bool Status { get; set; } = false; 
    }
}

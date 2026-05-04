using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProdjectApi.Domain.Models
{
    [Table("expenses")]
    public class Expenses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get;  set; }

        [Required]
        [Column("name"), StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [Column("receipt_amount")]
        public decimal ReceiptAmount { get; set; }

        [Required]
        [Column("payment_date")]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column ("payer_id")]
        public int PayerId { get; set; }

        [Required]
        [Column("distribution_type")]
        public string DistributionType { get; set; } = null!;

        [Required]
        [Column("rooms_id")]
        public int RoomsId { get; set; }

        [Required]
        [Column("currencies_id")]
        public int CurrenciesId { get; set; }

        public virtual Users Payer { get; set; } = null!;
        public virtual Rooms Room { get; set; } = null!;
        public virtual Currencies Currency { get; set; } = null!;

        public List<ExpenseParticipants> Participants { get; set; }
    }
}

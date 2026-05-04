using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProdjectApi.Domain.Models
{
    [Table("rooms")]
    public class Rooms
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; private set; }

        [Required]
        [Column("name"), StringLength(50)]
        public string Name { get; set; } = null!;

        [Column("description"), StringLength(400)]
        public string? Description { get; set; }

        [Required]
        [Column("date_creation")]
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("entry_code"), StringLength(100)]
        public string EntryCode { get; set; } = null!; // генирируется в сервисе

        [Required]
        [Column("creator")]
        public int Creator { get; set; }

        [Required]
        [Column("currencies_id")]
        public int CurrenciesId { get; set; }

        [ForeignKey(nameof(Creator))]
        public virtual Users? CreatorUser { get; set; }

        [ForeignKey(nameof(CurrenciesId))]
        public virtual Currencies? Currency { get; set; }
        public List<Debt> Debts { get; set; }

    }
}

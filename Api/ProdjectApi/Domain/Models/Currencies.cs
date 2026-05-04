using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProdjectApi.Domain.Models
{
    [Table("currencies")]
    public class Currencies
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; private set; }

        [Required]
        [Column("currency_code"), StringLength(3)]
        public string CurrencyCode { get; set; } = null!;

        [Required]
        [Column("name"), StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [Column("symbol"), StringLength(5)]
        public string Symbol { get; set; } = null!;
    }
}

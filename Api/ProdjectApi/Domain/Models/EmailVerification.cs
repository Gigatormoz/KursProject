using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProdjectApi.Domain.Models
{
    [Table("email_verification")]
    public class EmailVerification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; private set; }

        [Column("user_id")] 
        public int UserId { get; set; }

        [Required]
        [Column("code"), StringLength(6, MinimumLength = 6)]
        public string Code { get; set; } = null!;

        [Required]
        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("is_used")]
        public bool IsUsed { get; set; }
    }
}

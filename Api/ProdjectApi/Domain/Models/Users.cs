using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProdjectApi.Domain.Models
{
    [Table("users")]
    public class Users
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get;  set; }

        [Required]
        [Column("name"), StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [Column("surname"), StringLength(50)]
        public string Surname { get;  set; } = null!;

        [Column("patronymic"), StringLength(50)]
        public string? Patronymic { get; set; } = null;

        [Column("nickname"), StringLength(100)]
        public string? Nickname { get; set; } = null;

        [Required]
        [Column("email"), StringLength(255)]
        public string Email { get; set; } = null!;

        [Required]
        [Column("is_email_verified")]
        public bool IsEmailVerified { get; set; } = false;

        [Required]
        [Column("registration_date")]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("hash_password"), StringLength(255)]
        public string PasswordHash { get; set; } = null!;

        public List<ExpenseParticipants> ExpenseParticipants { get; set; }
        public List<Debt> DebtsAsDebtor { get; set; }
        public List<Debt> DebtsAsLender { get; set; }
    }
}


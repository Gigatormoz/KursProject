using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProdjectApi.Domain.Models
{
    [Table("expense_participants")]
    public class ExpenseParticipants
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; private set; }

        [Required]
        [Column("expenses_id")]
        public int ExpensesId { get; set; }
        public Expenses Expense { get; set; }

        [Required]
        [Column("users_id")]
        public int UsersId { get; set; }
        public Users User { get; set; }

        [Required]
        [Column("total_debt")]
        public decimal TotalDebt { get; set; }
    }
}

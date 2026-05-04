using System.ComponentModel.DataAnnotations;

namespace ProdjectApi.Domain.Requests.ExpenseParticipants
{
    public class ExpenseParticipantCreateRequest
    {
        [Required]
        public int ExpensesId { get; set; }
        [Required]
        public int UsersId { get; set; }
        [Required]
        public decimal TotalDebt { get; set; }
    }
}

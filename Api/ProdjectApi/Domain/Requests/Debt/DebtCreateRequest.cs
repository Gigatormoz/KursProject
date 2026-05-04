using System.ComponentModel.DataAnnotations;

namespace ProdjectApi.Domain.Requests.Debt
{
    public class DebtCreateRequest
    {
        [Required]
        public int RoomsId { get; set; }

        [Required]
        public int DebtorId { get; set; }

        [Required]
        public int LenderId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public bool Status { get; set; } = false;
    }
}

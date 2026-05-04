using ProdjectApi.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProdjectApi.Domain.Requests.Expenses
{
    public class ExpensesCreateRequest
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public decimal ReceiptAmount { get; set; }

        [Required]
        public int PayerId { get; set; }

        [Required]
        public string DistributionType { get; set; } = null!;

        [Required]
        public int RoomsId { get; set; }

        [Required]
        public int CurrenciesId { get; set; }
    }
}

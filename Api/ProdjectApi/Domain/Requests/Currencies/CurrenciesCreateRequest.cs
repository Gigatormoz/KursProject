using System.ComponentModel.DataAnnotations;

namespace ProdjectApi.Domain.Requests.Currencies
{
    public class CurrenciesCreateRequest
    {
        [Required]
        [StringLength(3)]
        public string CurrencyCode { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(5)]
        public string Symbol { get; set; } = null!;
    }
}

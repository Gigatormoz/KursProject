using System.ComponentModel.DataAnnotations;

namespace ProdjectApi.Domain.Requests.Currencies
{
    public class CurrencyRateRequest
    {
        [Required]
        public string BaseCurrency { get; set; } = null!; // "USD", "RUB"

        [Required]
        public string TargetCurrency { get; set; } = null!; // "RUB", "USD"
    }
}

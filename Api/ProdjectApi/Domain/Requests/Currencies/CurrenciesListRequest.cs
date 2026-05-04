using System.ComponentModel.DataAnnotations;

namespace ProdjectApi.Domain.Requests.Currencies
{
    public class CurrenciesListRequest
    {
        [StringLength(3)]
        public string? CurrencyCode { get; set; } 

        public int? Limit { get; set; } = 10;
        public int? Offset { get; set; } = 0;
    }
}

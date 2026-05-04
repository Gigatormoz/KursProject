using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProdjectApi.Domain.Services.Currencies
{
    public interface ICurrencyRateService
    {
        Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency);
    }

    public class CbrCurrencyRateService : ICurrencyRateService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CbrCurrencyRateService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency)
        {
            fromCurrency = fromCurrency.ToUpperInvariant();
            toCurrency = toCurrency.ToUpperInvariant();

            using var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync("https://www.cbr-xml-daily.ru/latest.js");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Не удалось получить курсы валют от ЦБ РФ");

            var json = await response.Content.ReadAsStringAsync();
            var cbr = JsonSerializer.Deserialize<CbrRatesResponse>(json);

            if (cbr == null || cbr.Rates == null)
                throw new InvalidOperationException("Некорректиный JSON");

            if (toCurrency == "RUB")
            {
                if (!cbr.Rates.TryGetValue(fromCurrency, out decimal rate))
                    throw new InvalidOperationException($"Курс {fromCurrency} не найден в данных ЦБ РФ");

                return 1m / rate;
            }

            if (fromCurrency == "RUB")
            {
                if (!cbr.Rates.TryGetValue(toCurrency, out decimal rate))
                    throw new InvalidOperationException($"Курс {toCurrency} не найден в данных ЦБ РФ");

                return rate;
            }

            if (!cbr.Rates.TryGetValue(fromCurrency, out decimal rateFrom))
                throw new InvalidOperationException($"Курс {fromCurrency} не найден в данных ЦБ РФ");

            if (!cbr.Rates.TryGetValue(toCurrency, out decimal rateTo))
                throw new InvalidOperationException($"Курс {toCurrency} не найден в данных ЦБ РФ");

            return rateFrom / rateTo;
        }
    }

    public class CbrRatesResponse
    {
        [JsonPropertyName("base")]
        public string Base { get; set; } = string.Empty;

        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; } = new();
    }
}


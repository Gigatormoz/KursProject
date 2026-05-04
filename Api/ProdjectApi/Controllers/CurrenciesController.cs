using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdjectApi.Data;
using ProdjectApi.Domain.Models;
using ProdjectApi.Domain.Requests.Currencies;
using ProdjectApi.Domain.Services.Currencies;

namespace ProdjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly ProjectDbContext _context;
        private readonly ICurrencyRateService _rateService;
        public CurrenciesController(ProjectDbContext context, ICurrencyRateService rateService)
        {
            _context = context;
            _rateService = rateService;
        }
        [HttpGet]
        public async Task<IActionResult> List(
            [FromQuery] string? currencyCode = null,
            [FromQuery] int? limit = 10,
            [FromQuery] int? offset = 0)
        {
            var query = _context.Currencies.AsQueryable();

            if (!string.IsNullOrEmpty(currencyCode))
                query = query.Where(x => x.CurrencyCode == currencyCode.ToUpper());

            var currencies = await query.Skip(offset ?? 0).Take(limit ?? 10).ToListAsync();

            return Ok(currencies.Select(x => new {
                x.Id,
                x.Name,
                x.CurrencyCode,
                x.Symbol }));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CurrenciesCreateRequest request)
        {
            if (await _context.Currencies.AnyAsync(x => x.CurrencyCode == request.CurrencyCode))
                return BadRequest("Валюта с таким кодом существует");

            var currency = new Currencies
            {
                CurrencyCode = request.CurrencyCode.ToUpper(),
                Name = request.Name,
                Symbol = request.Symbol
            };

            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(List), new { id = currency.Id }, new
            {
                currency.Id,
                currency.CurrencyCode,
                currency.Name,
                currency.Symbol
            });
        }

        [HttpGet("rate")]
        public async Task<IActionResult> GetRate(
            [FromQuery] string from,
            [FromQuery] string to)
        {
            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
                return BadRequest("Поля from и to должны быть заданы");

            decimal rate;

            try
            {
                rate = await _rateService.GetExchangeRateAsync(from, to);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка получения курса: {ex.Message}");
            }
            
            return Ok(new
            {
                From = from.ToUpperInvariant(),
                To = to.ToUpperInvariant(),
                Rate = rate
            });
        }

    }
}

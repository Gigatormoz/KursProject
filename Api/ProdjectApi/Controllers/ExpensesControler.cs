using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProdjectApi.Data;
using ProdjectApi.Domain.Models;
using ProdjectApi.Domain.Requests.Expenses;
using Microsoft.EntityFrameworkCore;

namespace ProdjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesControler : ControllerBase
    {
        private readonly ProjectDbContext _context;

        public ExpensesControler(ProjectDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
                return NotFound("Такого расхода не существует!");

            return Ok(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpensesCreateRequest request)
        {

            var payer = await _context.Users.FindAsync(request.PayerId);
            if (payer == null)
                return NotFound("Пользователь не найден!");

            var currency = await _context.Currencies.FindAsync(request.CurrenciesId);
            if (currency is null)
                return NotFound(" В данный момент валюта не найдена!");

            var room = await _context.Rooms.FindAsync(request.RoomsId);
            if (room is null)
                return NotFound("Комната не найдена!");


            var expense = new Expenses
            {
                Name = request.Name,
                ReceiptAmount = request.ReceiptAmount,
                PaymentDate = DateTime.UtcNow,
                PayerId = request.PayerId,
                DistributionType = request.DistributionType,
                RoomsId = request.RoomsId,
                CurrenciesId = request.CurrenciesId
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchExpence([FromQuery] ExpensesSearchRequest request)
        {
            var query = _context.Expenses.AsQueryable();

            if (!string.IsNullOrEmpty(request.Name))
                query = query.Where(x => x.Name.ToLower().Contains(request.Name.ToLower()));

            var expenses = await query.Skip(request.Offset ?? 0).Take(request.Limit ?? 10).ToListAsync();

            return Ok(expenses);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense is null)
                return NotFound();

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

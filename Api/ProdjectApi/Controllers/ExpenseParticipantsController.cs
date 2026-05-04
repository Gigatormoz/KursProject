using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdjectApi.Data;
using ProdjectApi.Domain.Dtos;
using ProdjectApi.Domain.Models;
using ProdjectApi.Domain.Requests.ExpenseParticipants;
using ProdjectApi.Service.Rooms;

namespace ProdjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseParticipantController : ControllerBase
    {
        private readonly ProjectDbContext _context;
        public ExpenseParticipantController(ProjectDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExpenseParticipantCreateRequest request)
        {
            var expense = await _context.Expenses.FindAsync(request.ExpensesId);
            if (expense == null) return NotFound("Раcход не найден");

            var user = await _context.Users.FindAsync(request.UsersId);
            if (user == null) return NotFound("Пользователь не найден");

            var participant = new ExpenseParticipants
            {
                ExpensesId = request.ExpensesId,
                UsersId = request.UsersId,
                TotalDebt = request.TotalDebt
            };

            _context.ExpenseParticipants.Add(participant);
            await _context.SaveChangesAsync();

            return Ok(" Комната успешна создана: " + GetById(participant.Id));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var expenseParticipant = await _context.ExpenseParticipants.FindAsync(id);

            if (expenseParticipant == null)
                return NotFound("Такого участника не существует!");

            return Ok(expenseParticipant);
        }

        [HttpGet("byexpense/{expensesId}")]
        public async Task<ActionResult<List<ExpenseParticipantDto>>> GetByExpense(int expensesId)
        {
            var items = await _context.ExpenseParticipants
            .Where(ep => ep.ExpensesId == expensesId)
            .Select(ep => new ExpenseParticipantDto
            {
                Id = ep.Id,
                ExpensesId = ep.ExpensesId,
                UsersId = ep.UsersId,
                UserName = ep.User.Name,   
                TotalDebt = ep.TotalDebt
            })
        .ToListAsync();

            return items;
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProdjectApi.Data;
using ProdjectApi.Domain.Dtos;
using ProdjectApi.Domain.Models;
using ProdjectApi.Domain.Requests.Debt;
using Microsoft.EntityFrameworkCore;

namespace ProdjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebtsController : ControllerBase
    {
        private readonly ProjectDbContext _context;

        public DebtsController(ProjectDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DebtCreateRequest request)
        {
            var room = await _context.Rooms.FindAsync(request.RoomsId);
            if (room == null)
                return NotFound("Комната не найдена");
            var lender = await _context.Users.FindAsync(request.LenderId);
            var debtor = await _context.Users.FindAsync(request.DebtorId);

            if (room == null) return NotFound("Комната не найдена");
            if (lender == null) return NotFound("Кредитор не найден");
            if (debtor == null) return NotFound("Должник не найден");

            var debt = new Debt
            {
                RoomsId = request.RoomsId,
                DebtorId = request.DebtorId,
                LenderId = request.LenderId,
                Amount = request.Amount,
                UpdatedAt = DateTime.UtcNow,
                Status = request.Status

            };

            _context.Debts.Add(debt);
            await _context.SaveChangesAsync();

            var dto = new DebtDto
            {
                Id = debt.Id,
                RoomsId = debt.RoomsId,
                DebtorId = debt.DebtorId,
                DebtorName = debtor.Name,
                LenderId = debt.LenderId,
                LenderName = lender.Name,
                Amount = debt.Amount,
                UpdatedAt = DateTime.UtcNow,
                Status = debt.Status
            };

            return Ok($"Долг создан!\nId: {GetById(debt.Id)}");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var debt = await _context.Debts.FindAsync(id);

            if (debt == null)
                return NotFound("Такого долга не существует!");

            return Ok(debt);
        }

        [HttpGet("byroom/{roomId}")]
        public async Task<ActionResult<List<DebtDto>>> GetByRoom(int roomId)
        {
            var debts = await _context.Debts
                .Where(d => d.RoomsId == roomId)
                .Select(d => new DebtDto
                {
                    Id = d.Id,
                    RoomsId = d.RoomsId,
                    DebtorId = d.DebtorId,
                    DebtorName = d.Debtor.Name,
                    LenderId = d.LenderId,
                    LenderName = d.Lender.Name,
                    Amount = d.Amount,
                    UpdatedAt = d.UpdatedAt,
                    Status = d.Status
                })
                .ToListAsync();

            return debts;
        }
    }
}

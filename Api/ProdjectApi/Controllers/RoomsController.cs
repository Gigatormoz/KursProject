using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using ProdjectApi.Data;
using ProdjectApi.Domain.Models;
using ProdjectApi.Domain.Requests.Rooms;
using ProdjectApi.Service.Rooms;
namespace ProdjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ProjectDbContext _context;
        private readonly RoomCodeGenerator _roomCodeGenerator;

        public RoomsController(ProjectDbContext context, RoomCodeGenerator roomCodeGenerator)
        {
            _context = context;
            _roomCodeGenerator = roomCodeGenerator;
        }

        //создать комнату
        [HttpPost]
        public async Task<IActionResult> CreateRooms([FromBody] RoomCreateRequest request)
        {
            var entryCode = _roomCodeGenerator.Generate();

            while (await _context.Rooms.AnyAsync(x => x.EntryCode == entryCode))
            {
                entryCode = _roomCodeGenerator.Generate();
            }

            var creater = await _context.Users.FindAsync(request.Creator);
            if (creater is null)
                return NotFound("Пользователь не найден.");

            var currency = await _context.Currencies.FindAsync(request.CurrenciesId);
            if (currency is null)
                return NotFound("Приносим свои извенения! В данный момент валюта не найдена.\nПопробуйте позже или выберите другую");

            var room = new Rooms
            {
                Name = request.Name,
                Description = request.Description,
                EntryCode = entryCode,
                DateCreation = DateTime.UtcNow,
                Creator = request.Creator,
                CurrenciesId = request.CurrenciesId
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }

        //Переход в комнату по коду
        [HttpPost("join")]
        public async Task<IActionResult> JoinRoom([FromBody] RoomJoinRequest request)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.EntryCode == request.EntryCode);

            if (room == null)
                return NotFound("Код комнаты неверный или комната не существует!");

            var user = await _context.Users.FindAsync(request.UserId);

            if (user == null)
                return NotFound("Пользователь не найден!");

            var alreadyMember = await _context.BandMembers.AnyAsync(bm => bm.UsersId == user.Id && bm.RoomsId == room.Id);

            if (alreadyMember)
                return BadRequest("Пользователь уже состоит в группе");

            var bandMember = new BandMember
            {
                UsersId = user.Id,
                RoomsId = room.Id,
                WhenJoined = DateTime.UtcNow,
            };

            _context.BandMembers.Add(bandMember);
            await _context.SaveChangesAsync();

            return Ok($"Успешно присоединились к комнате {room.Name}!");
        }
        //поиск комнаты по id
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
                return NotFound("Такой комнаты не существует!");

            return Ok(room);
        }

        //Поиск комнаты по имени
        [HttpGet("search")]
        public async Task<IActionResult> SearchRoom([FromQuery] RoomSearchRequest request)
        {
            var query = _context.Rooms.AsQueryable();

            if (!string.IsNullOrEmpty(request.Name))
                query = query.Where(x => x.Name.ToLower().Contains(request.Name.ToLower()));

            var rooms = await query.Skip(request.Offset ?? 0).Take(request.Limit ?? 10).ToListAsync();

            return Ok(rooms);
        }

        //Изменения в комнате
        [HttpPut("{id}")]
        public async Task<IActionResult> RoomUpdate(int id, RoomUpdateRequest request)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
                return NotFound("Такой комнаты не существует");

            if (request.CurrenciesId == null)
            {
                var currency = _context.Currencies.FindAsync(request.CurrenciesId);

                if (currency == null)
                    return NotFound("Валюта не найдена");

                room.CurrenciesId = request.CurrenciesId;
            }

            room.Name = request.Name ?? room.Name;
            room.Description = request.Description ?? room.Description;
            
            await _context.SaveChangesAsync();

            return Ok("Комната успешно изменена");
        }

        //Удалить комнату
        [HttpDelete]
        public async Task<IActionResult> DeleteById(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
                return NotFound("Комнаты не существует или она уже удалена.");

            _context.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Баланс комнаты
        [HttpGet("{roomId}/balances")]
        public async Task<ActionResult<List<GroupBalanceRequest>>> GetBalance(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return NotFound("Комната не найдена");

            var memberIds = await _context.BandMembers.Where(bm => bm.RoomsId == roomId).Select(bm => bm.UsersId).ToListAsync();

            var users = await _context.Users.Where(u => memberIds.Contains(u.Id)).ToListAsync();

            var balances = users.Select(u => new GroupBalanceRequest
                {
                    UserId = u.Id,
                    Name = u.Name,
                    Balance = 0
                }).ToList();

            var balanceMap = balances.ToDictionary(x => x.UserId);

            var expenses = await _context.Expenses.Where(e => e.RoomsId == roomId).ToListAsync();

            foreach (var expense in expenses)
            {
                var participants = await _context.ExpenseParticipants.Where(ep => ep.ExpensesId == expense.Id).Select(ep => new
                {
                    ep.UsersId,
                    ep.TotalDebt
                }).ToListAsync();

                foreach (var participant in participants)
                {
                    if (balanceMap.ContainsKey(participant.UsersId))
                    {
                        balanceMap[participant.UsersId].Balance -= participant.TotalDebt;
                    }
                }

                if (balanceMap.ContainsKey(expense.PayerId))
                    balanceMap[expense.PayerId].Balance += expense.ReceiptAmount;
            }

            foreach (var item in balances)
            {
                if (item.Balance > 0)
                    item.BalanceText = $"Вам должны {item.Balance:0.##}";

                else if (item.Balance < 0)
                    item.BalanceText = $"Вы должны {Math.Abs(item.Balance):0.##}";

                else
                    item.BalanceText = "Вы в расчёте";

            }

            return Ok(balances);
        }
    }
}

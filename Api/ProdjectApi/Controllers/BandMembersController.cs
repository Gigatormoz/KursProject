using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProdjectApi.Data;
using ProdjectApi.Domain.Models;

namespace ProdjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BandMembersController : ControllerBase
    {
        private readonly ProjectDbContext _context;
        public BandMembersController(ProjectDbContext context)
        {
            _context = context;
        }

        [HttpPost("rooms/{roomId}/members")]
        public async Task<IActionResult> AddMember(int roomId, int userId)
        {
            var member = new BandMember { RoomsId = roomId, UsersId = userId };
            _context.BandMembers.Add(member);
            await _context.SaveChangesAsync();
            return Ok(member);  
        }
    }
}

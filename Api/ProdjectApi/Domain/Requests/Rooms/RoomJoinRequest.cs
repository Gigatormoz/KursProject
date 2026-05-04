using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProdjectApi.Domain.Requests.Rooms
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomJoinRequest : ControllerBase
    {
        [Required]
        public string EntryCode { get; set; } = null!;
        [Required]
        public int UserId { get; set; }

    }
}

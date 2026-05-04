using System.ComponentModel.DataAnnotations;

namespace ProdjectApi.Domain.Requests.Rooms
{
    public class RoomSearchRequest
    {
        [MaxLength(50)]
        public string? Name { get; set; }

        public int? Limit { get; set; } = 10;
        public int? Offset { get; set; } = 0;
    }
}

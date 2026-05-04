using System.ComponentModel.DataAnnotations;

namespace ProdjectApi.Domain.Requests.Rooms
{
    public class RoomCreateRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(400)]
        public string? Description { get; set; }

        [Required]
        public int Creator { get; set; }

        [Required]
        public int CurrenciesId { get; set; }
    }
}

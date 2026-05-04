using System.ComponentModel.DataAnnotations;

namespace ProdjectApi.Domain.Requests.Users
{
    public class UpdateUserRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; } = null!;

        [MaxLength(50)]
        public string? Patronymic { get; set; } = null;

        [MaxLength(100)]
        public string? Nickname { get; set; } = null;
    }
}

using System.ComponentModel.DataAnnotations;

namespace ProdjectApi.Domain.Requests.Users
{
    public class CreateUserRequest
    {
            [Required]
            [MaxLength(50)]
            public string Name { get; set; } = null!;

            [Required]
            [MaxLength(50)]
            public string Surname { get; set; } = null!;

            [MaxLength(50)]
            public string? Patronymic { get; set; } = null;

            [Required]
            [EmailAddress]
            [MaxLength(255)]
            public string Email { get; set; } = null!;

            [Required]
            [StringLength(100, MinimumLength = 6)]
            public string Password { get; set; } = null!;

            [MaxLength(100)]
            public string? Nickname { get; set; } = null;
    }
}
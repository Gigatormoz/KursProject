using System.ComponentModel.DataAnnotations;

namespace ProdjectApi.Domain.Requests.Users
{
    public class ChangeEmailRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = null!;
    }
}

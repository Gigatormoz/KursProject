using System.ComponentModel.DataAnnotations;

namespace ProdjectApi.Domain.Requests.Users
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; } = null!;
    }
}

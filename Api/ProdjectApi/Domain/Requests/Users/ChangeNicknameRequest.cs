using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProdjectApi.Domain.Requests.Users
{
    public class ChangeNicknameRequest
    {
        [Required]
        [MaxLength(100)]
        public string? Nickname { get; set; } = null;
    }
}

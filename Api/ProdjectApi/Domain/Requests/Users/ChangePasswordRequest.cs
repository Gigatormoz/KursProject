using System.ComponentModel.DataAnnotations;

namespace ProdjectApi.Domain.Requests.Users
{
    public class ChangePasswordRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string OldPassword { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; } = null!;
    }
}

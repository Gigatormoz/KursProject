using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProdjectApi.Domain.Models
{
    [Table("band_members")]
    public class BandMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; private set; }

        [Required]
        [Column("when_joined")]
        public DateTime WhenJoined { get; set; } = DateTime.UtcNow;

        [Required]
        [Column ("users_id")]
        public int UsersId { get; set; }

        [Required]
        [Column("rooms_id")]
        public int RoomsId { get; set; }

        [ForeignKey(nameof(UsersId))]
        public virtual Users User { get; set; } = null!;

        [ForeignKey(nameof(RoomsId))]
        public virtual Rooms Room { get; set; } = null!;

    }
}

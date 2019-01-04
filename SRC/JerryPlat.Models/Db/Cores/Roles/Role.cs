using JerryPlat.Utils.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JerryPlat.Models
{
    public class Role : IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        public int NavigationId { get; set; }

        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
        [ForeignKey("NavigationId")]
        public virtual Navigation Navigation { get; set; }
    }
}
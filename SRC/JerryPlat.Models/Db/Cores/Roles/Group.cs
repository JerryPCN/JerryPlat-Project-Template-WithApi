using JerryPlat.Utils.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public class Group : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        public int OrderIndex { get; set; }

        public virtual ICollection<AdminUser> AdminUsers { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
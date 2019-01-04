using JerryPlat.Utils.Models;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public enum SiteType
    {
        Admin = 1,
        Web = 2
    }

    public enum NavigationType
    {
        All = 0,
        Page = 1,
        Button = 2,
    }

    public class Navigation : IEntity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Code { get; set; }

        [StringLength(200)]
        public string RequestUrl { get; set; }

        [Required]
        public int ParentId { get; set; }

        [Required]
        public int OrderIndex { get; set; }

        [Required]
        public SiteType SiteType { get; set; }

        [Required]
        public NavigationType NavigationType { get; set; }
    }
}
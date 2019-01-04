using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JerryPlat.Models
{
    public class OwinToken : IEntity
    {
        [Key]
        public int Id { get; set; }
        [StringLength(200)]
        public string ClientId { get; set; }
        [StringLength(200)]
        public string ClientSecret { get; set; }
    }

}

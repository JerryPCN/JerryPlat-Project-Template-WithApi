using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JerryPlat.Models
{
    public class Banner : IEntity
    {
        [Key]
        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(200)]
        public string PicPath { get; set; }
        public int OrderIndex { get; set; }
    }
}

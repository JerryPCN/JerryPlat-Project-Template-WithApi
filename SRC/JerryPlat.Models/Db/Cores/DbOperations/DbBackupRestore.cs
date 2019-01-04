using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JerryPlat.Models
{
    public class DbBackupRestore : IEntity
    {
        [Key]
        public int Id { get; set; }
        [StringLength(200)]
        public string BackupPath { get; set; }
        public int RestoreCount { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? RestoreTime { get; set; }

        public DbBackupRestore()
        {
            CreateTime = DateTime.Now;
        }
    }
}

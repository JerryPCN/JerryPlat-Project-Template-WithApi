using JerryPlat.Models.Dto;
using JerryPlat.Utils.Models;
using NPOI.Extension;
using System;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public class Member : MemberDto, IEntity
    {
        [Key]
        [Column(Index = 0)]
        public int Id { get; set; }

        [StringLength(200)]
        [Column(Index = 1)]
        public string OpenId { get; set; }

        [StringLength(200)]
        [Column(IsIgnored = true)]
        public string Avatar { get; set; }

        [Column(Index = 2, Title = "性别")]
        public Sex Sex { get; set; }

        [StringLength(200)]
        [Column(IsIgnored = true)]
        public string Phone { get; set; }

        [StringLength(200)]
        [Column(Index = 3, Title = "姓名")]
        public string Name { get; set; }
        
        [Column(IsIgnored = true)]
        public int RefereeId { get; set; }

        [Column(Index = 4, Title = "加入时间", Formatter = "yyyy-MM-dd HH:mm:ss")]
        public DateTime JoinTime { get; set; }

        [StringLength(200)]
        [Column(IsIgnored = true)]
        public string ShareCode { get; set; }

        public Member()
        {
            JoinTime = DateTime.Now;
        }
    }
}
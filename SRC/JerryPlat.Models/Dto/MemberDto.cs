using NPOI.Extension;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models.Dto
{
    public class MemberDto
    {
        [StringLength(200)]
        [Column(Index = 5, Title = "昵称")]
        public string NickName { get; set; }
        
        [Column(IsIgnored = true)]
        public float? Latitude { get; set; }

        [Column(IsIgnored = true)]
        public float? Longitude { get; set; }

        [Column(IsIgnored = true)]
        public float? Precision { get; set; }
    }
}
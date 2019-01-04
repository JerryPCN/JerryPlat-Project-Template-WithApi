using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace JerryPlat.Owin.Models
{
    public class ClaimModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public ClaimModel(IEnumerable<Claim> claims)
        {
            UserId = Convert.ToInt32(claims.ElementAt(0).Value);
            Name = claims.ElementAt(1).Value;
            Role = claims.ElementAt(2).Value;
        }
    }
}
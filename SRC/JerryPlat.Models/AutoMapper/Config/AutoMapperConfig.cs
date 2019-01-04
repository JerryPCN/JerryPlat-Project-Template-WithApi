using AutoMapper;
using JerryPlat.Models.AutoMapper.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JerryPlat.Models.AutoMapper.Config
{
    public class AutoMapperConfig
    {
        public static void Init()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<DtoProfile>();
            });
        }
    }
}

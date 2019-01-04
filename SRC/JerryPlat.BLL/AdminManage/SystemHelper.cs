using JerryPlat.DAL;
using JerryPlat.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JerryPlat.BLL.AdminManage
{
    public class SystemHelper : AdminBaseSessionHelper
    {
        public async Task ClearData()
        {
            await _Db.Database.ExecuteSqlCommandAsync(@"
                truncate table Banner;
                truncate table Member;");
        }
    }
}

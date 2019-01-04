using JerryPlat.BLL.AdminManage;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;


namespace JerryPlat.API.Controllers
{
    public class UserController : AdminAuthurizeBaseApiController<AdminUserHelper, AdminUser, AdminUserDto>
    {
        private static List<int> NoActionIdList = new List<int> { 1 };
        protected override List<int> GetNoActionIdList()
        {
            return NoActionIdList;
        }

        [HttpPost]
        public async Task<IHttpActionResult> ChangePassword(PasswordDto model)
        {
            bool result = await _helper.ChangePassword(model);
            return Success(result);
        }
    }
}
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using System.Threading.Tasks;
using System.Web.Http;
using System.Collections.Generic;
using JerryPlat.Utils.Helpers;
using JerryPlat.BLL.AdminManage;
using JerryPlat.BLL.CommonManage;

namespace JerryPlat.API.Controllers
{
    public class GroupController : AdminAuthurizeBaseApiController<GroupHelper, Group, GroupDto>
    {
        private static List<int> NoActionIdList = new List<int> { 1 };
        protected override List<int> GetNoActionIdList()
        {
            return NoActionIdList;
        }

        [HttpPost]
        public IHttpActionResult GetNavigationTreeList()
        {
            return Success(new AdminNavigationHelper().GetTreeList(SiteType.Admin));
        }
    }
}
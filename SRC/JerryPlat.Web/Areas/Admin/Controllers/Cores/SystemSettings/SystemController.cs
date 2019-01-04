using JerryPlat.BLL.AdminManage;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using JerryPlat.Web.Areas.Base;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Admin.Controllers
{
    public class SystemController : AdminAuthurizeBaseController
    {
        [HttpPost]
        public ActionResult Restart()
        {
            SiteHelper.RestartAppDomain();
            return Success();
        }
    }
}
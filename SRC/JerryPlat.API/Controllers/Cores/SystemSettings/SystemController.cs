using JerryPlat.BLL.AdminManage;
using JerryPlat.Utils.Helpers;
using System.Threading.Tasks;
using System.Web.Http;

namespace JerryPlat.API.Controllers
{
    public class SystemController : AdminAuthurizeBaseApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> ClearData()
        {
            SystemHelper helper = new SystemHelper();
            await helper.ClearData();
            return Restart();
        }

        [HttpPost]
        public IHttpActionResult Restart()
        {
            SiteHelper.RestartAppDomain();
            return Success();
        }
    }
}
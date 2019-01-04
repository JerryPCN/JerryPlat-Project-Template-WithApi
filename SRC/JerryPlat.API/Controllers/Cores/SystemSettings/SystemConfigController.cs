using JerryPlat.BLL.AdminManage;
using JerryPlat.Utils.Models;
using System.Web.Http;

namespace JerryPlat.API.Controllers
{
    public class SystemConfigController : AdminAuthurizeBaseApiController<SystemConfigHelper>
    {
        [HttpPost]
        public IHttpActionResult Get()
        {
            return Success(_helper.Get());
        }

        [HttpPost]
        public IHttpActionResult Save(SystemConfigModel model)
        {
            _helper.Save(model);
            return Success();
        }
    }
}
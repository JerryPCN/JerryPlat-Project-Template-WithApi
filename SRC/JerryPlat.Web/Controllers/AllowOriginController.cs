using JerryPlat.BLL.CommonManage;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Models;
using JerryPlat.Web.App_Start.Filter;
using JerryPlat.Web.Areas.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JerryPlat.Web.Controllers
{
    [ControllerAllowOrigin]
    public class AllowOriginController : BaseController
    {
        // GET: Api
        [HttpPost]
        //[ActionAllowOrigin]
        public ActionResult GetReport()
        {
            return Success();
        }
    }
}
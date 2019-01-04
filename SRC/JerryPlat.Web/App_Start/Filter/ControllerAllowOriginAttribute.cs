using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JerryPlat.Web.App_Start.Filter
{
    public class ControllerAllowOriginAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            AllowOriginHelper.OnExcute(filterContext.HttpContext, SystemConfigModel.Instance.AllowOriginSites.Split(';'));
        }
    }
}
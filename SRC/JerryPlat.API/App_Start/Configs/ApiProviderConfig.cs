using JerryPlat.API.App_Start.Providers;
using JerryPlat.API.App_Start.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;

namespace JerryPlat.API.App_Start.Configs
{
    public class ApiProviderConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Services.Add(typeof(ValueProviderFactory), new CookieValueProviderFactory());
            //config.Services.Replace(typeof(IHttpActionSelector), new CustomApiControllerActionSelector());
        }
    }
}
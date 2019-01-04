using JerryPlat.API.App_Start.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace JerryPlat.API.App_Start.Configs
{
    public class ApiFilterConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configure Web API to use only bearer token authentication.
            config.Filters.Add(new ValidateModelAttribute());
            config.Filters.Add(new CustomHandleExceptionFilterAttribute());
        }
    }
}
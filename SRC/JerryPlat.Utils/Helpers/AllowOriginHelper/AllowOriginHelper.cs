using System;
using System.Configuration;
using System.Linq;
using System.Web;

namespace JerryPlat.Utils.Helpers
{
    public static class AllowOriginHelper
    {
        public static void OnExcute(HttpContextBase httpContext, string[] allowOriginSites)
        {
            var origin = httpContext.Request.Headers["Origin"];
            if (allowOriginSites != null && allowOriginSites.Contains(origin))
            {
                httpContext.Response.AppendHeader("Access-Control-Allow-Origin", origin);
            }
        }
    }
}
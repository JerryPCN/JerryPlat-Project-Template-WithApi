using JerryPlat.DAL;
using JerryPlat.Models.AutoMapper.Config;
using JerryPlat.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace JerryPlat.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DbContextHelper.Init();
            AutoMapperConfig.Init();
        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");           //Remove Server Header
            Response.Headers.Remove("X-AspNet-Version"); //Remove X-AspNet-Version Header
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            LogHelper.Error(ex);

            //.....这里可以根据项目需要返回到客户端特定的状态码。如果找不到相应的异常，统一返回服务端错误500
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

            //2.返回调用方具体的异常信息
            if (ex is NotImplementedException)
            {
                httpStatusCode = HttpStatusCode.NotImplemented;
            }
            else if (ex is TimeoutException)
            {
                httpStatusCode = HttpStatusCode.RequestTimeout;
            }
            else if (ex is HttpException)
            {
                httpStatusCode = EnumHelper.ToEnum<HttpStatusCode>((ex as HttpException).GetHttpCode());
            }

            HttpContext httpContext = ((WebApiApplication)sender).Context;
            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = (int)httpStatusCode;
            httpContext.Response.Write(ex);
        }
    }
}

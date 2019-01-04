using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using System;
using System.Data.Entity.Core;
using System.Web;
using System.Web.Mvc;

namespace JerryPlat.Web.App_Start.Filter
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            LogHelper.Error(filterContext.Exception);

            bool bIsEFException = filterContext.Exception is EntityException;

            if (bIsEFException)
            {
                SiteHelper.RestartAppDomain();
            }

            string strErrorMsg = bIsEFException ? EF_ERROR : GetExceptionType(filterContext.Exception);

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                JsonNetResult jsonResult = new JsonNetResult
                {
                    Data = bIsEFException ? ResponseModel<string>.Logout("/", strErrorMsg) : ResponseModel<string>.Error(strErrorMsg),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.Result = jsonResult;
                return;
            }

            var result = new ViewResult()
            {
                ViewName = "Error"
            };
            result.ViewBag.Error = strErrorMsg;
            result.ViewBag.IsRestart = bIsEFException;
            filterContext.Result = result;
        }

        private const string ERROR = "程序发生了错误，请先联系技术人员，或稍候再试。";
        private const string INVALID_REQUEST = "对不起，该请求非法，请合法的使用该站点。";
        private const string EF_ERROR = "当前数据库出现异常，即将重启站点。";

        private string GetExceptionType(Exception ex)
        {
            if (ex is HttpAntiForgeryException
                || ex is HttpRequestValidationException)
            {
                return INVALID_REQUEST;
            }

            return ERROR;
        }
    }
}
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace JerryPlat.API.App_Start.Filters
{
    public class CustomHandleExceptionFilterAttribute : ExceptionFilterAttribute
    {
        //重写基类的异常处理方法
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            LogHelper.Error(actionExecutedContext.Exception);

            //.....这里可以根据项目需要返回到客户端特定的状态码。如果找不到相应的异常，统一返回服务端错误500
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            
            //2.返回调用方具体的异常信息
            if (actionExecutedContext.Exception is NotImplementedException)
            {
                httpStatusCode = HttpStatusCode.NotImplemented;
            }
            else if (actionExecutedContext.Exception is TimeoutException)
            {
                httpStatusCode = HttpStatusCode.RequestTimeout;
            }
            else if (actionExecutedContext.Exception is HttpException)
            {
                httpStatusCode = EnumHelper.ToEnum<HttpStatusCode>((actionExecutedContext.Exception as HttpException).GetHttpCode());
            }

            //3.返回Response
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(httpStatusCode, actionExecutedContext.Exception);
            base.OnException(actionExecutedContext);
        }
    }
}
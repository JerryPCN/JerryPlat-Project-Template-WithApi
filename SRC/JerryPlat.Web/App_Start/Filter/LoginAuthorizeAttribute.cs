using JerryPlat.BLL.CommonManage;
using JerryPlat.Models;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JerryPlat.Web.App_Start.Filter
{
    public class LoginAuthorizeAttribute : AuthorizeAttribute
    {
        private string _Area { get; set; }
        private string _LoginUri { get; set; }

        private bool _IsNoPermissionToAccess = false;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.Request.RequestContext.RouteData.DataTokens.Keys.Contains("area"))
            {
                return true;
            }

            _Area = httpContext.Request.RequestContext.RouteData.DataTokens["area"].ToString();

            SessionHelper session = TypeHelper.GetInstance<SessionHelper>(_Area);

            if (session == null)
            {
                return true;
            }

            _LoginUri = session.LoginUri;

            if (session.IsNullSession())
            {
                return false;
            }


            if (httpContext.Request.IsAjaxRequest())
            {
                if (_Area == SiteType.Admin.ToString())
                {
                    AdminNavigationHelper helper = new AdminNavigationHelper();
                    if (!helper.IsValidAjaxRequest(httpContext.Request.Url.AbsolutePath))
                    {
                        _IsNoPermissionToAccess = true;
                        return false;
                    }
                }
            }

            return true;
        }

        //加上如下的重写方法，在方法内进行跳转，问题解决
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (_IsNoPermissionToAccess)
            {
                filterContext.Result = new JsonResult()
                {
                    Data = ResponseModel<string>.Invalid(MessageHelper.NoPermissionToAccess),
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                return;
            }

            string url = $"{_LoginUri}?returnURL=";
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                url += HttpUtility.UrlEncode(filterContext.HttpContext.Request.UrlReferrer.ToString());
                filterContext.Result = new JsonResult()
                {
                    Data = ResponseModel<string>.Logout(url, MessageHelper.NoSession),
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                url += HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.ToString());
                filterContext.Result = new RedirectResult(url);
            }
        }
    }
}
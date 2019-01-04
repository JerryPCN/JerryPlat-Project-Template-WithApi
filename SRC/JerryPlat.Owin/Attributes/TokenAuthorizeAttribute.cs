using JerryPlat.BLL.CommonManage;
using JerryPlat.Models;
using JerryPlat.Owin.Identities;
using JerryPlat.Utils.Helpers;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using JerryPlat.Utils.Models;
using System.Text;

namespace JerryPlat.Owin.Attributes
{
    public class TokenAuthorizeAttribute : AuthorizeAttribute
    {
        private bool _IsNoPermissionToAccess = false;
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var claims = ClaimsPrincipal.Current.Claims;
            if (!claims.Any())
            {
                return false;
            }

            Claim claim = claims.Where(o => o.Type == ClaimTypes.System).FirstOrDefault();
            if(claim == null)
            {
                return false;
            }
            
            //[TokenAuthorize(Users = "Admin")]
            if (!string.IsNullOrEmpty(Users))
            {
                //TODO
            }

            //[TokenAuthorize(Roles = "Edit")]
            if (string.IsNullOrEmpty(Roles))
            {
                //TODO
            }

            if (EnumHelper.ToEnum<SiteType>(claim.Value) == SiteType.Admin)
            {
                AdminNavigationHelper helper = new AdminNavigationHelper();
                if (!helper.IsValidAjaxRequest(actionContext.Request.RequestUri.AbsolutePath))
                {
                    _IsNoPermissionToAccess = true;
                    return false;
                }
            }

            return base.IsAuthorized(actionContext);
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (_IsNoPermissionToAccess)
            {
                var response = actionContext.Response = actionContext.Response ?? new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.OK;
                ResponseModel<string> responseModel = ResponseModel.Invalid(MessageHelper.NoPermissionToAccess);
                response.Content = new StringContent(SerializationHelper.ToJson(responseModel), Encoding.UTF8, "application/json");
                return;
            }

            base.HandleUnauthorizedRequest(actionContext);
        }
    }
}
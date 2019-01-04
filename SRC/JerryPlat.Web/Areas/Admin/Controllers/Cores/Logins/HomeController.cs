using JerryPlat.BLL.AdminManage;
using JerryPlat.BLL.CommonManage;
using JerryPlat.Models;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using JerryPlat.Web.Areas.Base;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController<AdminUserHelper>
    {
        [HttpPost]
        public async Task<ActionResult> Login(string returnURL, LoginModel loginModel)
        {
            loginModel.Password = EncryptHelper.Encrypt(loginModel.Password);
            AdminUser user = await _helper.GetAsync(loginModel);
            if (user == null)
            {
                return Faild("您输入的用户名或密码错误。");
            }

            TokenModel model = ApiHelper.Instance.GetToken(loginModel);
            if(model == null)
            {
                return Faild("获取Token失败，请稍候重试。");
            }

            CookieHelper.SetTokenCookie(model);

            _helper.SetSession(user);

            AdminNavigationHelper navigatoinHelper = new AdminNavigationHelper();

            string strResult = navigatoinHelper.GetFirstPageUrl(SiteType.Admin, user.GroupId);

            if (!string.IsNullOrEmpty(returnURL) && returnURL != "null")
            {
                returnURL = HttpUtility.UrlDecode(returnURL);

                if (HttpHelper.IsLocalUrl(returnURL))
                {
                    strResult = returnURL;
                }
            }

            return Success(strResult);
        }

        [HttpPost]
        public ActionResult RefreshToken(string refrshtoken)
        {
            if (string.IsNullOrEmpty(refrshtoken))
            {
                return RefreshTokenFaild("获取Refresh Token失败，请重新登陆。");
            }

            TokenModel model = ApiHelper.Instance.GetRefreshToken(refrshtoken);

            if (model == null)
            {
                return RefreshTokenFaild("获取Refresh Token失败，请重新登陆。");
            }

            CookieHelper.SetTokenCookie(model);

            return Success();
        }


        /// <summary>
        /// 注销  删除session+cookie
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            SessionHelper.ClearSession();
            return Redirect("/Admin/Home/Index");
        }
    }
}
using JerryPlat.Utils.Helpers;
using System.Web.Mvc;

namespace JerryPlat.Web.Controllers
{
    public class VerifyCodeController : Controller
    {
        // GET: VerifyCode
        public ActionResult Index()
        {
            string strVerifyCode = RandomHelper.CreateCode(4);
            SessionHelper.VerifyCode.SetSession(strVerifyCode);
            return File(IOHelper.GenerateValidateGraphic(strVerifyCode), "image/Jpeg");
        }
    }
}
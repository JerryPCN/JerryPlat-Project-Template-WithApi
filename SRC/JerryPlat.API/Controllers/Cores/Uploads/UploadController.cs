using JerryPlat.Utils.Helpers;
using System.Web;
using System.Web.Http;
namespace JerryPlat.API.Controllers
{
    public class UploadController : AdminAuthurizeBaseApiController
    {
        [HttpPost]
        public IHttpActionResult Index(HttpPostedFileBase file, string folder)
        {
            if (!file.IsValid())
            {
                return Faild("请上传合法的文件。");
            }

            return Success(file.SaveTo(folder));
        }
    }
}
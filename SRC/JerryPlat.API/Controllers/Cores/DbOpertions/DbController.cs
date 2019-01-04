using JerryPlat.BLL.AdminManage;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using System.Threading.Tasks;
using System.Web.Http;

namespace JerryPlat.API.Controllers
{
    public class DbController : AdminAuthurizeBaseApiController<DbBackupRestoreHelper>
    {
        [HttpPost]
        public async Task<IHttpActionResult> GetPageList(PageSearchModel model)
        {
            var pageData = await _helper.GetPageListAsync(model.SearchModel, o => o.CreateTime, model.PageParam, false);
            return Success(pageData);
        }

        [HttpPost]
        public async Task<IHttpActionResult> BackUp()
        {
            string strResult = await _helper.BackUpAsync();
            if (strResult == ConstantHelper.Ok)
            {
                return Success();
            }
            return Faild(strResult);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Restore(int id)
        {
            string strResult = await _helper.RestoreAsync(id);
            if (strResult == ConstantHelper.Ok)
            {
                return Success();
            }
            return Faild(strResult);
        }
    }
}
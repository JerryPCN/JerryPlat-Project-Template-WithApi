using JerryPlat.DAL;
using JerryPlat.DAL.Context;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace JerryPlat.API.Controllers
{
    #region BaseSessionApiController
    public class BaseSessionApiController<TSession> : ApiController
        where TSession : class
    {
        #region Session
        private TSession _session { get; set; }
        protected TSession _Session => _session ?? (_session = GetSession());
        private TSession GetSession()
        {
            ClaimsPrincipal clainsPrincipal = User as ClaimsPrincipal;
            if (clainsPrincipal != null)
            {
                Claim claim = clainsPrincipal.Claims.Where(o => o.Type == ClaimTypes.UserData).FirstOrDefault();
                if (claim != null)
                {
                    _session = SerializationHelper.JsonToObject<TSession>(claim.Value);
                }
            }
            return null;
        }
        #endregion

        #region Response Result
        protected IHttpActionResult Success()
        {
            return Return(ResponseModel.Ok(""));
        }

        protected IHttpActionResult Success<T>(T data)
        {
            return Return(ResponseModel<T>.Ok(data));
        }

        protected IHttpActionResult Faild(string strMsg)
        {
            return Return(ResponseModel.Error(strMsg));
        }

        protected IHttpActionResult Invalid(string strMsg)
        {
            return Return(ResponseModel.Invalid(strMsg));
        }

        protected new IHttpActionResult NotFound()
        {
            return Return(ResponseModel.NotFound());
        }

        protected IHttpActionResult Existed()
        {
            return Return(ResponseModel.Existed());
        }

        private static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            //忽略模型循环引用
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            // 设置日期序列化的格式
            DateFormatString = "yyyy-MM-dd HH:mm:ss"
        };

        private IHttpActionResult Return<T>(ResponseModel<T> responseModel)
        {
            return Json(responseModel, jsonSerializerSettings);
            //return Ok<ResponseModel<T>>(responseModel);
        }
        #endregion Response Result
    }

    public class BaseSessionApiController<THelper, TSession> : BaseSessionApiController<TSession>
        where THelper : DbContextBaseHelper<JerryPlatDbContext, TSession>, new()
        where TSession : class
    {
        #region Helper
        private THelper helper;
        protected THelper _helper
        {
            get
            {
                if (helper == null)
                {
                    helper = new THelper();
                    helper.InitSession(this._Session);
                }
                return helper;
            }
        }
        #endregion Helper

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing && helper != null)
            {
                helper.Dispose();
            }

            base.Dispose(disposing);
        }
        #endregion Dispose
    }

    public class BaseSessionApiController<THelper, TEntity, TQueryableEntity, TSession>
            : BaseSessionApiController<THelper, TSession>
        where THelper : BaseSessionHelper<TEntity, TQueryableEntity, TSession>, new()
        where TEntity : class, new()
        where TQueryableEntity : class, new()
        where TSession : class
    {

        #region NoAction
        private bool IsNoActionRecord(TQueryableEntity entity)
        {
            return IsNoActionRecord(TypeHelper.GetIdPropertyValue(entity));
        }

        private bool IsNoActionRecord(int intId)
        {
            return IsNoActionRecord(new List<int> { intId });
        }

        private bool IsNoActionRecord(List<int> idList)
        {
            List<int> noActionIdList = GetNoActionIdList();
            if (noActionIdList == null)
            {
                return false;
            }
            return idList.Intersect(noActionIdList).Any();
        }

        protected virtual List<int> GetNoActionIdList()
        {
            return null;
        }
        #endregion

        [HttpPost]
        public virtual async Task<IHttpActionResult> GetPageList(PageSearchModel model)
        {
            PageData<TQueryableEntity> pageData = await _helper.GetPageListAsync(model.SearchModel, PageHelper.GetKeyExpression<TQueryableEntity, int>(new string[] { "OrderIndex", "Id" }), model.PageParam, true);
            return Success(pageData);
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> GetList(SearchModel model)
        {
            List<TQueryableEntity> pageData = await _helper.GetListAsync(model);
            return Success(pageData);
        }

        [HttpGet]
        public virtual async Task<IHttpActionResult> GetDetail(int id)
        {
            TQueryableEntity entity = await _helper.GetByIdAsync(id);
            return Success(entity);
        }
        
        protected virtual async Task<IHttpActionResult> Save(TQueryableEntity entity)
        {
            if (IsNoActionRecord(entity))
            {
                return Faild(MessageHelper.NoEdit);
            }

            bool result = await _helper.SaveAsync(entity);
            return Success(result);
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> Add(TQueryableEntity entity)
        {
            return await Save(entity);
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> Edit(TQueryableEntity entity)
        {
            return await Save(entity);
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> Delete(int id)
        {
            if (IsNoActionRecord(id))
            {
                return Faild(MessageHelper.NoDelete);
            }

            bool result = await _helper.DeleteAsync(id);
            return Success(result);
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> DeleteList(List<int> idList)
        {
            if (IsNoActionRecord(idList))
            {
                return Faild(MessageHelper.NoDeleteList);
            }
            bool result = await _helper.DeleteListAsync(idList);
            return Success(result);
        }
    }

    public class BaseSessionApiController<THelper, TEntity, TSession> 
        : BaseSessionApiController<THelper, TEntity, TEntity, TSession>
        where THelper : BaseSessionHelper<TEntity, TEntity, TSession>, new()
        where TEntity : class, new()
        where TSession : class
    {
    }

    public class BaseSessionHelperApiController<TEntity, TSession>
       : BaseSessionApiController<BaseSessionHelper<TEntity, TEntity, TSession>, TEntity, TEntity, TSession>
       where TEntity : class, new()
       where TSession : class
    {
    }
    #endregion

    #region BaseApiController
    public class BaseApiController : BaseSessionApiController<object>
    { }

    public class BaseApiController<THelper> : BaseSessionApiController<THelper, object>
         where THelper : DbContextBaseHelper<JerryPlatDbContext, object>, new()
    { }

    public class BaseApiController<THelper, TEntity, IQueryableEntity> : BaseSessionApiController<THelper, TEntity, IQueryableEntity, object>
        where THelper : BaseSessionHelper<TEntity, IQueryableEntity, object>, new()
        where TEntity : class, new()
        where IQueryableEntity : class, new()
    { }

    public class BaseApiController<THelper, TEntity> : BaseApiController<THelper, TEntity, TEntity>
        where THelper : BaseSessionHelper<TEntity, TEntity, object>, new()
        where TEntity : class, new()
    { }

    public class BaseHelperApiController<TEntity>
       : BaseSessionHelperApiController<TEntity, object>
       where TEntity : class, new()
    { }
    #endregion
}
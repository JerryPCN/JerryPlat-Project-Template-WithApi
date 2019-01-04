using JerryPlat.DAL;
using JerryPlat.DAL.Context;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using JerryPlat.Web.App_Start;
using log4net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Base
{
    #region BaseSessionController
    
    public class BaseSessionController<TSession> : Controller
        where TSession : class
    {
        #region Session
        private TSession _session { get; set; }
        protected TSession _Session
        {
            get
            {
                if (_session == null)
                {
                    _session = GetSession();
                }
                return _session;
            }
        }

        private TSession GetSession()
        {
            string strSessionKey = typeof(TSession).Name;
            if (SessionHelper.KeyValues.ContainsKey(strSessionKey))
            {
                return SessionHelper.KeyValues[strSessionKey].GetSession<TSession>();
            }
            return null;
        }
        #endregion

        public virtual async Task<ActionResult> Index(int id = 0)
        {
            return View();
        }

        #region Response Result
        protected ActionResult Success()
        {
            return Return(ResponseModel<string>.Ok(""));
        }

        protected ActionResult Success<T>(T data, string strMessage = "")
        {
            return Return(ResponseModel<T>.Ok(data, strMessage));
        }

        protected ActionResult Faild(string strMsg)
        {
            return Return(ResponseModel.Error(strMsg));
        }

        protected ActionResult Invalid(string strMsg)
        {
            return Return(ResponseModel.Invalid(strMsg));
        }

        protected ActionResult NotFound()
        {
            return Return(ResponseModel.NotFound());
        }

        protected ActionResult RefreshTokenFaild(string strMsg)
        {
            return Return(ResponseModel.RefreshTokenFaild(strMsg));
        }

        protected ActionResult Existed()
        {
            return Return(ResponseModel.Existed());
        }

        private ActionResult Return<T>(ResponseModel<T> responseModel)
        {
            return new JsonNetResult { Data = responseModel, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //return Json(responseModel);
        }

        #endregion Response Result
    }

    public class BaseSessionController<THelper, TSession> : BaseSessionController<TSession>
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

    public class BaseSessionController<THelper, TEntity, TQueryableEntity, TSession> 
            : BaseSessionController<THelper, TSession>
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
            if(noActionIdList == null)
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
        public virtual async Task<ActionResult> GetPageList(SearchModel search, PageParam pageParam)
        {
            PageData<TQueryableEntity> pageData = await _helper.GetPageListAsync(search, PageHelper.GetKeyExpression<TQueryableEntity, int>(new string[] { "OrderIndex", "Id" }), pageParam, true);
            return Success(pageData);
        }

        [HttpPost]
        public virtual async Task<ActionResult> GetList(SearchModel search)
        {
            List<TQueryableEntity> pageData = await _helper.GetListAsync(search);
            return Success(pageData);
        }

        [HttpGet]
        public virtual async Task<ActionResult> GetDetail(int id)
        {
            TQueryableEntity entity = await _helper.GetByIdAsync(id);
            return Success(entity);
        }

        protected virtual async Task<ActionResult> Save(TQueryableEntity entity)
        {
            if (IsNoActionRecord(entity))
            {
                return Faild(MessageHelper.NoEdit);
            }

            bool result = await _helper.SaveAsync(entity);
            return Success(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Add(TQueryableEntity entity)
        {
            return await Save(entity);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Edit(TQueryableEntity entity)
        {
            return await Save(entity);
        }

        [HttpGet]
        public virtual async Task<ActionResult> Delete(int id)
        {
            if (IsNoActionRecord(id))
            {
                return Faild(MessageHelper.NoDelete);
            }

            bool result = await _helper.DeleteAsync(id);
            return Success(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult> DeleteList(List<int> idList)
        {
            if (IsNoActionRecord(idList))
            {
                return Faild(MessageHelper.NoDeleteList);
            }

            bool result = await _helper.DeleteListAsync(idList);
            return Success(result);
        }
    }

    public class BaseSessionController<THelper, TEntity, TSession>
         : BaseSessionController<THelper, TSession>
     where THelper : BaseSessionHelper<TEntity, TEntity, TSession>, new()
     where TEntity : class, new()
     where TSession : class
    {
    }

    public class BaseSessionHelperController<TEntity, TSession> 
        : BaseSessionController<BaseSessionHelper<TEntity, TEntity, TSession>, TEntity, TSession>
        where TEntity : class, new()
        where TSession : class
    {
    }
    #endregion
    
    #region BaseController
    public class BaseController : BaseSessionController<object>
    { }

    public class BaseController<THelper> : BaseSessionController<THelper, object>
        where THelper : DbContextBaseHelper<JerryPlatDbContext, object>, new()
    { }

    public class BaseController<THelper, TEntity, TQueryableEntity> : BaseSessionController<THelper, TEntity, TQueryableEntity, object>
        where THelper : BaseSessionHelper<TEntity, TQueryableEntity, object>, new()
        where TEntity : class, new()
        where TQueryableEntity : class, new()
    { }

    public class BaseController<THelper, TEntity> : BaseController<THelper, TEntity, TEntity>
        where THelper : BaseSessionHelper<TEntity, TEntity, object>, new()
        where TEntity : class, new()
    { }

    public class BaseHelperController<TEntity> : BaseSessionHelperController<TEntity, object>
        where TEntity : class, new()
    { }
    #endregion
}
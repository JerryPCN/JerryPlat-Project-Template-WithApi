using JerryPlat.DAL;
using JerryPlat.DAL.Context;
using JerryPlat.Models;
using JerryPlat.Utils.Helpers;
using JerryPlat.Web.App_Start.Filter;

namespace JerryPlat.Web.Areas.Base
{
    #region AdminBaseController
    public class AdminBaseController : BaseSessionController<AdminUser>
    { }

    public class AdminBaseController<THelper> : BaseSessionController<THelper, AdminUser>
        where THelper : DbContextBaseHelper<JerryPlatDbContext, AdminUser>, new()
    { }

    public class AdminBaseController<THelper, TEntity, TQueryableEntity> : BaseSessionController<THelper, TEntity, TQueryableEntity, AdminUser>
        where THelper : BaseSessionHelper<TEntity, TQueryableEntity, AdminUser>, new()
        where TEntity : class, new()
        where TQueryableEntity : class, new()
    { }

    public class AdminBaseController<THelper, TEntity> : AdminBaseController<THelper, TEntity, TEntity>
    where THelper : BaseSessionHelper<TEntity, TEntity, AdminUser>, new()
    where TEntity : class, new()
    { }

    public class AdminBaseHelperController<TEntity> : BaseSessionHelperController<TEntity, AdminUser>
        where TEntity : class, new()
    { }
    #endregion

    #region AdminAuthurizeBaseController
    [LoginAuthorize]
    public class AdminAuthurizeBaseController : AdminBaseController
    {}

    [LoginAuthorize]
    public class AdminAuthurizeBaseController<THelper> : AdminBaseController<THelper> 
        where THelper : DbContextBaseHelper<JerryPlatDbContext, AdminUser>, new()
    {}

    [LoginAuthorize]
    public class AdminAuthurizeBaseController<THelper, TEntity, TQueryableEntity> : AdminBaseController<THelper, TEntity, TQueryableEntity>
        where THelper : BaseSessionHelper<TEntity, TQueryableEntity, AdminUser>, new()
        where TEntity : class, new()
        where TQueryableEntity : class, new()
    { }

    //[LoginAuthorize]
    public class AdminAuthurizeBaseController<THelper, TEntity> : AdminAuthurizeBaseController<THelper, TEntity, TEntity>
        where THelper : BaseSessionHelper<TEntity, TEntity, AdminUser>, new()
        where TEntity : class, new()
    { }

    [LoginAuthorize]
    public class AdminAuthurizeBaseHelperController<TEntity> : AdminBaseHelperController<TEntity>
        where TEntity : class, new()
    {}
    #endregion
}
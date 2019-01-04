using JerryPlat.DAL;
using JerryPlat.DAL.Context;
using JerryPlat.Models;
using JerryPlat.Utils.Helpers;
using JerryPlat.Web.App_Start.Filter;

namespace JerryPlat.Web.Areas.Base
{
    #region MobBaseController
    public class MobBaseController : BaseSessionController<Member>
    { }

    public class MobBaseController<THelper> : BaseSessionController<THelper, Member>
        where THelper : DbContextBaseHelper<JerryPlatDbContext, Member>, new()
    { }

    public class MobBaseController<THelper, TEntity, TQueryableEntity> : BaseSessionController<THelper, TEntity, TQueryableEntity, Member>
        where THelper : BaseSessionHelper<TEntity, TQueryableEntity, Member>, new()
        where TEntity : class, new()
        where TQueryableEntity : class, new()
    { }

    public class MobBaseController<THelper, TEntity> : MobBaseController<THelper, TEntity, TEntity>
        where THelper : BaseSessionHelper<TEntity, TEntity, Member>, new()
        where TEntity : class, new()
    { }

    public class MobBaseHelperController<TEntity> : BaseSessionHelperController<TEntity, Member>
        where TEntity : class, new()
    { }
    #endregion

    #region MobAuthurizeBaseController
    [LoginAuthorize]
    public class MobAuthurizeBaseController : MobBaseController
    { }

    [LoginAuthorize]
    public class MobAuthurizeBaseController<THelper> : MobBaseController<THelper>
        where THelper : DbContextBaseHelper<JerryPlatDbContext, Member>, new()
    { }

    [LoginAuthorize]
    public class MobAuthurizeBaseController<THelper, TEntity, TQueryableEntity> : MobBaseController<THelper, TEntity, TQueryableEntity>
        where THelper : BaseSessionHelper<TEntity, TQueryableEntity, Member>, new()
        where TEntity : class, new()
        where TQueryableEntity : class, new()
    { }

    //[LoginAuthorize]
    public class MobAuthurizeBaseController<THelper, TEntity> : MobAuthurizeBaseController<THelper, TEntity, TEntity>
        where THelper : BaseSessionHelper<TEntity, TEntity, Member>, new()
        where TEntity : class, new()
    { }

    [LoginAuthorize]
    public class MobAuthurizeBaseHelperController<TEntity> : MobBaseHelperController<TEntity>
        where TEntity : class, new()
    { }
    #endregion
}
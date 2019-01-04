using JerryPlat.DAL;
using JerryPlat.DAL.Context;
using JerryPlat.Models;
using JerryPlat.Utils.Helpers;
using JerryPlat.Web.App_Start.Filter;

namespace JerryPlat.Web.Areas.Base
{
    [LoginAuthorize]
    public class AuthurizeBaseController<TSession> : BaseSessionController<TSession>
        where TSession : class
    { }

    [LoginAuthorize]
    public class AuthurizeBaseController<THelper,TSession> : BaseSessionController<THelper, TSession>
        where THelper : DbContextBaseHelper<JerryPlatDbContext, TSession>, new()
        where TSession : class
    { }

    [LoginAuthorize]
    public class AuthurizeBaseController<THelper, TEntity, TSession> : BaseSessionController<THelper, TEntity, TSession>
        where THelper : BaseSessionHelper<TEntity, TSession>, new()
        where TEntity : class, new()
        where TSession : class
    { }

    [LoginAuthorize]
    public class AuthurizeBaseHelperController<TEntity, TSession> : BaseSessionHelperController<TEntity, TSession>
        where TEntity : class, new()
        where TSession :class
    { }
}
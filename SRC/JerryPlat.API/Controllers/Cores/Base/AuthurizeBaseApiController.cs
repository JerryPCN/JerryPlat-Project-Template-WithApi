using JerryPlat.DAL;
using JerryPlat.DAL.Context;
using JerryPlat.Owin.Attributes;
using JerryPlat.Utils.Helpers;

namespace JerryPlat.API.Controllers
{
    #region AuthurizeBaseApiController
    [TokenAuthorize]
    public class AuthurizeBaseApiController<TSession> : BaseSessionApiController<TSession>
        where TSession : class
    {
    }

    [TokenAuthorize]
    public class AuthurizeBaseApiController<THelper, TSession> : BaseSessionApiController<THelper, TSession>
        where THelper : DbContextBaseHelper<JerryPlatDbContext, TSession>, new()
        where TSession : class
    {
    }

    [TokenAuthorize]
    public class AuthurizeBaseApiController<THelper, TEntity, TQueryableEntity, TSession> : BaseSessionApiController<THelper, TEntity, TQueryableEntity, TSession>
        where THelper : BaseSessionHelper<TEntity, TQueryableEntity, TSession>, new()
        where TEntity : class, new()
        where TQueryableEntity : class, new()
        where TSession : class
    {
    }

    //[TokenAuthorize]
    public class AuthurizeBaseApiController<THelper, TEntity, TSession> : AuthurizeBaseApiController<THelper, TEntity, TEntity, TSession>
        where THelper : BaseSessionHelper<TEntity, TEntity, TSession>, new()
        where TEntity : class, new()
        where TSession : class
    {
    }

    [TokenAuthorize]
    public class AuthurizeBaseHelperApiController<TEntity, TSession> : BaseSessionHelperApiController<TEntity, TSession>
        where TEntity : class, new()
        where TSession : class
    {
    }
    #endregion
}
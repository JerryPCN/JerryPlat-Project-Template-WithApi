using JerryPlat.DAL.Context;
using JerryPlat.Models;
using JerryPlat.Utils.Helpers;

namespace JerryPlat.DAL
{
    #region BaseSessionHelper
    public class BaseSessionHelper<TSession> : DbContextBaseHelper<JerryPlatDbContext, TSession>
        where TSession : class
    { }

    public class BaseSessionHelper<TEntity, TQueryableEntity, TSession> : DbContextBaseHelper<JerryPlatDbContext, TEntity, TQueryableEntity, TSession>
        where TEntity : class, new()
        where TQueryableEntity : class, new()
        where TSession : class
    { }

    public class BaseSessionHelper<TEntity, TSession> : BaseSessionHelper<TEntity, TEntity, TSession>
        where TEntity : class, new()
        where TSession : class
    { }
    #endregion

    #region AdminBaseSessionHelper
    public class AdminBaseSessionHelper : BaseSessionHelper<AdminUser>
    { }

    public class AdminBaseSessionHelper<TEntity, TQueryableEntity> : BaseSessionHelper<TEntity, TQueryableEntity, AdminUser>
        where TEntity : class, new()
        where TQueryableEntity : class, new()
    { }

    public class AdminBaseSessionHelper<TEntity> : AdminBaseSessionHelper<TEntity, TEntity>
     where TEntity : class, new()
    { }
    #endregion


    #region MobBaseSessionHelper
    public class MobBaseSessionHelper : BaseSessionHelper<Member>
    { }

    public class MobBaseSessionHelper<TEntity, TQueryableEntity> : BaseSessionHelper<TEntity, TQueryableEntity, Member>
       where TEntity : class, new()
       where TQueryableEntity : class, new()
    { }

    public class MobBaseSessionHelper<TEntity> : MobBaseSessionHelper<TEntity, TEntity>
        where TEntity : class, new()
    { }
    #endregion

    #region BaseHelper
    public class BaseHelper : BaseSessionHelper<object>
    { }

    public class BaseHelper<TEntity, TQueryableEntity> : BaseSessionHelper<TEntity, TQueryableEntity, object>
        where TEntity : class, new()
        where TQueryableEntity : class, new()
    { }

    public class BaseHelper<TEntity> : BaseHelper<TEntity, TEntity>
        where TEntity : class, new()
    { }
    #endregion
}
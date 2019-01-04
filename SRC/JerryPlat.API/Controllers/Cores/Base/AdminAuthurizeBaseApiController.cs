using JerryPlat.DAL;
using JerryPlat.DAL.Context;
using JerryPlat.Models;
using JerryPlat.Utils.Helpers;

namespace JerryPlat.API.Controllers
{
    #region AdminAuthurizeBaseApiController
    public class AdminAuthurizeBaseApiController : AuthurizeBaseApiController<AdminUser> 
    {
    }
    
    public class AdminAuthurizeBaseApiController<THelper> : AuthurizeBaseApiController<THelper, AdminUser> 
        where THelper : DbContextBaseHelper<JerryPlatDbContext, AdminUser>, new()
    {
    }
    
    public class AdminAuthurizeBaseApiController<THelper, TEntity, TQueryableEntity> : AuthurizeBaseApiController<THelper, TEntity, TQueryableEntity, AdminUser> 
        where THelper : BaseSessionHelper<TEntity, TQueryableEntity, AdminUser>, new()
        where TEntity : class, new()
        where TQueryableEntity : class, new()
    {
    }
    
    public class AdminAuthurizeBaseApiController<THelper, TEntity> : AdminAuthurizeBaseApiController<THelper, TEntity, TEntity> 
        where THelper : BaseSessionHelper<TEntity, TEntity, AdminUser>, new()
        where TEntity : class, new()
    {
    }
    
    public class AdminAuthurizeBaseHelperApiController<TEntity> : AuthurizeBaseHelperApiController<TEntity, AdminUser>
        where TEntity : class, new()
    {
    }
    #endregion
}
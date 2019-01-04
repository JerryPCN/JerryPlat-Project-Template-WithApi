using AutoMapper;
using AutoMapper.QueryableExtensions;
using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace JerryPlat.Utils.Helpers
{
    public class DbContextBaseHelper<TDbContext> : IDisposable
       where TDbContext : DbContext, new()
    {
        #region ORM

        private TDbContext db;
        protected TDbContext _Db => db ?? (db = SingleInstanceHelper.GetInstance<TDbContext>());

        protected void ResetDbContext()
        {
            Dispose();
            db = SingleInstanceHelper.GetInstance<TDbContext>(true);
        }

        public virtual DbSet<TEntity> GetDbSet<TEntity>()
            where TEntity : class, new()
        {
            return _Db.Set<TEntity>();
        }

        public virtual void Attach<TEntity>(TEntity entity)
            where TEntity : class, new()
        {
            GetDbSet<TEntity>().Attach(entity);
        }

        public virtual void Attach<TEntity>(TEntity entity, EntityState entityState)
              where TEntity : class, new()
        {
            Attach(GetDbSet<TEntity>(), entity, entityState);
        }

        //http://www.cnblogs.com/scy251147/p/3688844.html
        public virtual void Detach<TEntity>(TEntity entity)
            where TEntity : class
        {
            var objContext = ((IObjectContextAdapter)_Db).ObjectContext;
            var objSet = objContext.CreateObjectSet<TEntity>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);

            object foundEntity;
            var exists = objContext.TryGetObjectByKey(entityKey, out foundEntity);

            if (exists)
            {
                objContext.Detach(foundEntity);
            }
        }

        public virtual void Attach<TEntity>(DbSet<TEntity> entities, TEntity entity, EntityState entityState)
               where TEntity : class, new()
        {
            Detach(entity);

            entities.Attach(entity);
            _Db.Entry(entity).State = entityState;
        }

        public virtual IQueryable<TEntity> GetQueryableList<TEntity>(IQueryable<TEntity> queryEntity = null)
           where TEntity : class, new()
        {
            return queryEntity ?? (GetDbSet<TEntity>().AsNoTracking() as IQueryable<TEntity>);
        }

        public virtual IQueryable<TEntity> GetQueryableList<TEntity>(SearchModel seachModel, IQueryable<TEntity> queryEntity = null)
            where TEntity : class, new()
        {
            queryEntity = GetQueryableList(queryEntity);

            return PageHelper.GetOrderQueryable(queryEntity, seachModel?.Sort);
        }

        public virtual IQueryable<TEntity> GetQueryableList<TEntity, TKey>(Expression<Func<TEntity, TKey>> orderByKeySelector, bool bIsAscOrder = true, string strOrder = "", IQueryable<TEntity> queryEntity = null)
          where TEntity : class, new()
        {
            queryEntity = GetQueryableList(queryEntity);
            return PageHelper.GetOrderQueryable(queryEntity, orderByKeySelector, bIsAscOrder, strOrder);
        }

        public virtual IQueryable<TEntity> GetQueryableList<TEntity, TKey>(SearchModel seachModel, Expression<Func<TEntity, TKey>> orderByKeySelector, bool bIsAscOrder = true, IQueryable<TEntity> queryEntity = null)
            where TEntity : class, new()
        {
            return GetQueryableList(orderByKeySelector, bIsAscOrder, seachModel?.Sort, queryEntity);
        }

        public virtual List<TEntity> GetList<TEntity>(IQueryable<TEntity> queryEntity = null)
         where TEntity : class, new()
        {
            queryEntity = GetQueryableList(queryEntity);
            return queryEntity.ToList();
        }

        public virtual async Task<List<TEntity>> GetListAsync<TEntity>(IQueryable<TEntity> queryEntity = null)
            where TEntity : class, new()
        {
            queryEntity = GetQueryableList(queryEntity);
            return await queryEntity.ToListAsync();
        }

        public virtual List<TEntity> GetList<TEntity>(SearchModel seachModel, IQueryable<TEntity> queryEntity = null)
            where TEntity : class, new()
        {
            queryEntity = GetQueryableList(seachModel, queryEntity);
            return queryEntity.ToList();
        }

        public virtual async Task<List<TEntity>> GetListAsync<TEntity>(SearchModel seachModel, IQueryable<TEntity> queryEntity = null)
            where TEntity : class, new()
        {
            queryEntity = GetQueryableList(seachModel, queryEntity);
            return await queryEntity.ToListAsync();
        }

        public virtual List<TEntity> GetList<TEntity, TKey>(Expression<Func<TEntity, TKey>> orderByKeySelector, bool bIsAscOrder = true, IQueryable<TEntity> queryEntity = null)
            where TEntity : class, new()
        {
            queryEntity = GetQueryableList(orderByKeySelector, bIsAscOrder, null, queryEntity);
            return queryEntity.ToList();
        }

        public virtual async Task<List<TEntity>> GetListAsync<TEntity, TKey>(Expression<Func<TEntity, TKey>> orderByKeySelector, bool bIsAscOrder = true, IQueryable<TEntity> queryEntity = null)
            where TEntity : class, new()
        {
            queryEntity = GetQueryableList(orderByKeySelector, bIsAscOrder, null, queryEntity);
            return await queryEntity.ToListAsync();
        }

        public virtual List<TEntity> GetList<TEntity, TKey>(SearchModel seachModel, Expression<Func<TEntity, TKey>> orderByKeySelector, bool bIsAscOrder = true, IQueryable<TEntity> queryEntity = null)
            where TEntity : class, new()
        {
            queryEntity = GetQueryableList(seachModel, orderByKeySelector, bIsAscOrder, queryEntity);
            return queryEntity.ToList();
        }

        public virtual async Task<List<TEntity>> GetListAsync<TEntity, TKey>(SearchModel seachModel, Expression<Func<TEntity, TKey>> orderByKeySelector, bool bIsAscOrder = true, IQueryable<TEntity> queryEntity = null)
            where TEntity : class, new()
        {
            queryEntity = GetQueryableList(seachModel, orderByKeySelector, bIsAscOrder, queryEntity);
            return await queryEntity.ToListAsync();
        }

        public virtual PageData<TEntity> GetPageList<TEntity>(SearchModel searchModel, PageParam pageParam, bool bIsAscOrder = true, IQueryable<TEntity> queryEntity = null)
            where TEntity : class, new()
        {
            Expression<Func<TEntity, int>> keySelector = PageHelper.GetDefaultKeyExpression<TEntity, int>();
            IQueryable<TEntity> queryList = GetQueryableList(searchModel, queryEntity);
            return PageHelper.GetPageData(queryList, keySelector, pageParam, bIsAscOrder, searchModel?.Sort);
        }

        public virtual async Task<PageData<TEntity>> GetPageListAsync<TEntity>(SearchModel searchModel, PageParam pageParam, bool bIsAscOrder = true, IQueryable<TEntity> queryEntity = null)
           where TEntity : class, new()
        {
            Expression<Func<TEntity, int>> keySelector = PageHelper.GetDefaultKeyExpression<TEntity, int>();
            IQueryable<TEntity> queryList = GetQueryableList(searchModel, queryEntity);
            return await PageHelper.GetPageDataAsync(queryList, keySelector, pageParam, bIsAscOrder, searchModel?.Sort);
        }

        public virtual PageData<TEntity> GetPageList<TEntity, TKey>(SearchModel searchModel, Expression<Func<TEntity, TKey>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true, IQueryable<TEntity> queryEntity = null)
            where TEntity : class, new()
        {
            IQueryable<TEntity> queryList = GetQueryableList(searchModel, queryEntity);
            return PageHelper.GetPageData(queryList, orderByKeySelector, pageParam, bIsAscOrder, searchModel?.Sort);
        }

        public virtual async Task<PageData<TEntity>> GetPageListAsync<TEntity, TKey>(SearchModel searchModel, Expression<Func<TEntity, TKey>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true, IQueryable<TEntity> queryEntity = null)
        where TEntity : class, new()
        {
            IQueryable<TEntity> queryList = GetQueryableList(searchModel, queryEntity);
            return await PageHelper.GetPageDataAsync(queryList, orderByKeySelector, pageParam, bIsAscOrder, searchModel?.Sort);
        }

        public virtual bool ExistById<TEntity>(int id, IQueryable<TEntity> entities = null)
            where TEntity : class, new()
        {
            entities = entities ?? GetDbSet<TEntity>();
            Expression<Func<TEntity, bool>> predicate = PageHelper.GetPredicateById<TEntity>(id);
            return entities.Any(predicate);
        }

        public virtual async Task<bool> ExistByIdAsync<TEntity>(int id, IQueryable<TEntity> entities = null)
          where TEntity : class, new()
        {
            entities = entities ?? GetDbSet<TEntity>();
            Expression<Func<TEntity, bool>> predicate = PageHelper.GetPredicateById<TEntity>(id);
            return await entities.AnyAsync(predicate);
        }

        public virtual TEntity GetById<TEntity>(int id, IQueryable<TEntity> entities = null, bool bIsNotNull = true)
          where TEntity : class, new()
        {
            entities = entities ?? GetDbSet<TEntity>();
            Expression<Func<TEntity, bool>> predicate = PageHelper.GetPredicateById<TEntity>(id);
            TEntity entity = entities.FirstOrDefault(predicate);
            if (bIsNotNull && entity == null)
            {
                throw new Exception("Not exist [" + typeof(TEntity).Name + "] entity with Id=" + id);
            }
            return entity;
        }

        public virtual async Task<TEntity> GetByIdAsync<TEntity>(int id, IQueryable<TEntity> entities = null, bool bIsNotNull = true)
             where TEntity : class, new()
        {
            entities = entities ?? GetDbSet<TEntity>();
            Expression<Func<TEntity, bool>> predicate = PageHelper.GetPredicateById<TEntity>(id);
            TEntity entity = await entities.FirstOrDefaultAsync(predicate);
            if (bIsNotNull && entity == null)
            {
                throw new Exception("Not exist [" + typeof(TEntity).Name + "] entity with Id=" + id);
            }
            return entity;
        }

        public virtual TEntity GetDetail<TEntity>(int id, IQueryable<TEntity> entities = null, bool bIsNotNull = true)
          where TEntity : class, new()
        {
            return GetById(id, entities, bIsNotNull);
        }

        public virtual async Task<TEntity> GetDetailAsync<TEntity>(int id, IQueryable<TEntity> entities = null, bool bIsNotNull = true)
             where TEntity : class, new()
        {
            return await GetByIdAsync(id, entities, bIsNotNull);
        }

        public virtual List<TEntity> GetByIdList<TEntity>(List<int> idList, IQueryable<TEntity> entities = null)
            where TEntity : class, new()
        {
            entities = entities ?? GetDbSet<TEntity>();
            Expression<Func<TEntity, bool>> predicate = PageHelper.GetPredicateByIdList<TEntity>(idList);
            List<TEntity> entityList = entities.Where(predicate).ToList();
            return entityList;
        }

        public virtual async Task<List<TEntity>> GetByIdListAsync<TEntity>(List<int> idList, IQueryable<TEntity> entities = null)
            where TEntity : class, new()
        {
            entities = entities ?? GetDbSet<TEntity>();
            Expression<Func<TEntity, bool>> predicate = PageHelper.GetPredicateByIdList<TEntity>(idList);
            List<TEntity> entityList = await entities.Where(predicate).ToListAsync();
            return entityList;
        }

        public bool Delete<TEntity>(int id, DbSet<TEntity> entities = null)
            where TEntity : class, new()
        {
            entities = entities ?? GetDbSet<TEntity>();
            TEntity entity = GetById<TEntity>(id);
            return Delete(entity, entities);
        }
        
        public void DeleteOnly<TEntity>(TEntity entity, DbSet<TEntity> entities = null)
            where TEntity : class, new()
        {
            entities = entities ?? GetDbSet<TEntity>();
            Attach(entities, entity, EntityState.Deleted);
        }

        public bool Delete<TEntity>(TEntity entity, DbSet<TEntity> entities = null, bool bIsSaveChanges = true)
            where TEntity : class, new()
        {
            DeleteOnly(entity, entities);
            return SaveChanges(bIsSaveChanges);
        }

        public bool DeleteList<TEntity>(List<int> idList, DbSet<TEntity> entities = null)
            where TEntity : class, new()
        {
            entities = entities ?? GetDbSet<TEntity>();
            List<TEntity> entityList = GetByIdList<TEntity>(idList);
            return DeleteList(entityList, entities);
        }

        public bool DeleteList<TEntity>(List<TEntity> entityList, DbSet<TEntity> entities = null)
           where TEntity : class, new()
        {
            foreach (TEntity entity in entityList)
            {
                Delete(entity, entities, false);
            }
            return SaveChanges();
        }

        public async Task<bool> DeleteAsync<TEntity>(int id, DbSet<TEntity> entities = null)
            where TEntity : class, new()
        {
            entities = entities ?? GetDbSet<TEntity>();
            TEntity entity = await GetByIdAsync<TEntity>(id);
            return await DeleteAsync(entity, entities);
        }

        public async Task<bool> DeleteAsync<TEntity>(TEntity entity, DbSet<TEntity> entities = null, bool bIsSaveChanges = true)
         where TEntity : class, new()
        {
            DeleteOnly(entity, entities);
            return await SaveChangesAsync(bIsSaveChanges);
        }

        public async Task<bool> DeleteListAsync<TEntity>(List<int> idList, DbSet<TEntity> entities = null)
            where TEntity : class, new()
        {
            entities = entities ?? GetDbSet<TEntity>();
            List<TEntity> entityList = await GetByIdListAsync<TEntity>(idList);
            return await DeleteListAsync(entityList, entities);
        }

        public async Task<bool> DeleteListAsync<TEntity>(List<TEntity> entityList, DbSet<TEntity> entities = null)
           where TEntity : class, new()
        {
            foreach (TEntity item in entityList)
            {
                await DeleteAsync(item, entities, false);
            }
            return await SaveChangesAsync();
        }

        protected void CheckNullEntity<TEntity>(TEntity entity)
            where TEntity : class, new()
        {
            if (entity == null)
            {
                throw new Exception("[" + typeof(TEntity).Name + "] entity can not be null!");
            }
        }

        public void Change<TEntity>(TEntity entity, DbSet<TEntity> entities = null)
            where TEntity : class, new()
        {
            CheckNullEntity(entity);

            entities = entities ?? GetDbSet<TEntity>();

            if (PageHelper.IsAdd<TEntity, int>(entity))
            {
                entities.Add(entity);
            }
            else
            {
                Attach(entities, entity, EntityState.Modified);
            }
        }

        public bool Save<TEntity>(TEntity entity, DbSet<TEntity> entities = null)
            where TEntity : class, new()
        {
            Change(entity, entities);
            return SaveChanges();
        }

        public async Task<bool> SaveAsync<TEntity>(TEntity entity, DbSet<TEntity> entities = null)
             where TEntity : class, new()
        {
            Change(entity, entities);
            return await SaveChangesAsync();
        }

        public bool SaveChanges(bool bIsSaveChanges = true)
        {
            if (bIsSaveChanges)
            {
                return (_Db.SaveChanges()) > 0;
            }

            return true;
        }

        public async Task<bool> SaveChangesAsync(bool bIsSaveChanges = true)
        {
            if (bIsSaveChanges)
            {
                return (await _Db.SaveChangesAsync()) > 0;
            }
            return true;
        }

        #endregion ORM

        #region Dispose
        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }
        #endregion Dispose
    }

    public class DbContextBaseHelper<TDbContext, TSession> : DbContextBaseHelper<TDbContext>
        where TDbContext : DbContext, new()
        where TSession : class
    {
        #region Session
        private TSession _session { get; set; }
        protected TSession _Session => _session ?? (_session = GetSession());

        private bool? isFromApi { get; set; }
        private bool _IsFromApi
        {
            get
            {
                if (isFromApi == null)
                {
                    isFromApi = Thread.CurrentPrincipal != null
                        && (Thread.CurrentPrincipal as ClaimsPrincipal).Claims.Any();
                }
                return isFromApi.Value;
            }
        }
        private string _sessionKey { get { return this._IsFromApi ? null : typeof(TSession).Name; } }
        private TSession GetSession()
        {
            if (this._IsFromApi)
            {
                ClaimsPrincipal clainsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
                if (clainsPrincipal != null)
                {
                    Claim claim = clainsPrincipal.Claims.Where(o => o.Type == ClaimTypes.UserData).FirstOrDefault();
                    if (claim != null)
                    {
                        return SerializationHelper.JsonToObject<TSession>(claim.Value);
                    }
                }
            }
            else if (SessionHelper.KeyValues.ContainsKey(this._sessionKey))
            {
                return SessionHelper.KeyValues[this._sessionKey].GetSession<TSession>();
            }

            return null;
        }
        public void InitSession(TSession session)
        {
            this._session = session;
        }
        
        public virtual void SetSession(object session)
        {
            if(!(session is TSession))
            {
                return;
            }

            InitSession(session as TSession);
            if (!string.IsNullOrEmpty(this._sessionKey))
            {
                if (!SessionHelper.KeyValues.ContainsKey(this._sessionKey))
                {
                    throw new Exception("Not exist Session with Key = " + this._sessionKey);
                }
                SessionHelper.KeyValues[this._sessionKey].SetSession(session);
            }
        }

        public bool IsNullSession()
        {
            return this._Session == null;
        }
        #endregion
    }

    public class DbContextBaseHelper<TDbContext, TEntity, TQueryableEntity, TSession> : DbContextBaseHelper<TDbContext, TSession>
        where TDbContext : DbContext, new()
        where TEntity : class, new()
        where TQueryableEntity : class, new()
        where TSession : class
    {
        #region private Fields

        private DbSet<TEntity> _entities;
        private DbSet<TEntity> _Entites => _entities ?? (_entities = GetDbSet<TEntity>());

        private bool? _isSameEntity;
        private bool _IsSameEntity => _isSameEntity ?? (_isSameEntity = typeof(TEntity) == typeof(TQueryableEntity)).Value;

        #endregion private Fields

        #region private functions

        public DbSet<TEntity> GetEntities()
        {
            return _Entites;
        }

        //protected override void CheckEntityIfDiffThenMustOverride()
        //{
        //    if (!_IsSameEntity)
        //    {
        //        throw new Exception("This method must be override!");
        //    }
        //}

        #endregion private functions

        #region public functions
        public virtual IQueryable<TQueryableEntity> GetQueryableList(SearchModel seachModel)
        {
            if (_IsSameEntity)
            {
                return _Entites as IQueryable<TQueryableEntity>;
            }

            return _Entites.ProjectTo<TQueryableEntity>();
        }

        public virtual List<TQueryableEntity> GetList(SearchModel seachModel)
        {
            return GetList(seachModel, GetQueryableList(seachModel));
        }

        public virtual async Task<List<TQueryableEntity>> GetListAsync(SearchModel seachModel)
        {
            return await GetListAsync(seachModel, GetQueryableList(seachModel));
        }

        public virtual List<TQueryableEntity> GetList<TKey>(SearchModel seachModel, Expression<Func<TQueryableEntity, TKey>> orderByKeySelector, bool bIsAscOrder = true)
        {
            return GetList(seachModel, orderByKeySelector, bIsAscOrder, GetQueryableList(seachModel));
        }

        public virtual async Task<List<TQueryableEntity>> GetListAsync<TKey>(SearchModel seachModel, Expression<Func<TQueryableEntity, TKey>> orderByKeySelector, bool bIsAscOrder = true)
        {
            return await GetListAsync(seachModel, orderByKeySelector, bIsAscOrder, GetQueryableList(seachModel));
        }
        
        public virtual PageData<TQueryableEntity> GetPageList(SearchModel seachModel, PageParam pageParam, bool bIsAscOrder = true)
        {
            return GetPageList(seachModel, pageParam, bIsAscOrder, GetQueryableList(seachModel));
        }

        public virtual async Task<PageData<TQueryableEntity>> GetPageListAsync(SearchModel seachModel, PageParam pageParam, bool bIsAscOrder = true)
        {
            return await GetPageListAsync(seachModel, pageParam, bIsAscOrder, GetQueryableList(seachModel));
        }

        public virtual PageData<TQueryableEntity> GetPageList<TKey>(SearchModel seachModel, Expression<Func<TQueryableEntity, TKey>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true)
        {
            return GetPageList(seachModel, orderByKeySelector, pageParam, bIsAscOrder, GetQueryableList(seachModel));
        }

        public virtual async Task<PageData<TQueryableEntity>> GetPageListAsync<TKey>(SearchModel seachModel, Expression<Func<TQueryableEntity, TKey>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true)
        {
            return await GetPageListAsync(seachModel, orderByKeySelector, pageParam, bIsAscOrder, GetQueryableList(seachModel));
        }

        public virtual TQueryableEntity GetById(int id, bool bIsNotNull = true)
        {
            return GetDetail(id, GetQueryableList(SearchModel.Instance), bIsNotNull);
        }

        public virtual async Task<TQueryableEntity> GetByIdAsync(int id, bool bIsNotNull = true)
        {
            return await GetDetailAsync(id, GetQueryableList(SearchModel.Instance), bIsNotNull);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync<TEntity>(id, null);
        }

        public virtual async Task<bool> DeleteListAsync(List<int> idList)
        {
            return await DeleteListAsync<TEntity>(idList, null);
        }
        
        public virtual bool Save(TQueryableEntity queryableEntity)
        {
            TEntity entity = Mapper.Map<TQueryableEntity, TEntity>(queryableEntity);
            return Save(entity, null);
        }
        public virtual async Task<bool> SaveAsync(TQueryableEntity queryableEntity)
        {
            TEntity entity = Mapper.Map<TQueryableEntity, TEntity>(queryableEntity);
            return await SaveAsync(entity, null);
        }
        #endregion public functions
    }

    //public class DbContextBaseHelper<TDbContext, TEntity, TSession> 
    //    : DbContextBaseHelper<TDbContext, TEntity, TEntity, TSession>
    //    where TDbContext : DbContext, new()
    //    where TEntity : class, new()
    //    where TSession : class
    //{
    //}
}
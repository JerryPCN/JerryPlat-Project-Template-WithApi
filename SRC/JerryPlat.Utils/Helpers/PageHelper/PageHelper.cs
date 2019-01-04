using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace JerryPlat.Utils.Helpers
{
    public static class PageHelper
    {
        #region Public Method For IEntity Interface

        public static Expression<Func<IEntity, bool>> GetPredicateById(int id)
        {
            return o => o.Id == id;
        }

        public static bool IsAdd(IEntity entity)
        {
            return entity.Id == 0;
        }

        public static Expression<Func<IEntity, int>> GetDefaultOrderRule()
        {
            return o => o.Id;
        }

        #endregion Public Method For IEntity Interface

        #region Public Method For TEntity

        public static Expression<Func<TEntity, bool>> GetPredicate<TEntity, TKey>(string strKeyProp, TKey id) where TEntity : class
        {
            PropertyInfo idProp = TypeHelper.GetPropertyInfo<TEntity>(strKeyProp);
            if(idProp == null)
            {
                return null;
            }

            ParameterExpression param = Expression.Parameter(typeof(TEntity), "o");

            MemberExpression left = Expression.Property(param, idProp);

            ConstantExpression right = Expression.Constant(id);
            BinaryExpression equal = Expression.Equal(left, right);

            Expression<Func<TEntity, bool>> Predicate = Expression.Lambda<Func<TEntity, bool>>(equal, param);

            return Predicate;
        }

        public static Expression<Func<TEntity, bool>> GetPredicate<TEntity, TKey>(string strKeyProp, ICollection<TKey> idList) where TEntity : class
        {
            PropertyInfo idProp = TypeHelper.GetPropertyInfo<TEntity>(strKeyProp);
            if(idProp == null)
            {
                return null;
            }

            ParameterExpression param = Expression.Parameter(typeof(TEntity), "o");
            
            MemberExpression right = Expression.Property(param, idProp);

            ConstantExpression left = Expression.Constant(idList);

            MethodInfo method = typeof(ICollection<TKey>).GetMethod("Contains", new[] { typeof(TKey) });
            MethodCallExpression containsMethodExp = Expression.Call(left, method, right);

            Expression<Func<TEntity, bool>> Predicate = Expression.Lambda<Func<TEntity, bool>>(containsMethodExp, param);

            return Predicate;
        }

        public static Expression<Func<TEntity, bool>> GetPredicateById<TEntity>(int id) where TEntity : class
        {
            return GetPredicate<TEntity, int>("Id", id);
        }

        public static Expression<Func<TEntity, bool>> GetPredicateByIdList<TEntity>(ICollection<int> idList) where TEntity : class
        {
            return GetPredicate<TEntity, int>("Id", idList);
        }

        public static bool IsAdd<TEntity, TKey>(TEntity entity) where TEntity : class
        {
            PropertyInfo idProp = TypeHelper.GetIdPropertyInfo<TEntity>();
            object objValue = idProp.GetValue(entity);
            return ((TKey)objValue).Equals(default(TKey));
        }

        public static Expression<Func<TEntity, TKey>> GetKeyExpression<TEntity, TKey>(string strKey) where TEntity : class
        {
            return GetKeyExpression<TEntity, TKey>(new string[] { strKey });
        }

        public static Expression<Func<TEntity, TKey>> GetKeyExpression<TEntity, TKey>(string[] aryStrKey) where TEntity : class
        {
            PropertyInfo keyProp = TypeHelper.GetPropertyInfo<TEntity>(aryStrKey);
            if(keyProp == null)
            {
                return null;
            }

            ParameterExpression param = Expression.Parameter(typeof(TEntity), "o");
            
            MemberExpression left = Expression.MakeMemberAccess(param, keyProp);

            Expression<Func<TEntity, TKey>> keySelector = Expression.Lambda<Func<TEntity, TKey>>(left, param);

            return keySelector;
        }

        public static Expression<Func<TEntity, TKey>> GetKeyExpression<TEntity, TKey>(PropertyInfo keyProp) where TEntity : class
        {
            ParameterExpression param = Expression.Parameter(typeof(TEntity), "o");

            MemberExpression left = Expression.MakeMemberAccess(param, keyProp);

            Expression<Func<TEntity, TKey>> keySelector = Expression.Lambda<Func<TEntity, TKey>>(left, param);

            return keySelector;
        }

        public static Expression<Func<TEntity, TKey>> GetDefaultKeyExpression<TEntity, TKey>() where TEntity : class
        {
            return GetKeyExpression<TEntity, TKey>("Id");
        }

        #endregion Public Method For TEntity

        #region Page
        /// <summary>
        /// GetOrderQueryable
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="querableList"></param>
        /// <param name="orderByKeySelector"></param>
        /// <param name="bIsAscOrder"></param>
        /// <param name="strOrder">Id Asc, Date Desc</param>
        /// <returns>IQueryable<TEntity></returns>
        public static IQueryable<TEntity> GetOrderQueryable<TEntity>(IQueryable<TEntity> querableList, string strOrder = "")
        {
            IQueryable<TEntity> orderQuerableList = querableList;
            if (!string.IsNullOrEmpty(strOrder))
            {
                orderQuerableList = querableList.OrderBy(strOrder);
            }
            return orderQuerableList;
        }

        /// <summary>
        /// GetOrderQueryable
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="querableList"></param>
        /// <param name="orderByKeySelector"></param>
        /// <param name="bIsAscOrder"></param>
        /// <param name="strOrder">Id Asc, Date Desc</param>
        /// <returns>IQueryable<TEntity></returns>
        public static IQueryable<TEntity> GetOrderQueryable<TEntity, TKey>(IQueryable<TEntity> querableList
            , Expression<Func<TEntity, TKey>> orderByKeySelector
            , bool bIsAscOrder = true, string strOrder = ""
            )
        {
            IQueryable<TEntity> orderQuerableList = null;
            if (string.IsNullOrEmpty(strOrder))
            {
                if (orderByKeySelector != null)
                {
                    orderQuerableList = bIsAscOrder
                                        ?
                                        querableList.OrderBy(orderByKeySelector)
                                        :
                                        querableList.OrderByDescending(orderByKeySelector);
                }
            }
            else
            {
                orderQuerableList = querableList.OrderBy(strOrder);
            }
            return orderQuerableList;
        }


        /// <summary>
        /// GetPageData
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="querableList"></param>
        /// <param name="pageParam"></param>
        /// <returns>IQueryable<TEntity></returns>
        public static IQueryable<TEntity> GetPageData<TEntity>(IQueryable<TEntity> orderQuerableList, PageParam pageParam)
        {
            int intPageSkip = pageParam.GetPageSkip();
            return orderQuerableList.Skip(intPageSkip).Take(pageParam.PageSize);
        }
        
        /// <summary>
        /// GetPageData
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="querableList"></param>
        /// <param name="orderByKeySelector"></param>
        /// <param name="pageParam"></param>
        /// <param name="bIsAscOrder"></param>
        /// <param name="strOrder">Id Asc, Date Desc</param>
        /// <returns>PageData<TEntity></returns>
        public static PageData<TEntity> GetPageData<TEntity, TKey>(IQueryable<TEntity> querableList
                       , Expression<Func<TEntity, TKey>> orderByKeySelector
                       , PageParam pageParam, bool bIsAscOrder = true, string strOrder = "")
        {
            pageParam.Check();

            PageData<TEntity> pageData = new PageData<TEntity>(pageParam, querableList.Count());

            IQueryable<TEntity> orderQuerableList = GetOrderQueryable(querableList, orderByKeySelector, bIsAscOrder, strOrder);

            pageData.Data = GetPageData(orderQuerableList, pageParam).ToList();
            return pageData;
        }


        /// <summary>
        /// GetPageDataAsync
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="querableList"></param>
        /// <param name="orderByKeySelector"></param>
        /// <param name="pageParam"></param>
        /// <param name="bIsAscOrder"></param>
        /// <param name="strOrder">Id Asc, Date Desc</param>
        /// <returns>PageData<TEntity></returns>
        public static async Task<PageData<TEntity>> GetPageDataAsync<TEntity, TKey>(IQueryable<TEntity> querableList
                       , Expression<Func<TEntity, TKey>> orderByKeySelector
                       , PageParam pageParam, bool bIsAscOrder = true, string strOrder = "")
        {
            pageParam.Check();

            PageData<TEntity> pageData = new PageData<TEntity>(pageParam, await querableList.CountAsync());

            IQueryable<TEntity> orderQuerableList = GetOrderQueryable(querableList, orderByKeySelector, bIsAscOrder, strOrder);

            pageData.Data = await GetPageData(orderQuerableList, pageParam).ToListAsync();

            return pageData;
        }

        

        /// <summary>
        /// GetPageData
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="querableList"></param>
        /// <param name="orderByKeySelector"></param>
        /// <param name="bIsAscOrder"></param>
        /// <param name="strOrder">Id Asc, Date Desc</param>
        /// <returns>PageData<TEntity></returns>
        public static List<TEntity> GetData<TEntity, TKey>(IQueryable<TEntity> querableList
                       , Expression<Func<TEntity, TKey>> orderByKeySelector
                       , bool bIsAscOrder = true, string strOrder = "")
        {
            IQueryable<TEntity> orderQuerableList = GetOrderQueryable(querableList, orderByKeySelector, bIsAscOrder, strOrder);
            return orderQuerableList.ToList();
        }

        /// <summary>
        /// GetDataAsync
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="querableList"></param>
        /// <param name="orderByKeySelector"></param>
        /// <param name="bIsAscOrder"></param>
        /// <param name="strOrder">Id Asc, Date Desc</param>
        /// <returns>PageData<TEntity></returns>
        public static async Task<List<TEntity>> GetDataAsync<TEntity, TKey>(IQueryable<TEntity> querableList
                       , Expression<Func<TEntity, TKey>> orderByKeySelector
                       , bool bIsAscOrder = true, string strOrder = "")
        {
            IQueryable<TEntity> orderQuerableList = GetOrderQueryable(querableList, orderByKeySelector, bIsAscOrder, strOrder);
            return await orderQuerableList.ToListAsync();
        }

        #endregion Page
    }
}
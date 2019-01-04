using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JerryPlat.Utils.Helpers
{
    public class CacheHelper
    {
        private static ICache _cache = new Cache();

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public static T GetCache<T>(CacheKey cacheKey)
            where T : class
        {
            return GetCache<T>(cacheKey.ToString());
        }

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        /// <param name="expireTime">到期时间(默认10分钟)</param>
        public static void SetCache<T>(T value, CacheKey cacheKey, DateTime? expireTime = null)
            where T : class
        {
            SetCache<T>(value, cacheKey.ToString(), expireTime);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public static void RemoveCache(CacheKey cacheKey)
        {
            RemoveCache(cacheKey.ToString());
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public static T GetCache<T>(string cacheKey)
            where T : class
        {
            return _cache.GetCache<T>(cacheKey);
        }

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        /// <param name="expireTime">到期时间(默认10分钟)</param>
        public static void SetCache<T>(T value, string cacheKey, DateTime? expireTime = null)
            where T : class
        {
            _cache.SetCache<T>(value, cacheKey, expireTime);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public static void RemoveCache(string cacheKey)
        {
            _cache.RemoveCache(cacheKey);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public static void RemoveCache(Func<string, bool> func)
        {
            _cache.RemoveCache(func);
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void ClearCache()
        {
            _cache.ClearCache();
        }
    }
}

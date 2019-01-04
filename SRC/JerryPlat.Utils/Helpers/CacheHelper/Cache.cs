using System;
using System.Collections;
using System.Web;

namespace JerryPlat.Utils.Helpers
{
    public class Cache : ICache
    {
        private static System.Web.Caching.Cache cache = HttpRuntime.Cache;

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public T GetCache<T>(string cacheKey) where T : class
        {
            if (cache[cacheKey] != null)
            {
                return (T)cache[cacheKey];
            }
            return default(T);
        }
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        /// <param name="expireTime">到期时间</param>
        public void SetCache<T>(T value, string cacheKey, DateTime? expireTime = null) where T : class
        {
            expireTime = expireTime ?? DateTime.Now.AddDays(1);
            cache.Insert(cacheKey, value, null, expireTime.Value, System.Web.Caching.Cache.NoSlidingExpiration);
        }
        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public void RemoveCache(string cacheKey)
        {
            cache.Remove(cacheKey);
        }

        public void RemoveCache(Func<string, bool> func)
        {
            ForEach(key =>
            {
                if (func != null && func(key))
                {
                    cache.Remove(key);
                }
            });
        }

        private void ForEach(Action<string> action)
        {
            IDictionaryEnumerator CacheEnum = cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                action(CacheEnum.Key.ToString());
            }
        }
        
        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public void ClearCache()
        {
            ForEach(key =>
            {
                cache.Remove(key);
            });
        }
    }
}

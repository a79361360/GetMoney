using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Common.Cache
{
    public sealed class Cache
    {
        internal Cache() { }

        /// <summary>
        /// 保存
        /// </summary>
        /// <typeparam name="CacheType"></typeparam>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        public void Save<CacheType>(string key, object value) where CacheType : ICache, new()
        { 
            ICache cache = new CacheType();
            cache.Add(key, value);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <typeparam name="CacheType"></typeparam>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        /// <param name="timeout">过期时间</param>
        public void Save<CacheType>(string key, object value, int? timeout) where CacheType : ICache, new()
        {
            ICache cache = new CacheType();
            cache.Add(key, value, timeout);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <typeparam name="CacheType"></typeparam>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        /// <param name="timeout">过期时间</param>
        /// <param name="dt">当前时间</param>
        public void Save<CacheType>(string key, object value, int? timeout,DateTime dt) where CacheType : ICache, new()
        {
            ICache cache = new CacheType();
            cache.Add(key, value, timeout, dt);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="CacheType"></typeparam>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        public void Update<CacheType>(string key, object value) where CacheType : ICache, new()
        {
            ICache cache = new CacheType();
            cache.Update(key, value);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="CacheType"></typeparam>
        /// <param name="key">键名</param>
        public void Delete<CacheType>(string key) where CacheType : ICache, new()
        {
            ICache cache = new CacheType();
            cache.Delete(key);
        }

        /// <summary>
        /// 清空
        /// </summary>
        /// <typeparam name="CacheType"></typeparam>
        public void Clear<CacheType>() where CacheType : ICache, new()
        {
            ICache cache = new CacheType();
            cache.Delete();
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="CacheType"></typeparam>
        /// <param name="key">键名</param>
        /// <returns></returns>
        public object Get<CacheType>(string key) where CacheType : ICache, new()
        {
            ICache cache = new CacheType();
            return cache.GetValue(key);
        }
    }
}

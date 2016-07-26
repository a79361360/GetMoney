using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GetMoney.Common.Cache
{
    public sealed class AspNetCache:ICache
    {
        /// <summary>
        /// 添加AspNetCache
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        public void Add(string key, object value)
        {
            Add(key, value, 20);
        }

        /// <summary>
        /// 添加AspNetCache
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        /// <param name="timeout">过期时间，默认20分钟</param>
        public void Add(string key, object value, int? timeout)
        {
            HttpContext.Current.Cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, timeout == null || timeout.Value <= 0 ? new TimeSpan(0, 20, 0) : new TimeSpan(0, timeout.Value, 0));
        }
        /// <summary>
        /// 添加AspNetCache
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        /// <param name="timeout">绝对过期时间，默认20分钟</param>
        /// <param name="当前时间">当前时间</param>
        public void Add(string key, object value, int? timeout,DateTime dt)
        {
            dt = DateTime.Now.AddMinutes(Convert.ToDouble(timeout));
            HttpContext.Current.Cache.Insert(key, value, null, dt, System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 更新AspNetCache
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        public void Update(string key, object value)
        {
            HttpContext.Current.Cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 20, 0));
        }

        /// <summary>
        /// 删除指定AspNetCache
        /// </summary>
        /// <param name="key">键名</param>
        public void Delete(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }

        /// <summary>
        /// 删除所有AspNetCache
        /// </summary>
        public void Delete()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取AspNetCache
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            return HttpContext.Current.Cache.Get(key);
        }

        /// <summary>
        /// 获取AspNetCache
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns></returns>
        public object this[string key]
        {
            get { return this.GetValue(key); }
        }
    }
}

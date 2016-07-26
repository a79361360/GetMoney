using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GetMoney.Common.Cache
{
    public sealed class SessionCache : ICache
    {
        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        public void Add(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        /// <param name="timeout">过期时间</param>
        public void Add(string key, object value, int? timeout)
        {
            if (timeout != null && timeout > 0)
                HttpContext.Current.Session.Timeout = timeout.Value;
            HttpContext.Current.Session[key] = value;
        }
        public void Add(string key, object value, int? timeout, DateTime dt)
        {
            throw new EntryPointNotFoundException();
        }
        /// <summary>
        /// 更新Session
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        public void Update(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        /// <summary>
        /// 删除指定Session
        /// </summary>
        /// <param name="key">键名</param>
        public void Delete(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }

        /// <summary>
        /// 删除所有Session
        /// </summary>
        public void Delete()
        {
            HttpContext.Current.Session.RemoveAll();
        }

        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            return HttpContext.Current.Session[key];
        }

        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns></returns>
        public object this[string key]
        {
            get { return this.GetValue(key); }
        }
    }
}

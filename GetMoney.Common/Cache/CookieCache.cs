using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GetMoney.Common.Cache
{
    public sealed class CookieCache : ICache
    {
        private const string COOKIENAME = "FglyCookies", EXPIRESNAME = "TimeOut", AUTHNAME = "FglyCookieAuthCode", AUTHCODE = "AD2DB6C6-061E-4F0C-BAB4-725A2D3F39DC", DOMAIN = "", PATH = "/";

        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        public void Add(string key, object value)
        {
            Add(key, value, 20);
        }

        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        /// <param name="timeout">过期时间</param>
        public void Add(string key, object value, int? timeout)
        {
            HttpCookie ck = HttpContext.Current.Request.Cookies[COOKIENAME];
            if (ck == null)
                ck = new HttpCookie(COOKIENAME);
            ck.Expires = DateTime.Now.AddYears(100);
            ck.Domain = DOMAIN;
            ck.Path = PATH;
            ck.Values[key] = HttpUtility.UrlEncode(value.ToString());
            ck.Values[key + EXPIRESNAME] = HttpUtility.UrlEncode(DateTime.Now.AddMinutes(timeout == null || timeout.Value <= 0 ? 20 : timeout.Value).ToString("yyyy-MM-dd HH:mm:ss"));
            ck.Values[AUTHNAME] = GetAuthCode(ck);
            HttpContext.Current.Response.Cookies.Add(ck);
        }
        public void Add(string key, object value, int? timeout, DateTime dt)
        {
            throw new EntryPointNotFoundException();
        }
        /// <summary>
        /// 更新Cookie
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        public void Update(string key, object value)
        {
            HttpCookie ck = HttpContext.Current.Request.Cookies[COOKIENAME];
            if (ck == null || string.IsNullOrEmpty(key) || ck.Values[key] == null)
                return;
            ck.Expires = DateTime.Now.AddYears(100);
            ck.Domain = DOMAIN;
            ck.Path = PATH;
            ck.Values[key] = HttpUtility.UrlEncode(value.ToString());
            ck.Values[AUTHNAME] = GetAuthCode(ck);
            HttpContext.Current.Response.Cookies.Add(ck);
        }

        /// <summary>
        /// 删除指定Cookie
        /// </summary>
        /// <param name="key">键名</param>
        public void Delete(string key)
        {
            HttpCookie ck = HttpContext.Current.Request.Cookies[COOKIENAME];
            if (ck == null || string.IsNullOrEmpty(key) || ck.Values[key] == null)
                return;
            ck.Expires = DateTime.Now.AddYears(100);
            ck.Domain = DOMAIN;
            ck.Path = PATH;
            ck.Values[key] = "";
            ck.Values[key + EXPIRESNAME] = "";
            ck.Values[AUTHNAME] = GetAuthCode(ck);
            HttpContext.Current.Response.Cookies.Add(ck);
        }

        /// <summary>
        /// 删除所有Cookie
        /// </summary>
        public void Delete()
        {
            HttpCookie ck = HttpContext.Current.Request.Cookies[COOKIENAME];
            if (ck == null)
                return;
            ck.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(ck);
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            HttpCookie ck = HttpContext.Current.Request.Cookies[COOKIENAME];
            if (ck == null || string.IsNullOrEmpty(key) || ck.Values[key] == null || !IsAuthCode(ck))
                return null;
            ck.Expires = DateTime.Now.AddYears(100);
            ck.Domain = DOMAIN;
            ck.Path = PATH;
            try
            {
                if (Convert.ToDateTime(HttpUtility.UrlDecode(ck.Values[key + EXPIRESNAME])) < DateTime.Now)
                {
                    ck.Values[key] = "";
                    ck.Values[key + EXPIRESNAME] = "";
                    ck.Values[AUTHNAME] = GetAuthCode(ck);
                    HttpContext.Current.Response.Cookies.Add(ck);
                    return null;
                }
            }
            catch
            {
                return null;
            }
            return HttpUtility.UrlDecode(ck.Values[key]);
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns></returns>
        public object this[string key]
        {
            get { return this.GetValue(key); }
        }

        /// <summary>
        /// 获取认证码
        /// </summary>
        /// <param name="ck"></param>
        /// <returns></returns>
        private string GetAuthCode(HttpCookie ck)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in ck.Values.AllKeys)
            {
                if (!key.Equals(AUTHNAME))
                    sb.Append(ck.Values[key]);
            }
            sb.Append(AUTHCODE);
            sb.Append(HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"]);
            sb.Append(HttpContext.Current.Request.ServerVariables["Instance_ID"]);
            sb.Append(HttpContext.Current.Request.ServerVariables["Http_User_Agent"]);
            return sb.ToString().MD5();
        }

        /// <summary>
        /// 判断认证码是否合法
        /// </summary>
        /// <param name="ck"></param>
        /// <returns></returns>
        private bool IsAuthCode(HttpCookie ck)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in ck.Values.AllKeys)
            {
                if (!key.Equals(AUTHNAME))
                    sb.Append(ck.Values[key]);
            }
            sb.Append(AUTHCODE);
            sb.Append(HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"]);
            sb.Append(HttpContext.Current.Request.ServerVariables["Instance_ID"]);
            sb.Append(HttpContext.Current.Request.ServerVariables["Http_User_Agent"]);
            string authCode = sb.ToString().MD5();
            if (!ck.Values[AUTHNAME].Equals(authCode))
                return false;
            return true;
        }
    }
}

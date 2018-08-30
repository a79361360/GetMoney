using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;

namespace GetMoney.Common
{
    /// <summary>
    /// Web 操作类
    /// </summary>
    public sealed class Web
    {
        internal Web() { }

        /// <summary>
        /// 获取url字符串参数
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">默认键值</param>
        /// <returns></returns>
        public string Request(string key, string defaultValue)
        {
            return Request(key,defaultValue,false);
        }
        /// <summary>
        /// 获取url字符串参数
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">默认键值</param>
        /// <param name="isSaveSymbol">是否保留符号</param>
        /// <returns></returns>
        public string Request(string key, string defaultValue, bool isSaveSymbol)
        {
            string value = System.Web.HttpContext.Current.Request[key];
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            if (!isSaveSymbol)
             return value.Replace("'", "''").Replace(";", "");
            return value;
        }
        /// <summary>
        /// 获取url字符串参数（解码）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string RequestUrlDecode(string key, string defaultValue)
        {
            string value = System.Web.HttpContext.Current.Request[key];
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            return HttpUtility.UrlDecode(value).Replace("'", "''").Replace(";", "").Replace("--", "");
        }

        /// <summary>
        /// 获取表单内数据
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">默认键值</param>
        /// <returns></returns>
        public string RequestForm(string key, string defaultValue)
        {
            return RequestForm(key, defaultValue,false);
        }

        /// <summary>
        /// 获取表单内数据
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">默认键值</param>
        /// <param name="isSaveSymbol">是否保留符号</param>
        /// <returns></returns>
        public string RequestForm(string key, string defaultValue, bool isSaveSymbol)
        {
            string value = System.Web.HttpContext.Current.Request.Form[key];
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            if (!isSaveSymbol)
                return value.Replace("'", "''").Replace(";", "").Replace("--", "");
            return value;
        }
        /// <summary>
        /// 获取web客户端ip
        /// </summary>
        /// <returns></returns>
        public string GetWebClientIp()
        {

            string userIP = "127.0.0.2";    //如果没取到IP,使用127.0.0.2

            try
            {
                if (System.Web.HttpContext.Current == null
            || System.Web.HttpContext.Current.Request == null
            || System.Web.HttpContext.Current.Request.ServerVariables == null)
                    return "";

                string CustomerIP = "";

                //CDN加速后取到的IP simone 090805
                CustomerIP = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(CustomerIP) && IsIP(CustomerIP))
                {
                    return CustomerIP;
                }

                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];


                if (!String.IsNullOrEmpty(CustomerIP) && IsIP(CustomerIP))
                    return CustomerIP;

                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (CustomerIP == null)
                        CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.Compare(CustomerIP, "unknown", true) == 0 && IsIP(CustomerIP))
                    return System.Web.HttpContext.Current.Request.UserHostAddress;
                return CustomerIP;
            }
            catch { }

            return userIP;

        }
        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        /// <summary>
        /// 判断请求是否来自本站
        /// </summary>
        /// <returns></returns>
        public bool RequestIsHost()
        {
            string server_referrer = string.Empty, server_host = string.Empty;

            if (System.Web.HttpContext.Current.Request.UrlReferrer == null)
                return false;
            else
                server_referrer = System.Web.HttpContext.Current.Request.UrlReferrer.Host.ToLower();

            server_host = System.Web.HttpContext.Current.Request.Url.Host.ToLower();
            if (server_referrer.Equals(server_host))
                return true;
            return false;
        }

        /// <summary>
        /// 验证请求
        /// </summary>
        public void VerificationRequest()
        {
            if (!CommonManager.WebObj.RequestIsHost())
                System.Web.HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 验证表单
        /// </summary>
        public void VerificationFrom()
        {
            if (!CommonManager.WebObj.RequestIsHost() || System.Web.HttpContext.Current.Request.Form.Count == 0)
                System.Web.HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public string Get(string url)
        {
            //System.Net.ServicePointManager.DefaultConnectionLimit = 512;
            //int i = System.Net.ServicePointManager.DefaultPersistentConnectionLimit;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 20000;
            request.AllowAutoRedirect = false;
            request.ServicePoint.Expect100Continue = false;

            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                    return response.StatusDescription;
            }
            catch(Exception e)
            {
                return e.Message;
            }
            finally
            {
                request = null;
            }
        }

        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">发送的数据</param>
        /// <returns></returns>
        public string Post(string url, string data)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 20000;
            request.AllowAutoRedirect = false;
            request.ServicePoint.Expect100Continue = false;

            try
            {
                using (System.IO.StreamWriter write = new System.IO.StreamWriter(request.GetRequestStream()))
                {
                    write.Write(data);
                }

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                    return response.StatusDescription;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                request = null;
            }
        }

        /// <summary>
        /// 注册Js脚本到页面
        /// </summary>
        /// <param name="page"></param>
        /// <param name="js"></param>
        /// <param name="bottom"></param>
        public void RegJs(System.Web.UI.Page page, string js, bool bottom)
        {
            if (bottom)
                page.ClientScript.RegisterStartupScript(page.GetType(), new Random().Next(1000, 9999).ToString(), "<script type=\"text/javascript\" language=\"javascript\">" + js + "</script>");
            else
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), new Random().Next(1000, 9999).ToString(), "<script type=\"text/javascript\" language=\"javascript\">" + js + "</script>");
        }

        //添加cookie
        public void AddCookie(string name, string value, int? timeout) {
            HttpCookie cookie = new HttpCookie(name);
            if (cookie == null)
            {
                cookie.Expires = DateTime.Now.AddMinutes(timeout == null || timeout.Value <= 0 ? 20 : timeout.Value);
                cookie.Value = value;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else {
                UpdateCookie(name, value, timeout);
            }
        }
        //更新cookie失效时间
        public void UpdateCookie(string name, string value, int? timeout)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie.Expires = DateTime.Now.AddMinutes(timeout == null || timeout.Value <= 0 ? 20 : timeout.Value);
            cookie.Value = value;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        //移除cookie
        public void RemoveCookie(string name) {
            HttpContext.Current.Response.Cookies.Remove(name);
        }
        public string GetCurHttpsHost()
        {
            string httpstr = HttpContext.Current.Request.Url.ToString().Split(':')[0];
            return httpstr + "://" + HttpContext.Current.Request.Url.Host;
            //return "https://" + HttpContext.Current.Request.Url.Host;
        }
    }
}

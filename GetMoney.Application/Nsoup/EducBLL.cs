using CsharpHttpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Application.Nsoup
{
    public class EducBLL
    {
        public int EducLogin() {
            string account = "350481198810211018";
            string password = "abdcf0b97edad72851418564b361db9c";
            string url = "http://iycdz.ezhongzhi.com/";

            HttpHelper helpweb = new HttpHelper();  //初始实例化HttpHelper
            HttpResult result = new HttpResult();   //初始实例化HttpResult
            HttpItem item = new HttpItem()
            {
                URL = url,                                          //URL     必需项    
                Method = "GET",                                    //URL     可选项 默认为Get   
                ProxyIp = "ieproxy",
                ContentType = "application/x-www-form-urlencoded",  //ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
            };
            try
            {
                result = helpweb.GetHtml(item);
                string cookie = fhcookie(result.Cookie);
                url = "http://iycdz.ezhongzhi.com/index.php?_c=base.login.user_login";
                item = new HttpItem()
                {
                    URL = url,                                          //URL     必需项    
                    Method = "POST",                                    //URL     可选项 默认为Get   
                    ProxyIp = "ieproxy",
                    ContentType = "application/x-www-form-urlencoded",  //ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
                    Postdata = "juser[account]=" + account + "&juser[password]=" + password,
                    Cookie = cookie
                };
                result = helpweb.GetHtml(item);
                NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(result.Html);
                cookie = fhcookie1(result.Cookie);
                //url = "http://iycdz.ezhongzhi.com/index.php?_c=main.page.stu";
                //item = new HttpItem()
                //{
                //    URL = url,                                          //URL     必需项    
                //    Method = "GET",                                    //URL     可选项 默认为Get   
                //    ProxyIp = "ieproxy",
                //    ContentType = "application/x-www-form-urlencoded",  //ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
                //    Cookie = cookie
                //};
                //result = helpweb.GetHtml(item);
                //doc = NSoup.NSoupClient.Parse(result.Html);

                url = "http://iycdz.ezhongzhi.com/study/page/index/type/1/course_id/573024870703f245#zjxx";
                item = new HttpItem()
                {
                    URL = url,                                          //URL     必需项    
                    Method = "GET",                                    //URL     可选项 默认为Get   
                    ProxyIp = "ieproxy",
                    ContentType = "application/x-www-form-urlencoded",  //ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
                    Cookie = cookie
                };
                result = helpweb.GetHtml(item);
                doc = NSoup.NSoupClient.Parse(result.Html);
                NSoup.Select.Elements Elements = doc.Select(".empty span");
                foreach (var item_1 in Elements) {
                    string test = item_1.Html();
                }

                return -2;
            }
            catch (Exception er)
            {
                //CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "EducLogin 校验登入Cookie异常： " + er.Message);
                return -1;
            }
        }
        private string fhcookie(string str)
        {
            string[] str1 = str.Split(','); string tempstr = "";
            foreach (var item in str1)
            {
                string[] str2 = item.Split(';');
                if (tempstr == "") tempstr = str2[0];
                else tempstr += ";" + str2[0];
            }
            return tempstr;
        }
        /// <summary>
        /// 将登入以后的nnss的cookie值返回
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string fhcookie1(string str)
        {
            string[] str1 = str.Split(','); string tempstr = "";
            foreach (var item in str1)
            {
                string[] str2 = item.Split(';');
                string[] nnss = str2[0].Split('=');
                if (nnss[0] == "nnss") {
                    tempstr = "nnss=" + nnss[1];
                }
            }
            return tempstr;
        }
    }
}

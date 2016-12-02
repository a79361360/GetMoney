using GetMoney.Common;
using NSoup.Select;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GetMoney.Application.Nsoup
{
    public class NsoupHandle
    {
        public void Test(string url)
        {
            Elements links = null;
            MyWebClient webClient = new MyWebClient();
            String HtmlString = GetHtmlString(webClient, url);
            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(HtmlString);
            links = doc.Select("#read_tpc img");
            foreach (var item in links) {
                string imguri = item.Attr("src");
                if (!string.IsNullOrEmpty(imguri))
                    CommonManager.FileObj.DowdLoad_ImgByUrl(imguri, "/DownLoad/Img", "");
            }
        }
        public void TestT(string url) {
            Elements links = null;
            MyWebClient webClient = new MyWebClient();
            String HtmlString = GetHtmlString(webClient, url);
            //三次都在超时就暂时不抓了
            if (string.IsNullOrEmpty(HtmlString)) {
                return;
            }
            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(HtmlString);
            links = doc.Select(".t_one h3 a");
            foreach (var item in links)
            {
                string uri = item.Attr("href");
                string text = FilterFloder(item.Text());
                //if (!string.IsNullOrEmpty(uri))
                //    Test("http://c2.1024mx.org/pw/" + uri);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CatchImgPutPath(string url,string path) {
            CommonManager.FolderObj.CreateFolder(path);
            Elements links = null;
            MyWebClient webClient = new MyWebClient();
            String HtmlString = GetHtmlString(webClient, url);
            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(HtmlString);
            links = doc.Select("#read_tpc img");
            foreach (var item in links) {
                string imguri = item.Attr("src");
                if (!string.IsNullOrEmpty(imguri))
                    CommonManager.FileObj.DowdLoad_ImgByUrl(imguri, "/DownLoad/Img", "");
            }
        }
        /// <summary>
        /// 一个网址连续抓取三次,如果三次都抓取超时或者失败,就不抓了
        /// </summary>
        /// <param name="webClient"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetHtmlString(MyWebClient webClient,string url)
        {
            string result = "";
            int ExecuteNum = 0;     //执行次数
            try
            {
                result = Encoding.GetEncoding("utf-8").GetString(webClient.DownloadData(url));
            }
            catch (Exception er)
            {
                ExecuteNum++;
                if (ExecuteNum > 3) return "";
                GetHtmlString(webClient, url);
            }
            return result;
        }
        /// <summary>
        /// 将非法字符过滤成空
        /// </summary>
        /// <param name="floder"></param>
        private string FilterFloder(string floder) {
            //string illegal = "\"M\"\\a/ry/ h**ad:>> a\\/:*?\"| li*tt|le|| la\"mb.?";
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            foreach (char c in invalid)
            {
                //illegal = illegal.Replace(c.ToString(), "");
                floder = floder.Replace(c.ToString(), "");
            }
            return floder;
        }

    }
}

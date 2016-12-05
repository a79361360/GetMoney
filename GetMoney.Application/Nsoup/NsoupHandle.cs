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
        int ExecuteNum = 0;     //执行次数
        //public void Test(string url)
        //{
        //    Elements links = null;
        //    MyWebClient webClient = new MyWebClient();
        //    String HtmlString = GetHtmlString(webClient, url);
        //    NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(HtmlString);
        //    links = doc.Select("#read_tpc img");
        //    foreach (var item in links) {
        //        string imguri = item.Attr("src");
        //        if (!string.IsNullOrEmpty(imguri))
        //            CommonManager.FileObj.DowdLoad_ImgByUrl(imguri, "/DownLoad/Img", "");
        //    }
        //}
        /// <summary>
        /// 根据抓取地址,取得里面的下级地址,再保存到指定下载地址
        /// </summary>
        /// <param name="url">抓取地址</param>
        /// <param name="downpath">保存地址</param>
        /// <param name="hosturi">网址使用虚拟地址的时候取得HOST进行拼接,生成完整http访问地址</param>
        public void CatchUriByPUri(string url,string downpath,string hosturi) {
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
                string uri = hosturi + item.Attr("href");               //抓取图片的URI
                string text = FilterFloder(item.Text());                //将Title做为名称创建成文件夹,是否合法
                string path = downpath + text;                          //存放图片的文件夹
                if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(path)) {
                    CatchImgPutPath(uri, path);
                }
            }
        }
        /// <summary>
        /// 创建文件夹,并保存图片到此文件夹
        /// </summary>
        /// <param name="url">抓取图片的URI</param>
        /// <param name="path">保存图片的PATH</param>
        public void CatchImgPutPath(string url,string path) {
            CreateFloder(path);     //创建文件夹
            Elements links = null;
            MyWebClient webClient = new MyWebClient();
            ExecuteNum = 0;     //将重连次数清零
            String HtmlString = GetHtmlString(webClient, url);
            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(HtmlString);
            links = doc.Select("#read_tpc img");
            foreach (var item in links) {
                string imguri = item.Attr("src");
                if (!string.IsNullOrEmpty(imguri))
                    CommonManager.FileObj.DowdLoad_ImgByUrl(imguri, path, "");
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
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path"></param>
        private void CreateFloder(string path) {
            path = CommonManager.FileObj.GetPhysicalPath(path);
            CommonManager.FolderObj.CreateFolder(path);
        }

    }
}

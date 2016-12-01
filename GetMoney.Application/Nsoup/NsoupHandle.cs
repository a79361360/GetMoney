using GetMoney.Common;
using NSoup.Select;
using System;
using System.Collections.Generic;
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
            String HtmlString = Encoding.GetEncoding("utf-8").GetString(webClient.DownloadData(url));
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
            String HtmlString = Encoding.GetEncoding("utf-8").GetString(webClient.DownloadData(url));
            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(HtmlString);
            links = doc.Select(".t_one h3 a");
            foreach (var item in links)
            {
                string uri = item.Attr("href");
                string text = item.Text();
                if (!string.IsNullOrEmpty(uri))
                    Test("http://c2.1024mx.org/pw/" + uri);
            }
        }
    }
}

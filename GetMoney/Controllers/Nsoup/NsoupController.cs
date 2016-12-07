using GetMoney.Application.Nsoup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetMoney.Controllers.Nsoup
{
    public class NsoupController : Controller
    {
        //
        // GET: /Nsoup/

        public ActionResult Index()
        {
            GetImgByUrl();
            return View();
        }
        public void GetImgByUrl() {
            string url = "http://c2.1024mx.org/pw/thread.php?fid=49&page=1";
            System.Uri httpUrl = new System.Uri(url);
            string hosturl = httpUrl.AbsoluteUri.Substring(0, httpUrl.AbsoluteUri.LastIndexOf('/') + 1);
            NsoupHandle handle = new NsoupHandle();
            //handle.CatchUriByPUri(url, "/DownLoad/Img/", hosturl);
            handle.CatchUriByPUri(url, "/DownLoad/OriginalImg/", hosturl);
        }
    }
}

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
            string url = "http://c2.1024mx.org/pw/thread.php?fid=14&page=2";
            NsoupHandle handle = new NsoupHandle();
            //handle.TestT(url);
            handle.ttttr();
        }
    }
}

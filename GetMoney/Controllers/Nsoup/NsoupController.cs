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
            string url = "https://www.zhihu.com/explore";
            NsoupHandle handle = new NsoupHandle();
            handle.Test(url);
            
        }
    }
}

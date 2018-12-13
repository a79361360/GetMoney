using GetMoney.Application.WeiX;
using GetMoney.Common.Expand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetMoney.Controllers
{
    public class GlassesController : Controller
    {
        //
        // GET: /Glasses/
        public string generurl = "";
        public string appid = "";
        public string noncestr = randomstr();
        public long timestamp = DateTime.Now.ToUnixTimeStamp();
        public string title = "";
        public string desc = "";
        public string imgurl = "";
        public string linkurl = "";
        public string signatrue = "";
        WeiXBLL wxbll = new WeiXBLL();
        public ActionResult Index()
        {
            appid = wxbll.appid;
            ViewBag.appid = appid;
            ViewBag.timestamp = timestamp;
            ViewBag.noncestr = noncestr;
            ViewBag.signatrue = wxbll.Get_signature(timestamp, noncestr);
            ViewBag.title = "新店特惠 LOOK眼镜龙首路店开业";
            ViewBag.desc = "新店特惠 LOOK眼镜龙首路店开业";

            return View();
        }
        public static string randomstr()
        {
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            int num = rand.Next(0, 10000);
            string result = (num.ToString() + "4a64cd60aa6666f29a1dce503226eaa1").MD5();
            return result;
        }
    }
}

using GetMoney.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetMoney.Controllers.Pay
{
    public class WxPayController : BaseController
    {
        //
        // GET: /WxPay/

        public ActionResult Index()
        {
            return View();
        }

    }
}

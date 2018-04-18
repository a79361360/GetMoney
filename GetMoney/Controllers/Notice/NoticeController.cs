using GetMoney.Application.Notice;
using GetMoney.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetMoney.Controllers.Notice
{
    public class NoticeController : BaseController
    {
        //
        // GET: /Notice/
        NoticeBll bll = new NoticeBll();
        public ActionResult Index()
        {
            return View();
        }
    }
}

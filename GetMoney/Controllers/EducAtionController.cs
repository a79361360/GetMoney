using GetMoney.Application.Nsoup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetMoney.Controllers
{
    public class EducAtionController : Controller
    {
        //
        // GET: /EducAtion/

        public ActionResult Index()
        {

            EducBLL bll = new EducBLL();
            bll.EducLogin();
            return View();
        }

    }
}

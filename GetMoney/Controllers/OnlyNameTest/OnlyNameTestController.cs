using GetMoney.Application.OnlyNameTest;
using GetMoney.Framework;
using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GetMoney.Controllers.OnlyNameTest
{
    public class OnlyNameTestController : BaseController
    {
        //
        // GET: /OnlyNameTest/
        IOnlyNameTestBll _bll = null;
        public OnlyNameTestController(IOnlyNameTestBll bll)
        {
            _bll = bll;
        }
        public void TestDbcontext()
        {
            OnlyNameTestDto dto = new OnlyNameTestDto();
            //新增
            dto.Name = "qiuqiu";
            dto.InputDate = DateTime.Now;
            _bll.Add(dto);
            //Db.OnlyNameTest.Add(dto);
            //Db.SaveChanges();

        }
        public ActionResult Test() {
            return View();
        }
    }
}

using GetMoney.Application.TUser.Service;
using GetMoney.Framework;
using GetMoney.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetMoney.Controllers.TUser
{
    public class TuOptionController : BaseController
    {
        //
        // GET: /TuOption意见/
        TUOptionBLL bll = new TUOptionBLL();
        public ActionResult Index()
        {
            var dto = new TUOptionDTO();
            if (Request["id"] != null)
                dto = bll.FindTOptionById(Convert.ToInt32(Request["id"]));
            return View(dto);
        }
        public ActionResult AddUOption() {
            if (Session["uid"] == null)
                return JsonFormat(new ExtJsonPage { success = false, code = -1001, msg = "登入状态已失效！" });
            string title = Request["title"].ToString();
            string type = Request["type"].ToString();
            string content = Request["content"].ToString();
            string userid = Session["uid"].ToString();
            int result = bll.AddTUOption(title, Convert.ToInt32(type), content);
            if (result > 0)
                return JsonFormat(new ExtJson { success = true, msg = "提交成功！" });
            else
                return JsonFormat(new ExtJson { success = false, msg = "提交失败！" });
        }
    }
}

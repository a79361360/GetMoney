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
            if (Request.Form["title"] == null || Request.Form["type"] == null || Request.Form["content"] == null)
                return JsonFormat(new ExtJsonPage { success = false, code = -1001, msg = "参数不能为空！" });
            string title = Request.Form["title"].ToString();
            string type = Request.Form["type"].ToString();
            string content = Request.Form["content"].ToString();
            string userid = Session["uid"].ToString();
            int result = bll.AddTUOption(Convert.ToInt32(userid), title, Convert.ToInt32(type), content);
            if (result > 0)
                return JsonFormat(new ExtJson { success = true, msg = "提交成功！" });
            else
                return JsonFormat(new ExtJson { success = false, msg = "提交失败！" });
        }
        public ActionResult ListOption() {
            if (Session["uid"] == null)
                return JsonFormat(new ExtJsonPage { success = false, code = -1001, msg = "登入状态已失效！" });
            int pageIndex = Convert.ToInt32(Request["pageIndex"]) - 1;      //当前页,这个存储过程首页0为开始
            int pageSize = Convert.ToInt32(Request["pageSize"]);            //每页条数
            int uid = Convert.ToInt32(Session["uid"]);                      //登入的用户ID

            string filter = "userid=" + uid.ToString();
            int Total = 0;
            IList<TUOptionDTO> list = bll.ListTUOptionPage(ref Total, pageSize, pageIndex, filter);
            if (list.Count > 0)
                return JsonFormat(new ExtJsonPage { success = true, code = 1000, msg = "查询成功！", total = Total, list = list });
            else
                return JsonFormat(new ExtJsonPage { success = false, code = -1000, msg = "查询失败！" });
        }
    }
}

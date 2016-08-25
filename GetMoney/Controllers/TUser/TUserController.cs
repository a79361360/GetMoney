using GetMoney.Application;
using GetMoney.Framework;
using GetMoney.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetMoney.Controllers.TUser
{
    public class TUserController : BaseController
    {
        ITUserBll _bll = null;
        public TUserController(ITUserBll bll)
        {
            _bll = bll;
        }
        //
        // GET: /TUser/

        public ActionResult Index()
        {
            AddTUser();
            return View();
        }
        public ActionResult AddTUser()
        {
            TUserDto dto = new TUserDto();
            dto.UserName = "3333";
            dto.NickName = "123";
            dto.UserJb = 100;
            dto.IdentityNum = "111111111111111111";
            dto.Phone = "18020637512";
            dto.TxUrl = "";
            dto.State = 1;
            dto.Addtime = DateTime.Now;
            _bll.AddTUser(dto);
            return JsonFormat(new ExtJson { success = true, msg = "添加成功！" });
        }
        public ActionResult Remove()
        {
            string id = "1";
            bool result = _bll.RemoveTUser(id);
            if (result)
                return JsonFormat(new ExtJson { success = true, msg = "删除成功！" });
            else
                return JsonFormat(new ExtJson { success = false, msg = "删除失败！" });
        }
        public ActionResult ListTUserPage()
        {
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            int Total = 0;
            IList<TUserDto> list = _bll.ListTUserPage(ref Total, pageSize, pageIndex);
            if (list.Count > 0)
                return JsonFormat(new ExtJsonPage { success = true, code = 1000, msg = "查询成功！", total = Total, list = list });
            else
                return JsonFormat(new ExtJsonPage { success = false, code = -1000, msg = "查询失败！" });
        }
    }
}

﻿using GetMoney.Application;
using GetMoney.Common;
using GetMoney.Common.Expand;
using GetMoney.Common.SerializeObject;
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
            RegTUser();
            return View();
        }
        public ActionResult AddTUser()
        {
            TUserDto dto = new TUserDto();
            dto.UserName = "3333";
            dto.UserPwd = "123123";
            dto.BankPwd = "123123";
            dto.NickName = "123";
            dto.UserJb = 100;
            dto.TrueName = "中文";
            dto.IdentityNum = "111111111111111111";
            dto.Phone = "18020637512";
            dto.RegIP = CommonManager.WebObj.GetWebClientIp();
            dto.TxUrl = "";
            dto.State = 1;
            dto.Addtime = DateTime.Now;
            _bll.AddTUser(dto);
            return JsonFormat(new ExtJson { success = true, msg = "添加成功！" });
        }
        public ActionResult RegTUser() {
            TUserDto dto = new TUserDto();
            //dto.UserName = CommonManager.WebObj.RequestForm("UserName", "");
            //dto.UserPwd = CommonManager.WebObj.RequestForm("UserPwd", "").MD5();
            dto.UserName = CommonManager.WebObj.Request("UserName", "");
            dto.UserPwd = CommonManager.WebObj.Request("UserPwd", "").MD5();
            dto.RegIP = CommonManager.WebObj.GetWebClientIp();
            Dictionary<string, object> list = new Dictionary<string, object>();
            int result = -1;    //注册结果
            string msg = "注册成功!";
            if (!string.IsNullOrEmpty(dto.UserName) || !string.IsNullOrEmpty(dto.UserPwd)) { 
                _bll.RegTUser(dto, out list);
                if (list.Count > 0) {
                    result = Convert.ToInt32(list["@ReturnValue"]);
                    switch (result)
                    {
                        case 1:
                            Session["Userid"] = list["@Userid"].ToString();
                            break;
                        default:
                            msg = "注册失败!";
                            break;

                    }
                }
            }
            if (result == 1)
                return JsonFormat(new ExtJson { success = true, msg = msg });
            else
                return JsonFormat(new ExtJson { success = false, msg = msg });
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
        public ActionResult TUserFriend()
        {
            string stt = CommonManager.WebObj.RequestForm("data", "");
            List<ttt> list = SerializeJson<ttt>.JSONStringToList(stt);
            return JsonFormat(new ExtJson { success = true, msg = "添加成功！", jsonresult = list });
        }
    }
    public class ttt {
        public string name { get; set; }
        public string index { get; set; }
    }
}

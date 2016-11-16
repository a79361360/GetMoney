using GetMoney.Application;
using GetMoney.Common;
using GetMoney.Common.Expand;
using GetMoney.Common.SerializeObject;
using GetMoney.Framework;
using GetMoney.Model;
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
            return View();
        }
        /// <summary>
        /// 快速注册
        /// </summary>
        /// <returns></returns>
        public ActionResult QuickRegTUser() { 
            TUserDto dto = new TUserDto();
            dto.UserName = CommonManager.WebObj.RequestForm("UserName", "");
            dto.UserPwd = CommonManager.WebObj.RequestForm("UserPwd", "").MD5();
            dto.BankPwd = dto.UserPwd;
            dto.NickName = dto.UserName;
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
        /// <summary>
        /// 手机注册
        /// </summary>
        /// <returns></returns>
        public ActionResult PhoneRegTUser() {
            TUserDto dto = new TUserDto();
            dto.UserName = CommonManager.WebObj.RequestForm("UserName", "");
            dto.UserPwd = CommonManager.WebObj.RequestForm("UserPwd", "").MD5();
            dto.BankPwd = dto.UserPwd;
            dto.Phone = CommonManager.WebObj.RequestForm("UserName", "");
            dto.RegIP = CommonManager.WebObj.GetWebClientIp();
            Dictionary<string, object> list = new Dictionary<string, object>();
            int result = -1;    //注册结果
            string msg = "注册成功!";
            if (!string.IsNullOrEmpty(dto.UserName) || !string.IsNullOrEmpty(dto.UserPwd))
            {
                _bll.RegTUser(dto, out list);
                if (list.Count > 0)
                {
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
        /// <summary>
        /// 完整注册
        /// </summary>
        /// <returns></returns>
        public ActionResult FullRegTUser() {
            TUserDto dto = new TUserDto();
            dto.UserName = CommonManager.WebObj.RequestForm("UserName", "");
            dto.UserPwd = CommonManager.WebObj.RequestForm("UserPwd", "").MD5();
            dto.BankPwd = CommonManager.WebObj.RequestForm("BankPwd", "").MD5();
            dto.NickName = CommonManager.WebObj.RequestForm("NickName", "");
            dto.TrueName = CommonManager.WebObj.RequestForm("TrueName", "");
            dto.IdentityNum = CommonManager.WebObj.RequestForm("IdentityNum", "");
            dto.Phone = CommonManager.WebObj.RequestForm("Phone", "");
            dto.RegIP = CommonManager.WebObj.GetWebClientIp();
            dto.TxUrl= CommonManager.WebObj.RequestForm("TxUrl", "");
            Dictionary<string, object> list = new Dictionary<string, object>();
            int result = -1;    //注册结果
            string msg = "注册成功!";
            if (!string.IsNullOrEmpty(dto.UserName) || !string.IsNullOrEmpty(dto.UserPwd))
            {
                _bll.RegTUser(dto, out list);
                if (list.Count > 0)
                {
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
            string type = CommonManager.WebObj.Request("type", "");
            string text = CommonManager.WebObj.Request("text", "");
            string filter = "";
            if (!string.IsNullOrEmpty(text))
            {
                filter = type + " like '%" + text + "%'";
            }
            int Total = 0;
            IList<TUserDto> list = _bll.ListTUserPage(ref Total, pageSize, pageIndex, filter);
            if (list.Count > 0)
                return JsonFormat(new ExtJsonPage { success = true, code = 1000, msg = "查询成功！", total = Total, list = list });
            else
                return JsonFormat(new ExtJsonPage { success = false, code = -1000, msg = "查询失败！" });
        }
        public ActionResult TUserSearch() {
            return View();
        }
        /// <summary>
        /// 批量添加好友
        /// </summary>
        /// <returns></returns>
        public ActionResult TUserFriend()
        {
            return View();
        }
        public ActionResult CreateFriend() {
            string data = CommonManager.WebObj.RequestForm("data", ""); //参数字符串
            string userid = "0";
            if (Session["uid"] == null)
            {
                return JsonFormat(new ExtJson { success = false, msg = "登入状态已失效！" });
            }
            userid = Session["uid"].ToString();
            IList<UListDto> list = SerializeJson<UListDto>.JSONStringToList(data);
            int Rnum = _bll.AddTUserFriend(Convert.ToInt32(userid), list);
            if (Rnum > 0)
            {
                return JsonFormat(new ExtJson { success = true, msg = "添加成功！", jsonresult = Rnum });
            }
            else
            {
                return JsonFormat(new ExtJson { success = false, msg = "添加失败！", jsonresult = "" });
            }
        }
        /// <summary>
        /// 查询我的好友
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchFriend() {
            if (Session["uid"] == null)
            {
                return JsonFormat(new ExtJsonPage { success = false, code = -1000, msg = "登入状态已失效！" });
            }
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            string type = CommonManager.WebObj.Request("type", "");     //手机,实名,昵称
            string text = CommonManager.WebObj.Request("text", "");     //查询条件的内容
            int Userid = Convert.ToInt32(Session["uid"]);               //当前登入的用户
            string State = CommonManager.WebObj.Request("state", "");   //是好友查询还是黑名单查询
            string filter = "TUserFriends.Userid=" + Userid;
            if (!string.IsNullOrEmpty(State)) {
                filter += " and TUserFriends.State=" + State.FilterSql();
            }
            if (!string.IsNullOrEmpty(text))
            {
                filter += " and TUsers." + type + " like '%" + text.FilterSql() + "%'";
            }
            int Total = 0;
            IList<FriendDto> list = _bll.ListFriendPage(ref Total, pageSize, pageIndex, filter);
            if (list.Count > 0)
                return JsonFormat(new ExtJsonPage { success = true, code = 1000, msg = "查询成功！", total = Total, list = list });
            else
                return JsonFormat(new ExtJsonPage { success = false, code = -1000, msg = "查询失败！" });
        }
        public ActionResult Login() {
            return View();
        }
        public ActionResult WebLogin()
        {
            string uid = CommonManager.WebObj.RequestForm("uid", "");
            if (uid != "")
            {
                Session["uid"] = uid;
                return JsonFormat(new ExtJson { success = true, msg = "登入成功" });
            }
            else {
                return JsonFormat(new ExtJson { success = false, msg = "登入失败" });
            }
        }
        public ActionResult TestLogin() {
            string UserName = CommonManager.WebObj.RequestForm("login", "");
            string Pwd = CommonManager.WebObj.RequestForm("password", "");
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Pwd)) {
                return JsonFormat(new ExtJson { success = false, msg = "账户和密码不能为空" });
            }
            TUserDto dto = new TUserDto();
            dto.UserName = UserName; dto.UserPwd = Pwd.MD5();
            int userid = _bll.VerifyTUsers(dto);
            if (userid != -1) {
                Session["uid"] = userid;
                //添加cookie,30天
                CommonManager.WebObj.AddCookie("user", UserName, 60 * 24 * 30);
                CommonManager.WebObj.AddCookie("pwd", Pwd, 60 * 24 * 30);
                return JsonFormat(new ExtJson { success = true, msg = "登入成功" });
            }
            return JsonFormat(new ExtJson { success = false, msg = "登入失败" });
        }
    }
}

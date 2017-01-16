using GetMoney.Application;
using GetMoney.Common;
using GetMoney.Common.Expand;
using GetMoney.Common.SerializeObject;
using GetMoney.Filters;
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
        public ActionResult UserMenuPortal()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult RegTUser() {
            return View();
        }
        public ActionResult TUserFriend()
        {
            return View();
        }
        public ActionResult TUserSearch()
        {
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
        /// <summary>
        /// 批量删除用户信息(ids数组)
        /// </summary>
        /// <returns></returns>
        public ActionResult Remove()
        {
            string data = CommonManager.WebObj.RequestForm("data", "");  //用户的IDS数组
            IList<UListDto> list = SerializeJson<UListDto>.JSONStringToList(data);
            bool result = _bll.RemoveTUsers(list);
            //bool result = false;
            if (result)
                return JsonFormat(new ExtJson { success = true, msg = "删除成功！" });
            else
                return JsonFormat(new ExtJson { success = false, msg = "删除失败！" });
        }
        /// <summary>
        /// 用户列表查询
        /// </summary>
        /// <returns></returns>
        public ActionResult ListTUserPage()
        {
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            string type = CommonManager.WebObj.Request("type", "");
            string text = CommonManager.WebObj.Request("text", "");
            string filter = "";
            if (!string.IsNullOrEmpty(text)&& !string.IsNullOrEmpty(type))
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
        /// <summary>
        /// 查询已经登入的用户的好友列表
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult MyListUserPage() {
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            string state = CommonManager.WebObj.Request("state", "");           //好友状态
            int uid = Convert.ToInt32(Session["uid"]);                          //登入的用户ID
            string type = CommonManager.WebObj.Request("type", "");
            string text = CommonManager.WebObj.Request("text", "");

            string filter = "";
            if (state == "1") filter = "Userid=" + uid + " AND State=1";         //我的好友
            else if (state == "2") filter = "Userid=" + uid + " AND State=2";    //我的黑名单
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
        /// <summary>
        /// 批量添加好友,好友的id数组列表
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Web用户登入(用户名,密码),生成用户ID的Session并且生成Cookie
        /// </summary>
        /// <returns></returns>
        public ActionResult WebLogin()
        {
            string UserName = CommonManager.WebObj.RequestForm("login", "");
            string Pwd = CommonManager.WebObj.RequestForm("password", "");
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Pwd))
            {
                return JsonFormat(new ExtJson { success = false, msg = "账户和密码不能为空" });
            }
            TUserDto dto = new TUserDto();
            dto.UserName = UserName; dto.UserPwd = Pwd.MD5();
            int userid = _bll.VerifyTUsers(dto);
            if (userid != -1)
            {
                Session["uid"] = userid;
                //添加cookie,30天
                CommonManager.WebObj.AddCookie("user", UserName, 60 * 24 * 30);
                CommonManager.WebObj.AddCookie("pwd", Pwd, 60 * 24 * 30);
                return JsonFormat(new ExtJson { success = true, msg = "登入成功" });
            }
            return JsonFormat(new ExtJson { success = false, msg = "登入失败" });
        }
        /// <summary>
        /// 方便不知道用户,密码的情况下临时使用
        /// </summary>
        /// <returns></returns>
        public ActionResult TestLogin() {
            string uid = CommonManager.WebObj.RequestForm("uid", "");
            if (uid != "")
            {
                Session["uid"] = uid;
                return JsonFormat(new ExtJson { success = true, msg = "登入成功" });
            }
            else
            {
                return JsonFormat(new ExtJson { success = false, msg = "登入失败" });
            }
        }
        public ActionResult WaterFall() {
            return JsonFormat(new ExtJson { success = false, msg = "登入失败" });
        }
    }
}

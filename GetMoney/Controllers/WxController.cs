using FJSZ.OA.Common.DEncrypt;
using FJSZ.OA.Common.Web;
using GetMoney.Application;
using GetMoney.Application.WeiX;
using GetMoney.Common;
using GetMoney.Framework;
using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetMoney.Controllers
{
    public class WxController : BaseController
    {
        ITUserBll _bll = null;
        public WxController(ITUserBll bll)
        {
            _bll = bll;
        }
        WeiXBLL wxbll = new WeiXBLL();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult WeiXLogin() {
            if (Request["p"] == null)
            {
                string c = "?c=" + DEncrypt.DESEncrypt1("SNS|1|" + WebHelp.GetCurHttpHost() + "/Wx/WeiXLogin");   //c参数进行加密
                string param = c;   //string param = Request.Url.Query + c; 参数串,例如:http://wx.ndll800.com/home/default?ctype=1&issue=1 取的param为:   ?ctype=1&issue=1
                CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WeiXLogin     param： " + param);
                string state = "";                  //state的值暂时为空,如果后面有需要验签,再用起来,现在就直接用参数来做校验
                //string url = wxbll.Wx_Auth_Code(wxbll.appid, System.Web.HttpUtility.UrlEncode(WebHelp.GetCurHttpHost() + "/WeiX/Wx_Auth_Code" + param), "snsapi_userinfo", state);  //snsapi_base,snsapi_userinfo
                string url = wxbll.Wx_Auth_Code(wxbll.appid, System.Web.HttpUtility.UrlEncode("http://wx.ndll800.com/WeiX/Wx_Auth_Code1" + param), "snsapi_userinfo", state);  //snsapi_base,snsapi_userinfo
                CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WeiXLogin     URL： " + url);
                return Redirect(url);
            }
            else
            {
                string openid = "", nickname = "", headurl = "";    //当没有自己的公众号的情况下,在这里进行数据写入,本来需要在ComExpendController->Wx_Auth_Code这里写入的,这里择中处理一下
                try
                {
                    string p = Request["p"].ToString(); //1|subscribe|openid  微信发送|是否关注|openid
                    CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WeiXLogin     p：" + Request["p"].ToString());
                    string temp = DEncrypt.DESDecrypt1(p);    //取得p参数,并且进行解密
                    CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WeiXLogin     p：" + temp);
                    string[] plist = temp.Split('|');   //微信发送|Openid|呢称|头像URL
                    if (plist[0] != "1") return Content("配置参数异常");
                    openid = plist[1]; headurl = plist[2]; nickname = Request["n"].ToString();
                    CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WeiXLogin     openid：" + openid + "headurl: " + headurl + "nickname: " + nickname);
                }
                catch (Exception er)
                {
                    CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WeiXLogin     异常：" + er.Message);
                    return Content("参数错误");
                }
                if (string.IsNullOrEmpty(openid))
                    return Content("授权失败");
                TUserDto userdto = new TUserDto();
                userdto = _bll.FindUserByUserName(openid);      //通过用openid来读取用户信息DTO
                CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "使用openid取得用户信息");
                if (userdto != null)
                {
                    if (userdto.State == 3) return Content("当前用户已被禁止登入");
                    Session["uid"] = userdto.id.ToString();                          //设置Session
                    var script = String.Format("<script>window.location.href='/TUser/TUserSearch';</script>");
                    return Content(script, "text/html");
                }
                else
                {
                    userdto = new TUserDto();
                    userdto.UserName = openid; userdto.NickName = nickname; userdto.TxUrl = headurl; userdto.UserPwd = TxtHelp.MD5("123"); userdto.BankPwd = TxtHelp.MD5("123");
                    int result = -1;    //注册结果
                    string msg = "注册成功!";
                    Dictionary<string, object> list = new Dictionary<string, object>();
                    _bll.RegTUser(userdto, out list);
                    if (list.Count > 0)
                    {
                        result = Convert.ToInt32(list["@ReturnValue"]);
                        if (result == 1) {
                            Session["uid"] = list["@Userid"].ToString();
                            var script = String.Format("<script>window.location.href='/TUser/TUserSearch';</script>");
                            return Content(script, "text/html");
                        }
                    }
                }
                CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "读取用户数据失败     openid：" + openid);
                return Content("读取用户信息失败");
            }
        }


        public ActionResult WxCreateFriend()
        {
            if (Session["Userid"] != null)
            {
                int parentid = Convert.ToInt32(CommonManager.WebObj.Request("pid", "0")); //师父ID
                int uid = Convert.ToInt32(Session["Userid"]); //徒弟ID
            }
            else {
                if (Request["p"] == null)
                {
                    string c = "?c=" + DEncrypt.DESEncrypt1("SNS|1|" + WebHelp.GetCurHttpHost() + "/Wx/WxCreateFriend");   //c参数进行加密
                    string param = c;   //string param = Request.Url.Query + c; 参数串,例如:http://wx.ndll800.com/home/default?ctype=1&issue=1 取的param为:   ?ctype=1&issue=1
                    CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WxCreateFriend     param： " + param);
                    string state = "";                  //state的值暂时为空,如果后面有需要验签,再用起来,现在就直接用参数来做校验
                    //string url = wxbll.Wx_Auth_Code(wxbll.appid, System.Web.HttpUtility.UrlEncode(WebHelp.GetCurHttpHost() + "/WeiX/Wx_Auth_Code" + param), "snsapi_userinfo", state);  //snsapi_base,snsapi_userinfo
                    string url = wxbll.Wx_Auth_Code(wxbll.appid, System.Web.HttpUtility.UrlEncode("http://wx.ndll800.com/WeiX/Wx_Auth_Code1" + param), "snsapi_userinfo", state);  //snsapi_base,snsapi_userinfo
                    CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WxCreateFriend     URL： " + url);
                    return Redirect(url);
                }
                else {
                    string openid = "", nickname = "", headurl = "";    //当没有自己的公众号的情况下,在这里进行数据写入,本来需要在ComExpendController->Wx_Auth_Code这里写入的,这里择中处理一下
                    try
                    {
                        string p = Request["p"].ToString(); //1|subscribe|openid  微信发送|是否关注|openid
                        CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WeiXLogin     p：" + Request["p"].ToString());
                        string temp = DEncrypt.DESDecrypt1(p);    //取得p参数,并且进行解密
                        CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WeiXLogin     p：" + temp);
                        string[] plist = temp.Split('|');   //微信发送|Openid|呢称|头像URL
                        if (plist[0] != "1") return Content("配置参数异常");
                        openid = plist[1]; headurl = plist[2]; nickname = Request["n"].ToString();
                        CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WeiXLogin     openid：" + openid + "headurl: " + headurl + "nickname: " + nickname);

                    }
                    catch (Exception er)
                    {
                        CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WeiXLogin     异常：" + er.Message);
                        return Content("参数错误");
                    }
                    if (string.IsNullOrEmpty(openid))
                        return Content("授权失败");
                }
            }
        }
    }
}

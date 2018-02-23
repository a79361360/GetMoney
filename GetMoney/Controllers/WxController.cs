using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetMoney.Controllers
{
    public class WxController : Controller
    {
        //
        // GET: /Wx/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult WxLogin() {
            return View();
            //if (Request["p"] == null)
            //{
            //    string c = "?c=" + DEncrypt.DESEncrypt1("SNS|1|" + WebHelp.GetCurHttpHost() + "/Login/WeiXLogin");   //c参数进行加密
            //    string param = c;   //string param = Request.Url.Query + c; 参数串,例如:http://wx.ndll800.com/home/default?ctype=1&issue=1 取的param为:   ?ctype=1&issue=1
            //    Common.Expend.LogTxtExpend.WriteLogs("/Logs/LoginController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "Index     param： " + param);
            //    string state = "";                  //state的值暂时为空,如果后面有需要验签,再用起来,现在就直接用参数来做校验
            //    //string url = wxbll.Wx_Auth_Code(wxbll.appid, System.Web.HttpUtility.UrlEncode(WebHelp.GetCurHttpHost() + "/WeiX/Wx_Auth_Code" + param), "snsapi_userinfo", state);  //snsapi_base,snsapi_userinfo
            //    string url = wxbll.Wx_Auth_Code(wxbll.appid, System.Web.HttpUtility.UrlEncode("http://wx.ndll800.com/WeiX/Wx_Auth_Code1" + param), "snsapi_userinfo", state);  //snsapi_base,snsapi_userinfo
            //    Common.Expend.LogTxtExpend.WriteLogs("/Logs/LoginController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "Index     URL： " + url);
            //    return Redirect(url);
            //}
            //else
            //{
            //    string openid = "", nickname = "", headurl = "";    //当没有自己的公众号的情况下,在这里进行数据写入,本来需要在ComExpendController->Wx_Auth_Code这里写入的,这里择中处理一下
            //    try
            //    {
            //        string p = Request["p"].ToString(); //1|subscribe|openid  微信发送|是否关注|openid
            //        Common.Expend.LogTxtExpend.WriteLogs("/Logs/LoginController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "Index     p：" + Request["p"].ToString());
            //        string temp = DEncrypt.DESDecrypt1(p);    //取得p参数,并且进行解密
            //        Common.Expend.LogTxtExpend.WriteLogs("/Logs/LoginController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "Index     p：" + temp);
            //        string[] plist = temp.Split('|');   //微信发送|Openid|呢称|头像URL
            //        if (plist[0] != "1") return Content("配置参数异常");
            //        openid = plist[1]; headurl = plist[2]; nickname = Request["n"].ToString();
            //        Common.Expend.LogTxtExpend.WriteLogs("/Logs/LoginController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "Index     openid：" + openid + "headurl: " + headurl + "nickname: " + nickname);
            //        wxbll.WeiX_Execute_User(0, 0, cbll.GetIp(), nickname, openid, headurl); //这里是临时择中处理,添加用户信息,有自己的公众号以后修改
            //    }
            //    catch (Exception er)
            //    {
            //        Common.Expend.LogTxtExpend.WriteLogs("/Logs/LoginController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "Index     异常：" + er.Message);
            //        return Content("参数错误");
            //    }
            //    if (string.IsNullOrEmpty(openid))
            //        return Content("授权失败");
            //    var dto = wxbll.WeiX_Execute_User(openid);      //通过用openid来读取用户信息
            //    Common.Expend.LogTxtExpend.WriteLogs("/Logs/LoginController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "使用openid取得用户信息");
            //    if (dto != null)
            //    {
            //        if (dto.UserDisable) return Content("当前用户已被禁止登入");
            //        Session["mly28User"] = dto.Userid.ToString();                          //设置Session
            //        var script = String.Format("<script>window.location.href='/WxJs28/Index';</script>");
            //        return Content(script, "text/html");
            //    }
            //    Common.Expend.LogTxtExpend.WriteLogs("/Logs/LoginController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "读取用户数据失败     openid：" + openid);
            //    return Content("读取用户信息失败");
            //}
        }
    }
}

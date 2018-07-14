using FJSZ.OA.Common.DEncrypt;
using FJSZ.OA.Common.Web;
using GetMoney.Application;
using GetMoney.Application.WeiX;
using GetMoney.Common;
using GetMoney.Common.Expand;
using GetMoney.Framework;
using GetMoney.Model;
using GetMoney.Model.WxModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

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
        public ActionResult LoginPortal()
        {
            string backurl = "/TUser/TUserWxOrder"; //默认回调地址
            if (Request["backurl"] != null)
                backurl = Request["backurl"].ToString();
            string url = WebHelp.GetCurHttpHost() + "/Wx/WxLogin?backurl=" + backurl;
            //string url1 = WebHelp.GetCurHttpHost() + "/Wx/WxLogin?backurl=" + backurl;
            ViewBag.url = url;
            //ViewBag.url1 = url1;
            return View();
        }
        public ActionResult WeiXLogin()
        {
            if (Request["p"] == null)
            {
                string c = "?c=" + DEncrypt.DESEncrypt1("SNS|1|" + WebHelp.GetCurHttpHost() + "/Wx/WeiXLogin");   //c参数进行加密
                string param = c;   //string param = Request.Url.Query + c; 参数串,例如:http://wx.ndll800.com/home/default?ctype=1&issue=1 取的param为:   ?ctype=1&issue=1
                CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WeiXLogin     param： " + param);
                string state = "";                  //state的值暂时为空,如果后面有需要验签,再用起来,现在就直接用参数来做校验
                if (Request["backurl"] != null) state = Request["backurl"].ToString();
                //string url = wxbll.Wx_Auth_Code(wxbll.appid, System.Web.HttpUtility.UrlEncode(WebHelp.GetCurHttpHost() + "/WeiX/Wx_Auth_Code" + param), "snsapi_userinfo", state);  //snsapi_base,snsapi_userinfo
                string url = wxbll.Wx_Auth_Code(wxbll.appid, System.Web.HttpUtility.UrlEncode("http://wx.ndll800.com/WeiX/Wx_Auth_Code1" + param), "snsapi_userinfo", state);  //snsapi_base,snsapi_userinfo
                CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WeiXLogin     URL： " + url);
                return Redirect(url);
            }
            else
            {
                string openid = "", nickname = "", headurl = "", backurl = "/TUser/TUserWxOrder";    //当没有自己的公众号的情况下,在这里进行数据写入,本来需要在ComExpendController->Wx_Auth_Code这里写入的,这里择中处理一下
                try
                {
                    string p = Request["p"].ToString(); //1|subscribe|openid  微信发送|是否关注|openid
                    CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WeiXLogin     p：" + Request["p"].ToString() + "URL: " + Request.RawUrl);
                    string temp = DEncrypt.DESDecrypt1(p);    //取得p参数,并且进行解密
                    CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WeiXLogin     temp：" + temp);
                    string[] plist = temp.Split('|');   //微信发送|Openid|呢称|头像URL
                    if (plist[0] != "1") return Content("配置参数异常");
                    openid = plist[1]; headurl = plist[2]; nickname = Request["n"].ToString();
                    if (!string.IsNullOrEmpty(Request["state"].ToString()))
                        backurl = Request["state"].ToString();
                    CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WeiXLogin     openid：" + openid + "headurl: " + headurl + "nickname: " + nickname + "backurl: " + backurl);
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
                    CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "存在用户信息: " + userdto.id);
                    if (userdto.State == 3) return Content("当前用户已被禁止登入");
                    Session["uid"] = userdto.id.ToString();                          //设置Session
                    var script = String.Format("<script>window.location.href='" + backurl + "';</script>");
                    return Content(script, "text/html");
                }
                else
                {
                    CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "不存在用户信息开始注册: ");
                    userdto = new TUserDto();
                    userdto.UserName = openid; userdto.NickName = nickname; userdto.TxUrl = headurl; userdto.UserPwd = TxtHelp.MD5("123"); userdto.BankPwd = TxtHelp.MD5("123");
                    int result = -1;    //注册结果
                    string msg = "注册成功!";
                    Dictionary<string, object> list = new Dictionary<string, object>();
                    _bll.RegTUser(userdto, out list);
                    if (list.Count > 0)
                    {
                        result = Convert.ToInt32(list["@ReturnValue"]);
                        if (result == 1)
                        {
                            CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "注册成功: " + list["@Userid"].ToString());
                            Session["uid"] = list["@Userid"].ToString();
                            var script = String.Format("<script>window.location.href='" + backurl + "';</script>");
                            return Content(script, "text/html");
                        }
                    }
                }
                CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "读取用户数据失败     openid：" + openid);
                return Content("读取用户信息失败");
            }
        }
        /// <summary>
        /// 自己的公众号登入
        /// </summary>
        public ActionResult WxLogin() {
            string backurl = CommonManager.WebObj.Request("backurl", "");
            CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WxLogin backurl：" + backurl);
            string url = "";
            if (Session["uid"] == null)
            {
                url = wxbll.RedirectWx(backurl);
                CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WxLogin url：" + url);
                return Redirect(url);
            }
            else return Redirect(backurl);
        }
        /// <summary>
        /// 微信授权
        /// </summary>
        /// <returns></returns>
        public ActionResult WxAccount() {
            string code = CommonManager.WebObj.Request("code", "");
            string state = CommonManager.WebObj.Request("state", "");
            CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WxAccount code：" + code + "State：" + state);
            string openid = "", nickname = "", headurl = "", backurl = "/TUser/TUserWxOrder"; TUserDto userdto = new TUserDto();
            if (!string.IsNullOrEmpty(Request["state"].ToString())) backurl = Request["state"].ToString();  //state非空,跳转地址付值
            var dto = wxbll.Wx_SNS_AccessToken(wxbll.appid, wxbll.appsecret, code); openid = dto.openid;   //取得用户信息,并附值给openid
            userdto = _bll.FindUserByUserName(openid);                                                      //通过用openid来读取用户信息DTO
            CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WxAccount 使用openid取得用户信息");
            if (userdto != null)
            {
                CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WxAccount 存在用户信息: " + userdto.id);
                if (userdto.State == 3) return Content("当前用户已被禁止登入");
                Session["uid"] = userdto.id.ToString();                          //设置Session
                var script = String.Format("<script>window.location.href='" + backurl + "';</script>");
                return Content(script, "text/html");
            }
            else
            {
                CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WxAccount 不存在用户信息开始注册: ");
                userdto = new TUserDto();
                var wx_user = wxbll.Get_SNS_UserInfo(openid, dto.access_token);
                userdto.UserName = openid; userdto.NickName = wx_user.nickname; userdto.TxUrl = wx_user.headimgurl; userdto.UserPwd = TxtHelp.MD5("123"); userdto.BankPwd = TxtHelp.MD5("123");    //默认密码123
                int result = -1;    //注册结果
                string msg = "注册成功!";
                Dictionary<string, object> list = new Dictionary<string, object>();
                _bll.RegTUser(userdto, out list);
                if (list.Count > 0)
                {
                    result = Convert.ToInt32(list["@ReturnValue"]);
                    if (result == 1)
                    {
                        CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "注册成功: " + list["@Userid"].ToString());
                        Session["uid"] = list["@Userid"].ToString();
                        var script = String.Format("<script>window.location.href='" + backurl + "';</script>");
                        return Content(script, "text/html");
                    }
                }
            }
            CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "读取用户数据失败     openid：" + openid);
            return Content("读取用户信息失败");
        }

        public ActionResult WxCreateFriend()
        {
            if (Session["uid"] == null)
            {
                string url = Request.Url.PathAndQuery;
                CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "WxCreateFriend     Request.Url.PathAndQuery：" + url);
                return Redirect("/Wx/WeiXLogin?backurl=" + url);
            }
            else
            {
                int parentid = Convert.ToInt32(CommonManager.WebObj.Request("pid", "0")); //师父ID
                if (parentid == 0) return Content("参数不能为空");
                var dto = _bll.FindUserById(parentid);      //通过用openid来读取用户信息DTO
                if (dto == null) return Content("不存在当前用户信息");
                ViewBag.pid = parentid;                 //用户ID
                ViewBag.NickName = dto.NickName;        //用户呢称
                ViewBag.TxUrl = dto.TxUrl;              //用户头象
                int uid = Convert.ToInt32(Session["uid"]); //徒弟ID
                ViewBag.uid = uid;          //好友ID
            }
            return View();
        }
        /// <summary>
        /// 服务器配置接口
        /// </summary>
        /// <returns></returns>
        public ActionResult WxMsgToken() {
            //string result = "";
            //result = wxbll.CheckSignature();
            //return Content(result);

            CommonManager.TxtObj.WriteLogs("/Logs/WxMsgToken_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "开始: ");
            Stream requestStream = Request.InputStream;
            byte[] requestByte = new byte[requestStream.Length];
            requestStream.Read(requestByte, 0, (int)requestStream.Length);
            string s = Encoding.UTF8.GetString(requestByte);
            CommonManager.TxtObj.WriteLogs("/Logs/WxMsgToken_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "获取输入流字符串: " + s);
            //            s = @"<xml><ToUserName><![CDATA[gh_c414afdbd6d7]]></ToUserName>
            //<FromUserName><![CDATA[oMO1C0fMcme_T4nDKXAREkUoGZWc]]></FromUserName>
            //<CreateTime>1529412251</CreateTime>
            //<MsgType><![CDATA[text]]></MsgType>
            //<Content><![CDATA[texy]]></Content>
            //<MsgId>6568775600578283548</MsgId>
            //</xml>";
            //封装请求类
            XmlDocument requestDocXml = new XmlDocument();
            requestDocXml.LoadXml(s);
            XmlElement rootElement = requestDocXml.DocumentElement;
            WxXmlModel WxXmlModel = new WxXmlModel();
            //XmlSerializer serializer = new XmlSerializer(typeof(WxXmlModel));
            //var result = (WxXmlModel)serializer.Deserialize(new MemoryStream(Encoding.Unicode.GetBytes(s)));
            WxXmlModel.ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText;
            WxXmlModel.FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText;
            WxXmlModel.CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText;
            WxXmlModel.MsgType = rootElement.SelectSingleNode("MsgType").InnerText;
            string type = "", result = "";
            switch (WxXmlModel.MsgType)
            {
                case "text":
                    CommonManager.TxtObj.WriteLogs("/Logs/WxMsgToken_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "开始text: " + WxXmlModel.MsgType);
                    result = "<xml><ToUserName><![CDATA[" + WxXmlModel.FromUserName + "]]></ToUserName>";
                    result += "<FromUserName><![CDATA[" + WxXmlModel.ToUserName + "]]></FromUserName>";
                    result += "<CreateTime>" + DateTime.Now.ToUnixTimeStamp() + "</CreateTime>";
                    result += "<MsgType><![CDATA[transfer_customer_service]]></MsgType></xml>";
                    CommonManager.TxtObj.WriteLogs("/Logs/WxMsgToken_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "result: " + result);
                    //创建菜单
                    WxXmlModel.Content = rootElement.SelectSingleNode("Content").InnerText;
                    if (WxXmlModel.Content == "createmenu_qiu")
                        wxbll.CreateMenu();
                    break;
                case "image":
                    CommonManager.TxtObj.WriteLogs("/Logs/WxMsgToken_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "开始image: ");
                    break;
                case "event":
                    WxXmlModel.Event = rootElement.SelectSingleNode("Event").InnerText;
                    WxXmlModel.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;
                    if (WxXmlModel.Event == "subscribe")
                    {
                        result = "<xml><ToUserName><![CDATA[" + WxXmlModel.FromUserName + "]]></ToUserName>";
                        result += "<FromUserName><![CDATA[" + WxXmlModel.ToUserName + "]]></FromUserName>";
                        result += "<CreateTime>" + DateTime.Now.ToUnixTimeStamp() + "</CreateTime>";
                        result += "<MsgType><![CDATA[text]]></MsgType>";
                        result += "<Content><![CDATA[欢迎关注《宁德极速代码》！\n <a href='http://gm.cf518.cn/wxpay/index'>成为会员</a>]]></Content></xml>";
                        CommonManager.TxtObj.WriteLogs("/Logs/WxMsgToken_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "result: " + result);
                    }
                    if (WxXmlModel.Event == "CLICK")
                    {
                        if (WxXmlModel.EventKey == "button_0102")
                        {
                            result = "<xml><ToUserName><![CDATA[" + WxXmlModel.FromUserName + "]]></ToUserName>";
                            result += "<FromUserName><![CDATA[" + WxXmlModel.ToUserName + "]]></FromUserName>";
                            result += "<CreateTime>" + DateTime.Now.ToUnixTimeStamp() + "</CreateTime>";
                            result += "<MsgType><![CDATA[text]]></MsgType>";
                            result += "<Content><![CDATA[联系客服微信:np059988\n 客服工作时间为每日8: 00 - 23:00]]></Content></xml>";
                        }
                    }
                    break;
                default:
                    CommonManager.TxtObj.WriteLogs("/Logs/WxMsgToken_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "开始default: " + WxXmlModel.MsgType);
                    break;

            }
            //Response.ContentType = "text/xml";
            //Response.Write(result);
            //Response.End();
            return Content(result, "text/xml");
        }
    }
}
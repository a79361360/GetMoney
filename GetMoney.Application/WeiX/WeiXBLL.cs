using FJSZ.OA.Common.Web;
using GetMoney.Common;
using GetMoney.Common.Cache;
using GetMoney.Dal.WeiX;
using GetMoney.Model;
using GetMoney.Model.WxModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace GetMoney.Application.WeiX
{
    public class WeiXBLL
    {
        WebHttp web = new WebHttp();
        WeiXDAL dal = new WeiXDAL();
        //public string appid = "wx0d8924c9bc2c6e11";
        //public string appsecret = "9c0125be80b710c17e09124f13c82b24";
        public string appid = "wx77e5a850663d5baa";                     //极速代码
        public string appsecret = "0e7dccb9f2587c0832dfb12b82c47045";   //密钥
        public string Wx_Auth_Code(string appid, string backurl, string scope, string state)
        {
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appid + "&redirect_uri=" + backurl + "&response_type=code&scope=" + scope + "&state=" + state + "#wechat_redirect";
            return url;
        }
        /// <summary>
        /// 全局授权token
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public WxJsApi_token Wx_Cgi_AccessToken(string appid, string secret)
        {
            var dto = (WxJsApi_token)CommonManager.CacheObj.Get<AspNetCache>("cgi_token");
            //WxJsApi_token dto = null;
            if (dto == null)
            {
                string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret;
                string result = web.Get(url);
                dto = JsonConvert.DeserializeObject<WxJsApi_token>(result);
                CommonManager.CacheObj.Save<AspNetCache>("cgi_token", dto, 120, DateTime.Now);
            }
            return (WxJsApi_token)dto;
        }
        //取得SNS网页授权token
        public WxJsApi_token Wx_SNS_AccessToken(string appid, string secret, string code)
        {
            //WxJsApi_token dto = (WxJsApi_token)CommonManager.CacheObj.Get<AspNetCache>("sns_token");
            //if (dto == null)
            //{
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + appid + "&secret=" + secret + "&code=" + code + "&grant_type=authorization_code";
            string result = web.Get(url);
            WxJsApi_token dto = JsonConvert.DeserializeObject<WxJsApi_token>(result);
            CommonManager.CacheObj.Save<AspNetCache>("sns_token", dto, 120, DateTime.Now);
            //}
            return dto;
        }
        /// <summary>
        /// SNS微信用户的详细信息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Wx_UserInfo Get_SNS_UserInfo(string openid, string token)
        {
            string url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + token + "&openid=" + openid + "&lang=zh_CN";
            string result = web.Get(url);
            Wx_UserInfo dto = JsonConvert.DeserializeObject<Wx_UserInfo>(result);
            return dto;
        }
        public string RedirectWx(string reurl)
        {
            CommonManager.TxtObj.WriteLogs("/Logs/WeiXBLL_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "RedirectWx 开始去微信获取授权" + reurl + WebHelp.GetCurHttpHost());
            string redirect_uri = HttpUtility.UrlEncode(WebHelp.GetCurHttpHost() + "/Wx/WxAccount");
            string response_type = "code";
            string scope = "snsapi_userinfo";   //snsapi_userinfo不仅只取到openid其他信息也一起取到,snsapi_base只取openid
            string state = reurl;
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appid + "&redirect_uri=" + redirect_uri + "&response_type=" + response_type + "&scope=" + scope + "&state=" + state + "#wechat_redirect";
            CommonManager.TxtObj.WriteLogs("/Logs/WeiXBLL_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "RedirectWx url" + url);
            return url;
        }
        /// <summary>
        /// 服务器配置 验证url有效性
        /// </summary>
        public string CheckSignature()
        {
            string token = "f081bfda13723a69a9b716d1dda129a7";
            if (string.IsNullOrEmpty(token))
            {
                return "";
            }
            string signature = CommonManager.WebObj.Request("signature", "");
            CommonManager.TxtObj.WriteLogs("/Logs/CheckSignature_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "signature: " + signature);
            string timestamp = CommonManager.WebObj.Request("timestamp", "");
            CommonManager.TxtObj.WriteLogs("/Logs/CheckSignature_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "timestamp: " + timestamp);
            string nonce = CommonManager.WebObj.Request("nonce", "");
            CommonManager.TxtObj.WriteLogs("/Logs/CheckSignature_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "nonce: " + nonce);
            string echoString = CommonManager.WebObj.Request("echoStr", "");
            CommonManager.TxtObj.WriteLogs("/Logs/CheckSignature_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "echoString: " + echoString);
            //排序
            List<string> list = new List<string>();
            list.Add(token);
            list.Add(timestamp);
            list.Add(nonce);
            list.Sort();

            //加密
            string res = string.Join("", list.ToArray());
            CommonManager.TxtObj.WriteLogs("/Logs/CheckSignature_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "res: " + res);
            res = FormsAuthentication.HashPasswordForStoringInConfigFile(res, "SHA1").ToLower();
            CommonManager.TxtObj.WriteLogs("/Logs/CheckSignature_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "res: " + res);
            if (signature == res)
            {
                //HttpContext.Current.Response.Write(echoString);
                //HttpContext.Current.Response.End();
                return echoString;
            }
            return "";
        }
        public void CreateMenu() {
            CommonManager.TxtObj.WriteLogs("/Logs/CreateMenu_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "CreateMenu开始: ");
            Wx_Menu dto_menu = new Wx_Menu();                           //总菜单对象
            List<Wx_Menu_button> list = new List<Wx_Menu_button>();     //子菜单列表
            Wx_Menu_button dto = new Wx_Menu_button();                  //子菜单对象
            dto.type = "view"; dto.name = "会单入口"; dto.url = "http://gm.cf518.cn/TUser/TUserWxOrder"; list.Add(dto);
            //dto.name = "游戏中心";
            //List<Wx_Menu_button> list1 = new List<Wx_Menu_button>();
            //Wx_Menu_button dto1 = new Wx_Menu_button();
            //dto1.type = "view"; dto1.name = "游戏下载"; dto1.url = "http://fkwx.0599yx.com/WeiXin/Download.aspx"; list1.Add(dto1);
            //dto1 = new Wx_Menu_button();
            ////dto1.type = "click"; dto1.name = "联系客服"; dto1.key = "button_0102"; list1.Add(dto1);
            //dto1.type = "view"; dto1.name = "联系客服"; dto1.url = "https://fzlwhywlkjyxgs.qiyukf.com/client?k=17a6055ef74032942d6a13c61ef988d7&wp=1&robotShuntSwitch=0"; list1.Add(dto1);
            ////添加二级菜单到一级菜单游戏中心
            //dto.sub_button = list1; list.Add(dto);
            ////领取福利
            //dto = new Wx_Menu_button(); //实例化一级菜单领取福利
            //dto.name = "领取福利";
            //list1 = new List<Wx_Menu_button>();
            //dto1 = new Wx_Menu_button();
            //dto1.type = "view"; dto1.name = "领取福利"; dto1.url = "http://fkwx.0599yx.com/WeiXin/RedPacket.aspx"; list1.Add(dto1);
            //dto1 = new Wx_Menu_button();
            //dto1.type = "view"; dto1.name = "购买房卡"; dto1.url = "http://fkwx.0599yx.com/pay/wxpay.aspx"; list1.Add(dto1);
            ////添加二级菜单到一级菜单领取福利
            //dto.sub_button = list1; list.Add(dto);
            ////个人中心
            //dto = new Wx_Menu_button(); //实例化一级菜单个人中心
            //dto.name = "个人中心";
            //list1 = new List<Wx_Menu_button>();
            //dto1 = new Wx_Menu_button();
            //dto1.type = "view"; dto1.name = "我的账户"; dto1.url = "http://fkwx.0599yx.com/WeiXin/MyInfo.aspx"; list1.Add(dto1);
            //dto1 = new Wx_Menu_button();
            //dto1.type = "view"; dto1.name = "邀请有礼"; dto1.url = "http://fkwx.0599yx.com/WeiXin/Generalize.aspx"; list1.Add(dto1);
            //dto.sub_button = list1; list.Add(dto);
            //将菜单列表绑定到Menu
            dto_menu.button = list;

            WxJsApi_token dto_Cgi = Wx_Cgi_AccessToken(appid, appsecret);      //access_token
            CommonManager.TxtObj.WriteLogs("/Logs/CreateMenu_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "access_token: " + dto_Cgi.access_token);
            string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + dto_Cgi.access_token;
            string postdata = "" + JsonConvert.SerializeObject(dto_menu, Newtonsoft.Json.Formatting.Indented);
            string result = CommonManager.WebObj.Post(url, postdata);
            CommonManager.TxtObj.WriteLogs("/Logs/CreateMenu_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "CreateMenu: " + result);
        }
        public void WxTemplate_Expire() {
            IOrderBll _bll_1 = new OrderBll();
            ITUserBll _bll = new TUserBll();
            var list = _bll_1.FindCurOrderList();
            if (list.Count > 0)
            {
                //CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "公众号授权 WxTemplate_Expire：");
                var token = Wx_Cgi_AccessToken(appid, appsecret);
                foreach (var item in list)
                {
                    if (item.Userid != 10000) return;
                    var dto_user = _bll.FindUserById(Convert.ToInt32(item.Userid));
                    if (dto_user != null)
                    {
                        Wx_Template dto = new Wx_Template();
                        dto.touser = dto_user.UserName; dto.template_id = "z2v9mM3S6vEBXJ6B-eXKCYaXbavhoDddIA9osrSEAdc";
                        Wx_Template_data dto1 = new Wx_Template_data();
                        Wx_Template_data_dic dto2 = new Wx_Template_data_dic();
                        dto2.value = "尊敬的 " + dto_user.TrueName + " 您预约的标会日期已到"; dto2.color = "#173177"; dto1.first = dto2;
                        dto2 = new Wx_Template_data_dic();
                        dto2.value = "标会提醒"; dto2.color = "#173177"; dto1.keyword1 = dto2;
                        dto2 = new Wx_Template_data_dic();
                        dto2.value = item.Lastdate; dto2.color = "#173177"; dto1.keyword2 = dto2;
                        dto2 = new Wx_Template_data_dic();
                        dto2.value = "于【" + item.Lastdate + "】您有一个标会的行程,总会款为:【￥" + item.AccrualMoney + "】,您的会费为:【￥" + item.StayPayNum + "】,您的利息为:【￥" + item.StayPayTax + "】."; dto2.color = "#173177"; dto1.remark = dto2;
                        dto.data = dto1;
                        //CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "开始推送公众号消息 WxTemplate_Expire：");
                        string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + token.access_token;
                        string postdata = JsonConvert.SerializeObject(dto, Newtonsoft.Json.Formatting.Indented);
                        string result = CommonManager.WebObj.Post(url, postdata);
                    }
                }
            }
        }
    }
}

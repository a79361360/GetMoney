using FJSZ.OA.Common.Web;
using GetMoney.Dal.WeiX;
using GetMoney.Model;
using GetMoney.Model.WxModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Application.WeiX
{
    public class WeiXBLL
    {
        WebHttp web = new WebHttp();
        WeiXDAL dal = new WeiXDAL();
        public string appid = "wx0d8924c9bc2c6e11";
        public string appsecret = "9c0125be80b710c17e09124f13c82b24";
        public string Wx_Auth_Code(string appid, string backurl, string scope, string state)
        {
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appid + "&redirect_uri=" + backurl + "&response_type=code&scope=" + scope + "&state=" + state + "#wechat_redirect";
            return url;
        }
        //取得SNS网页授权token
        public WxJsApi_token Wx_Auth_AccessToken(string appid, string secret, string code)
        {
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + appid + "&secret=" + secret + "&code=" + code + "&grant_type=authorization_code";
            string result = web.Get(url);
            WxJsApi_token dto = JsonConvert.DeserializeObject<WxJsApi_token>(result);
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
    }
}

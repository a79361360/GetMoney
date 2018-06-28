using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Model.WxModel
{
    public class WxModel
    {
    }
    public class WxJsApi_token
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
    }
    public class Wx_UserInfo
    {
        public int subscribe { get; set; }
        public string openid { get; set; }
        public string nickname { get; set; }
        public int sex { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string language { get; set; }
        public string headimgurl { get; set; }
        public string subscribe_time { get; set; }
        public string unionid { get; set; }
        public string remark { get; set; }
        public string groupid { get; set; }
    }
    /// <summary>
    /// 自定义菜单
    /// </summary>
    public class Wx_Menu {
        public List<Wx_Menu_btton> button { get; set; }
    }
    public class Wx_Menu_btton {
        public string type { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string url { get; set; }
        public List<Wx_Menu_btton> sub_button { get; set; }
    }
    /// <summary>
    /// 个性化菜单
    /// </summary>
    public class Wx_CondMenu{
        public Wx_Menu menu { get; set; }
        public Wx_Menu conditionalmenu { get; set; }
    }
}

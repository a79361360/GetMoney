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
    public class Wx_Menu
    {
        public List<Wx_Menu_button> button { get; set; }
    }
    public class Wx_Menu_button
    {
        public string type { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string url { get; set; }
        public List<Wx_Menu_button> sub_button { get; set; }
    }
    public class WxJsApi_ticket
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string ticket { get; set; }
        public string expires_in { get; set; }
    }
    /// <summary>
    /// 微信接口XmlModel
    /// XML解析
    /// </summary>
    public class WxXmlModel
    {
        /// <summary>
        /// 消息接收方微信号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 消息发送方微信号
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 信息类型 地理位置:location,文本消息:text,消息类型:image
        /// </summary>
        public string MsgType { get; set; }
        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string Location_X { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y { get; set; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale { get; set; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 图片链接，开发者可以用HTTP GET获取
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 事件类型，subscribe(订阅/扫描带参数二维码订阅)、unsubscribe(取消订阅)、CLICK(自定义菜单点击事件) 、SCAN（已关注的状态下扫描带参数二维码）
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 事件KEY值
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 二维码的ticket，可以用来换取二维码
        /// </summary>
        public string Ticket { get; set; }
    }
    public class Wx_Template
    {
        public string touser { get; set; }
        public string template_id { get; set; }
        public string url { get; set; }
        public string miniprogram { get; set; }
        public string appid { get; set; }
        public string pagepath { get; set; }
        public Wx_Template_data data { get; set; }
        public string color { get; set; }
    }
    public class Wx_Template_data
    {
        public Wx_Template_data_dic first { get; set; }
        public Wx_Template_data_dic remark { get; set; }
        public Wx_Template_data_dic keyword1 { get; set; }
        public Wx_Template_data_dic keyword2 { get; set; }
        public Wx_Template_data_dic keyword3 { get; set; }
        public Wx_Template_data_dic keyword4 { get; set; }
        public Wx_Template_data_dic keyword5 { get; set; }
    }
    public class Wx_Template_data_dic
    {
        public string value { get; set; }
        public string color { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Model
{
    public class OrderDto
    {
        public int ID { get; set; }
        //会单号
        public string OrderNo { get; set; }
        /// <summary>
        /// 用户数量
        /// </summary>
        public int PeoperNum { get; set; }
        /// <summary>
        /// 用户ID列表
        /// </summary>
        public string PeoperIds { get; set; }
        /// <summary>
        /// 每个用户每次标会需要上缴的标准金额
        /// </summary>
        public int PeoperMoney { get; set; }
        /// <summary>
        /// 除首次和最后次外最低标金金额
        /// </summary>
        public int LowestMoney { get; set; }
        /// <summary>
        /// 会头UserID
        /// </summary>
        public int TouUserid { get; set; }
        /// <summary>
        /// 会头TrueName
        /// </summary>
        public string TouTrueName { get; set; }
        /// <summary>
        /// 会头Phone
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 首次加标日期
        /// </summary>
        public DateTime FirstExtraDate { get; set; }
        /// <summary>
        /// 自定义加标列表
        /// </summary>
        public string ExtraDate { get; set; }
        /// <summary>
        /// 会钱总额
        /// </summary>
        public int AllPeoperMoney
        {
            get { return PeoperNum * PeoperMoney; }
        }
        /// <summary>
        /// 会钱总额发放类型(1为全额发放,每月需要在标准金额上补充标息,2为减掉标息后发放,每还是只需要上缴标准金额)
        /// </summary>
        public MnSdTypeEnum MoneySendType { get; set; }
        /// <summary>
        /// MoneySendType的Name保存给View使用
        /// </summary>
        public string MSType { get; set; }
        /// <summary>
        /// 标会类型(1约定标会日期,2间隔30天标会),暂时只有第一种形式
        /// </summary>
        public int MeetType { get; set; }
        /// <summary>
        /// 首次标会日期
        /// </summary>
        public DateTime FirstDate { get; set; }
        /// <summary>
        /// 指定每个月标会次数(暂定最多5次)
        /// </summary>
        public int MeetNum { get; set; }
        //录入时间
        public DateTime InputDate { get; set; }
        /// <summary>
        /// 标会地址
        /// </summary>
        public string Address { get; set; }
        //当前会的状态(1活会,2死会,3险会)
        public int State { get; set; }
        //备注
        public string Remark { get; set; }
        /// <summary>
        /// 会单每期记录
        /// </summary>
        public IList<OrderListDto> List { get; set; }
    }
}

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
        /// 会钱总额
        /// </summary>
        public int AllPeoperMoney
        {
            get { return PeoperNum * PeoperMoney; }
        }
        /// <summary>
        /// 会钱总额发放类型(1为全额发放,每月需要在标准金额上补充标息,2为减掉标息后发放,每还是只需要上缴标准金额)
        /// </summary>
        public int MoneySendType { get; set; }
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
        /// <summary>
        /// 每个月标会日期(以字符串形式奖5个标会日期分开,以逗号隔开Len(14)例如:01,10,15)
        /// </summary>
        public string MeetDate { get; set; }
        /// <summary>
        /// 标会时间(具体的时间:例如晚上7点就是:    19:00)
        /// </summary>
        public DateTime MeetTime { get; set; }
        //录入时间
        public DateTime InputDate { get; set; }
        //当前会的状态(1活会,2死会,3险会)
        public int State { get; set; }
        //备注
        public string Remark { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Model
{
    public class OrderListDto
    {
        public int ID { get; set; }
        /// <summary>
        /// 会单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Userid { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 夺标金额
        /// </summary>
        public int AccrualMoney { get; set; }
        /// <summary>
        /// 开标日期
        /// </summary>
        public DateTime MeetDate { get; set; }
        /// <summary>
        /// 当前月的状态(1结束,2未结束)
        /// </summary>
        public string State { get; set; }
    }
}

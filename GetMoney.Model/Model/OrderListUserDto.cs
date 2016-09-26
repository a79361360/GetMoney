using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Model
{
    public class OrderListUserDto
    {
        public int ID { get; set; }
        /// <summary>
        /// 会单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 记录ID号
        /// </summary>
        public int OrderListID { get; set; }
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
        /// 添加时间
        /// </summary>
        public DateTime Addtime { get; set; }
        /// <summary>
        /// 最后修改标金的时间
        /// </summary>
        public string Lastdate { get; set; }
    }
}

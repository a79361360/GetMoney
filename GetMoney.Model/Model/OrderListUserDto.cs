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
        /// <summary>
        /// 待支付会费金额
        /// </summary>
        public int StayPayNum { get; set; }
        /// <summary>
        /// 待支付标息金额
        /// </summary>
        public int StayPayTax { get; set; }
        /// <summary>
        /// 实际用户支付金额
        /// </summary>
        public int RealPayNum { get; set; }
        /// <summary>
        /// 实现支付时间
        /// </summary>
        public string PayDate { get; set; }
        /// <summary>
        /// 支付状态0为未付,1为已付,2未全付
        /// </summary>
        public int PayState { get; set; }
        
    }
}

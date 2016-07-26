using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Model
{
    public class CardDto
    {
        public int ID { get; set; }
        public int CardCode { get; set; }
        public string CardName { get; set; }
        //卡的银行类型例:建行信用卡,淘宝花呗,建行储蓄卡
        public int CardBankType { get; set; }
        //卡的使用类型(信用1资金平台2借贷3)
        public int CardUseType { get; set; }
        //信用卡的使用额度(借贷卡额度为0)
        public int CardAmount { get; set; }
        //账单日
        //延期时长(用来计算最后还款日/到期日)
        public DateTime CardBillDate { get; set; }
        public int CardDelayDay { get; set; }
        //录入时间
        public DateTime CardInputDate { get; set; }
        //备注
        public string Remark { get; set; }
    }
}

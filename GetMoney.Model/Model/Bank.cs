using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Model
{
    public class Bank
    {
        //银行ID
        public int BankCode { get; set; }
        //银行名称
        public int BankName { get; set; }
        //银行类型:银行1,资金平台2
        public BankTypeEnum BankType { get; set; }
    }
}

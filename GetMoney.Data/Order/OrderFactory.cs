using GetMoney.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Data.Order
{
    public class OrderFactory
    {
        public static Order Create(
            string OrderNo,
            int PeoperNum,
            int PeoperMoney,
            int MoneySendType,
            int MeetType,
            int MeetNum,
            string MeetDate,
            string MeetTime,
            DateTime InputDate,
            int State,
            string Remark)
        {
            return new Order
            {
                OrderNo = OrderNo,
                PeoperNum = PeoperNum,
                PeoperMoney = PeoperMoney,
                MoneySendType = (MnSdTypeEnum)MoneySendType,
                MeetType = MeetType,
                MeetNum = MeetNum,
                MeetDate = MeetDate,
                MeetTime = MeetTime,
                InputDate = InputDate,
                State = State,
                Remark = Remark
            };
        }
    }
}

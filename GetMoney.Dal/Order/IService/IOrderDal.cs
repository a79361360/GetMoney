using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GetMoney.Dal
{
    public interface IOrderDal
    {
        DataTable ListOrderPage(ref int Total, SqlPageParam Param);
        void CreateOrder(string OrderNo, int PeoperNum, string UserIds, int PeoperMoney, int MoneySendType, int MeetType, int MeetNum, DateTime FirstDate, string MeetDate, DateTime MeetTime, out Dictionary<string, object> list);
    }
}

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
        void CreateOrder(string OrderNo, int PeoperNum, string UserIds, int PeoperMoney, int MoneySendType, int MeetType, int MeetNum, DateTime FirstDate, string MeetDate, string MeetTime, out Dictionary<string, object> list);
        DataTable OrderLists(string No);
        DataTable OrderListUser(string OrderListID);
        DataTable UpdateOrderListState(string OrderNo, string OrderListID);
        int UpdateOrderListUserMoney(string OrderNo, string OrderListID, int Userid, int Money);
        DataTable GetOrderListUserPrvMoney(int Userid, string OrderListID);
    }
}

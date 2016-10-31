﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GetMoney.Dal
{
    public interface IOrderDal
    {
        DataTable ListOrderPage(ref int Total, SqlPageParam Param);
        DataTable GetOrderByOrderID(string OrderID);
        void CreateOrder(string OrderNo, int PeoperNum, string UserIds, int PeoperMoney, int LowestMoney, int TouUserid, int MoneySendType, int MeetType, int MeetNum, int Meetextnum, DateTime FirstDate, DateTime FirstExtraDate, string ExtraDate, string Address, string Remark, out Dictionary<string, object> list);
        DataTable OrderLists(string No);
        DataTable OrderListUser(string OrderListID);
        DataTable UpdateOrderListState(string OrderNo, string OrderListID);
        int UpdateOrderListUserMoney(string OrderNo, string OrderListID, int Userid, int Money);
        DataTable GetOrderListUserPrvMoney(int Userid, string OrderNo, string OrderListID);
        /// <summary>
        /// 验证当前用户是否有权限填写标金
        /// </summary>
        /// <param name="Userid">用户ID</param>
        /// <param name="OrderNo">互助单号</param>
        /// <param name="OrderListID">互助单记录号</param>
        /// <returns></returns>
        int VerUserUpdateMoney(int Userid, string OrderNo, string OrderListID);
        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        int DelOrder(string OrderNo);
    }
}

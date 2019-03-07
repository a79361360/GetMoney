using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Application
{
    public interface IOrderBll
    {
        void AddOrder(OrderDto dto);
        bool RemoveOrders(string[] ids);
        bool RemoveOrder(string id);
        IList<OrderDto> ListOrderPage(ref int Total, int pageSize, int pageIndex);
        IList<OrderDto> ListOrderPage(ref int Total, int pageSize, int pageIndex, string filter);
        OrderDto GetOrderByOrderID(string OrderID);
        int CreateOrder(OrderDto dto);
        string ListToString(IList<UListDto> list);
        IList<OrderListDto> OrderLists(string No);
        IList<OrderListUserDto> OrderListUser(string OrderListID);
        OrderListDto UpdateOrderListState(string OrderNo, string OrderListID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="OrderListID"></param>
        /// <param name="Userid"></param>
        /// <param name="Money"></param>
        /// <returns></returns>
        int UpdateOrderListUserMoney(string OrderNo, string OrderListID, int Userid, int Money);
        /// <summary>
        /// 取得当前用户当前期上一次填写标金金额
        /// </summary>
        /// <param name="Userid">用户ID</param>
        /// <param name="OrderListID">互助单记录ID</param>
        /// <returns></returns>
        OrderListUserDto GetOrderListUserPrvMoney(int Userid, string OrderNo, string OrderListID);
        /// <summary>
        /// 验证当前用户是否有权限填写标金
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="OrderNo"></param>
        /// <param name="OrderListID"></param>
        /// <returns></returns>
        int VerUserUpdateMoney(int Userid, string OrderNo, string OrderListID);
        /// <summary>
        /// 删除Order单
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        int DelOrder(string OrderNo);
        /// <summary>
        /// 今天开标的用户信息
        /// </summary>
        /// <returns></returns>
        IList<OrderListUserDto> FindCurOrderList();
        /// <summary>
        /// 当前期前7与后7天的会款信息
        /// </summary>
        /// <param name="orderno"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        IList<OrderListUserDto> FindListOrder(string orderno, int userid);
        /// <summary>
        /// 会头查看前7与后7天所有待缴会脚的金额
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        IList<OrderListUserDto> FindListOrder(string orderno);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="touuid"></param>
        /// <param name="filetype"></param>
        /// <returns></returns>
        int CreateOrderByImport(int touuid, int filetype);
    }
}

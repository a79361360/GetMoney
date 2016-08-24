using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GetMoney.Application.Order;

namespace GetMoney.Application.Order
{
    public interface IOrderBll
    {
        void AddOrder(OrderDto dto);
        bool RemoveOrders(string[] ids);
        bool RemoveOrder(string id);
        IList<OrderDto> ListOrderPage(ref int Total, int pageSize, int pageIndex);
    }
}

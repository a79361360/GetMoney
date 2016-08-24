using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GetMoney.Data.Order;

namespace GetMoney.Dal
{
    public class OrderRepository
   : Repository<Order>, IOrderRepository
    {
        public OrderRepository(DBContextOfUnitWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}

using GetMoney.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GetMoney.Dal
{
    public class OrderDal : IOrderDal
    {
        public System.Data.DataTable ListOrderPage(ref int Total, SqlPageParam Param)
        {
            DataSet ds = SqlHelper.PageResult(SqlHelper.SQLConnString, Param.TableName, Param.PrimaryKey, Param.Fields, Param.PageSize, Param.PageIndex, Param.Filter, Param.Group, Param.Order, ref Total);
            return ds.Tables[0];
        }
    }
}

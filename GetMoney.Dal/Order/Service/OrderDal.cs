using GetMoney.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GetMoney.Dal
{
    public class OrderDal : IOrderDal
    {
        SqlDal dal = new SqlDal();
        public DataTable ListOrderPage(ref int Total, SqlPageParam Param)
        {
            //DataSet ds = SqlHelper.PageResult(SqlHelper.SQLConnString, Param.TableName, Param.PrimaryKey, Param.Fields, Param.PageSize, Param.PageIndex, Param.Filter, Param.Group, Param.Order, ref Total);
            //return ds.Tables[0];
            DataTable dt = dal.PageResult(Param.TableName, Param.PrimaryKey, Param.Fields, Param.PageSize, Param.PageIndex, Param.Filter, Param.Group, Param.Order, ref Total);
            return dt;
        }
        public void CreateOrder(string OrderNo, int PeoperNum, string UserIds, int PeoperMoney, int MoneySendType, int MeetType, int MeetNum, DateTime FirstDate, string MeetDate, string MeetTime, out Dictionary<string, object> list)
        {
            string ProName = "SP_AddNewOrder";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@OrderNo",SqlDbType.NVarChar,50),
                new SqlParameter("@PeoperNum",SqlDbType.Int),
                new SqlParameter("@Peoper",SqlDbType.NVarChar,500),
                new SqlParameter("@PeoperMoney",SqlDbType.Int),
                new SqlParameter("@MoneySendType",SqlDbType.Int),
                new SqlParameter("@MeetType",SqlDbType.Int),
                new SqlParameter("@MeetNum",SqlDbType.Int),
                new SqlParameter("@FirstDate",SqlDbType.DateTime),
                new SqlParameter("@MeetDate",SqlDbType.NVarChar,14),
                new SqlParameter("@MeetTime",SqlDbType.NVarChar,30),
                new SqlParameter("@ReturnValue",SqlDbType.Int)
            };
            parameter[0].Value = OrderNo;
            parameter[1].Value = PeoperNum;
            parameter[2].Value = UserIds;
            parameter[3].Value = PeoperMoney;
            parameter[4].Value = MoneySendType;
            parameter[5].Value = MeetType;
            parameter[6].Value = MeetNum;
            parameter[7].Value = FirstDate;
            parameter[8].Value = MeetDate;
            parameter[9].Value = MeetTime;
            parameter[10].Direction = ParameterDirection.ReturnValue;
            string[] str = new string[] { "@ReturnValue" };
            dal.ExtProc(ProName, parameter, str, out list);
        }
        public DataTable OrderLists(string No) {
            string sql = "select a.ID,a.OrderNo,convert(varchar(10),a.MeetDate) MeetDate,a.Userid,b.TrueName,a.AccrualMoney,a.State from Order_Lists a left outer join TUsers b on a.Userid=b.id where a.OrderNo=@No";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@No",SqlDbType.NVarChar,50)
            };
            parameter[0].Value = No;
            return dal.ExtSql(sql, parameter);
        }
        public DataTable OrderListUser(string OrderListID) {
            string sql = "SP_PicthOrderList";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@CZType",SqlDbType.Int),
                new SqlParameter("@ListID",SqlDbType.NVarChar,50)
            };
            parameter[0].Value = 2;
            parameter[1].Value = OrderListID;
            return dal.ExtProc(sql, parameter);
        }
        public DataTable UpdateOrderListState(string OrderNo, string OrderListID)
        {
            string sql = "SP_PicthOrderList";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@CZType",SqlDbType.Int),
                new SqlParameter("@OrderID",SqlDbType.VarChar,50),
                new SqlParameter("@ListID",SqlDbType.Int),
            };
            parameter[0].Value = 3;
            parameter[1].Value = OrderNo;
            parameter[2].Value = OrderListID;
            return dal.ExtProc(sql, parameter);
        }
        public int UpdateOrderListUserMoney(string OrderNo, string OrderListID, int Userid, int Money)
        {
            string sql = "SP_PicthOrderList";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@CZType",SqlDbType.Int),
                new SqlParameter("@OrderID",SqlDbType.VarChar,50),
                new SqlParameter("@ListID",SqlDbType.Int),
                new SqlParameter("@Userid",SqlDbType.Int),
                new SqlParameter("@Money",SqlDbType.Int),
                new SqlParameter("@ReturnValue",SqlDbType.Int)
            };
            parameter[0].Value = 1;
            parameter[1].Value = OrderNo;
            parameter[2].Value = OrderListID;
            parameter[3].Value = Userid;
            parameter[4].Value = Money;
            parameter[5].Direction = ParameterDirection.ReturnValue;
            string[] str = new string[] { "@ReturnValue" };
            Dictionary<string, object> list = new Dictionary<string, object>();
            dal.ExtProc(sql, parameter, str, out list);
            return Convert.ToInt32(list["@ReturnValue"]);
        }
        public DataTable GetOrderListUserPrvMoney(int Userid, string OrderListID)
        {
            string sql = "select Userid,AccrualMoney,Convert(varchar(100),Lastdate,21) as Lastdate from Order_ListUsers where Userid=@Userid and OrderListID=@ListID";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Userid",SqlDbType.Int),
                new SqlParameter("@ListID",SqlDbType.VarChar,50)
            };
            parameter[0].Value = Userid;
            parameter[1].Value = OrderListID;
            return dal.ExtSql(sql, parameter);
        }
    }
}

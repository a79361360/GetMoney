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
            string sql = "select a.id,a.OrderNo,a.OrderListID,a.Userid,b.TrueName,a.AccrualMoney,a.Addtime,a.Lastdate from Order_ListUsers a left outer join TUsers b on a.Userid=b.id where a.OrderListID=@ListID";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@ListID",SqlDbType.NVarChar,50)
            };
            parameter[0].Value = OrderListID;
            return dal.ExtSql(sql, parameter);
        }
    }
}

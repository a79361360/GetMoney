﻿using GetMoney.Data;
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
        public DataTable GetOrderByOrderID(string OrderID) {
            string sql = "select a.[id],a.[OrderNo],a.[PeoperNum],a.[PeoperMoney],a.[MoneySendType],a.[MeetType],a.[MeetNum],a.[FirstDate],a.[InputDate],a.[Address],a.[State],a.[Remark],a.[LowestMoney],a.[TouUserid],a.[FirstExtraDate],a.[ExtraDate],b.TrueName TouTrueName,b.Phone from Orders a inner join TUsers b on a.TouUserid=b.id Where a.OrderNo=@No";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@No",SqlDbType.NVarChar,50)
            };
            parameter[0].Value = OrderID;
            return dal.ExtSql(sql, parameter);
        }
        public void CreateOrder(string OrderNo, int PeoperNum, string UserIds, int PeoperMoney, int LowestMoney, int TouUserid, int MoneySendType, int MeetType, int MeetNum, int Meetextnum, DateTime FirstDate, DateTime FirstExtraDate, string ExtraDate,string Address, string Remark, out Dictionary<string, object> list)
        {
            string ProName = "SP_AddNewOrder";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@OrderNo",SqlDbType.NVarChar,50),
                new SqlParameter("@PeoperNum",SqlDbType.Int),
                new SqlParameter("@Peoper",SqlDbType.NVarChar,500),
                new SqlParameter("@PeoperMoney",SqlDbType.Int),
                new SqlParameter("@LowestMoney",SqlDbType.Int),
                new SqlParameter("@TouUserid",SqlDbType.Int),
                new SqlParameter("@MoneySendType",SqlDbType.Int),
                new SqlParameter("@MeetType",SqlDbType.Int),
                new SqlParameter("@MeetNum",SqlDbType.Int),
                new SqlParameter("@MeetExtNum",SqlDbType.Int),
                new SqlParameter("@FirstDate",SqlDbType.DateTime),
                new SqlParameter("@FirstExtraDate",SqlDbType.DateTime),
                new SqlParameter("@ExtraDate",SqlDbType.NVarChar,1000),
                new SqlParameter("@Address",SqlDbType.NVarChar,250),
                new SqlParameter("@Remark",SqlDbType.NVarChar,1000),
                new SqlParameter("@ReturnValue",SqlDbType.Int)
            };
            parameter[0].Value = OrderNo;
            parameter[1].Value = PeoperNum;
            parameter[2].Value = UserIds;
            parameter[3].Value = PeoperMoney;
            parameter[4].Value = LowestMoney;
            parameter[5].Value = TouUserid;
            parameter[6].Value = MoneySendType;
            parameter[7].Value = MeetType;
            parameter[8].Value = MeetNum;
            parameter[9].Value = Meetextnum;
            parameter[10].Value = FirstDate;
            parameter[11].Value = FirstExtraDate;
            parameter[12].Value = ExtraDate;
            parameter[13].Value = Address;
            parameter[14].Value = Remark;
            parameter[15].Direction = ParameterDirection.ReturnValue;
            string[] str = new string[] { "@ReturnValue" };
            dal.ExtProc(ProName, parameter, str, out list);
        }
        public DataTable OrderLists(string No) {
            string sql = "select a.ID,a.OrderNo,convert(varchar(20),a.MeetDate,120) MeetDate,a.Userid,b.TrueName,a.AccrualMoney,a.State,";
            sql += "(SELECT (PeoperNum*PeoperMoney)+(SELECT ISNULL(SUM(AccrualMoney),0) FROM Order_Lists WHERE OrderNo=a.OrderNo AND MeetDate < a.MeetDate AND State=1) FROM Orders WHERE OrderNo=a.OrderNo) AllMoney";  //实收全款=会费+利息
            sql += " from Order_Lists a left outer join TUsers b on a.Userid=b.id where a.OrderNo=@No Order by MeetDate";
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
        public int VerUserUpdateMoney(int Userid,string OrderNo, string OrderListID)
        {
            //1为允许，-1为不允许
            string sql = "SP_PicthOrderList";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@CZType",SqlDbType.Int),
                new SqlParameter("@OrderID",SqlDbType.VarChar,50),
                new SqlParameter("@ListID",SqlDbType.Int),
                new SqlParameter("@Userid",SqlDbType.Int),
            };
            parameter[0].Value = 4;
            parameter[1].Value = OrderNo;
            parameter[2].Value = OrderListID;
            parameter[3].Value = Userid;
            DataTable dt = dal.ExtProc(sql, parameter);
            int result = -1;
            if (dt.Rows.Count > 0) {
                result = Convert.ToInt32(dt.Rows[0]["result"]);
            }
            return result;
        }
        public DataTable GetOrderListUserPrvMoney(int Userid, string OrderNo, string OrderListID)
        {
            string sql = "select Userid,AccrualMoney,Convert(varchar(100),Lastdate,21) as Lastdate from Order_ListUsers where OrderNo=@OrderNo and OrderListID=@ListID and Userid=@Userid";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Userid",SqlDbType.Int),
                new SqlParameter("@OrderNo",SqlDbType.VarChar,50),
                new SqlParameter("@ListID",SqlDbType.VarChar,50)
            };
            parameter[0].Value = Userid;
            parameter[1].Value = OrderNo;
            parameter[2].Value = OrderListID;
            return dal.ExtSql(sql, parameter);
        }
        public int DelOrder(string OrderNo) {
            string sql = "SP_PicthOrderList";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@CZType",SqlDbType.Int),
                new SqlParameter("@OrderID",SqlDbType.VarChar,50)
            };
            parameter[0].Value = 5;
            parameter[1].Value = OrderNo;
            DataTable dt = dal.ExtProc(sql, parameter);
            int result = -1;
            if (dt.Rows.Count > 0)
            {
                result = Convert.ToInt32(dt.Rows[0]["result"]);
            }
            return result;
        }
        /// <summary>
        /// 今天开标的用户信息
        /// </summary>
        /// <returns></returns>
        public DataTable FindCurOrderList() {
            string sql = "SELECT CONVERT(NVARCHAR(19),a.MeetDate,120) Lastdate,SUM(b.StayPayNum) StayPayNum,b.StayPayTax,b.Userid,(SELECT PeoperNum*PeoperMoney FROM Orders WHERE OrderNo=a.OrderNo) AccrualMoney FROM Order_Lists a INNER JOIN Order_ListUsers b ON a.id=b.OrderListID WHERE a.id IN(SELECT id FROM Order_Lists WHERE DATEDIFF(DAY,MeetDate,GETDATE()) IN(-2,-1,0)) GROUP BY a.MeetDate,b.StayPayTax,b.Userid,a.OrderNo";
            var dt = dal.ExtSql(sql);
            return dt;
        }
        /// <summary>
        /// 当前这一期会脚userid需要转换的金额
        /// </summary>
        /// <param name="orderno"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable FindListOrder(string orderno,int userid) {
            string sql = "SELECT TOP 1 CONVERT(NVARCHAR(19),a.MeetDate,120) Lastdate,SUM(b.StayPayNum) StayPayNum,b.StayPayTax,b.Userid,a.OrderNo,";
            sql += "(SELECT PeoperNum * PeoperMoney FROM Orders WHERE OrderNo = a.OrderNo) AccrualMoney";
            sql += " FROM Order_Lists a INNER JOIN Order_ListUsers b ON a.id = b.OrderListID";
            sql += " WHERE a.id IN(SELECT id FROM Order_Lists WHERE - 7 < DATEDIFF(DAY, MeetDate, GETDATE()) AND DATEDIFF(DAY, MeetDate, GETDATE()) < 7) AND b.Userid = @Userid AND a.OrderNo = @OrderNo";
            sql += " GROUP BY a.MeetDate,b.StayPayTax,b.Userid,a.OrderNo";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Userid",SqlDbType.Int),
                new SqlParameter("@OrderNo",SqlDbType.VarChar,50)
            };
            parameter[0].Value = userid;
            parameter[1].Value = orderno;
            var dt = dal.ExtSql(sql, parameter);
            return dt;
        }
        /// <summary>
        /// 会头查看当前这一期所有待缴会脚的金额
        /// </summary>
        /// <param name="orderno"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable FindListOrder(string orderno)
        {
            string sql = "SELECT CONVERT(NVARCHAR(19),a.MeetDate,120) Lastdate,SUM(b.StayPayNum) StayPayNum,b.StayPayTax,b.Userid,a.OrderNo,c.NickName TrueName,";
            sql += "(SELECT PeoperNum * PeoperMoney FROM Orders WHERE OrderNo = a.OrderNo) AccrualMoney";
            sql += " FROM Order_Lists a INNER JOIN Order_ListUsers b ON a.id = b.OrderListID";
            sql += " RIGHT JOIN TUsers c ON b.Userid=c.id";
            sql += " WHERE a.id IN(SELECT id FROM Order_Lists WHERE - 7 < DATEDIFF(DAY, MeetDate, GETDATE()) AND DATEDIFF(DAY, MeetDate, GETDATE()) < 7) AND a.OrderNo = @OrderNo";
            sql += " GROUP BY a.MeetDate,b.StayPayTax,b.Userid,a.OrderNo,c.NickName";
            sql += " Order BY b.Userid";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@OrderNo",SqlDbType.VarChar,50)
            };
            parameter[0].Value = orderno;
            var dt = dal.ExtSql(sql, parameter);
            return dt;
        }
    }
}

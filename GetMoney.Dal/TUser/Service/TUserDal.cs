using GetMoney.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GetMoney.Dal
{
    public class TUserDal : ITUserDal
    {
        SqlDal dal = new SqlDal();
        public DataTable ListUserPage(ref int Total, SqlPageParam Param)
        {
            DataSet ds = SqlHelper.PageResult(SqlHelper.SQLConnString, Param.TableName, Param.PrimaryKey, Param.Fields, Param.PageSize, Param.PageIndex, Param.Filter, Param.Group, Param.Order, ref Total);
            return ds.Tables[0];
        }


        public void AddTUserByProce(string UserName, string Pwd, string BankPwd,string NickName,string TrueName,string IdentityNum,string Phone, string RegIp,string TxUrl, out Dictionary<string, object> list)
        {
            string ProName="SP_AddNewUser";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@UserName",SqlDbType.NVarChar,20),
                new SqlParameter("@Password",SqlDbType.NVarChar,50),
                new SqlParameter("@BankPwd",SqlDbType.NVarChar,50),
                new SqlParameter("@NickName",SqlDbType.NVarChar,20),
                new SqlParameter("@TrueName",SqlDbType.NVarChar,20),
                new SqlParameter("@IdentityNum",SqlDbType.NVarChar,20),
                new SqlParameter("@Phone",SqlDbType.NVarChar,12),
                new SqlParameter("@RegIP",SqlDbType.NVarChar,16),
                new SqlParameter("@TxUrl",SqlDbType.NVarChar,100),
                new SqlParameter("@Userid",SqlDbType.Int),
                new SqlParameter("@ReturnValue",SqlDbType.Int)
            };
            parameter[0].Value = UserName;
            parameter[1].Value = Pwd;
            parameter[2].Value = BankPwd;
            parameter[3].Value = NickName;
            parameter[4].Value = TrueName;
            parameter[5].Value = IdentityNum;
            parameter[6].Value = Phone;
            parameter[7].Value = RegIp;
            parameter[8].Value = TxUrl;
            parameter[9].Direction = ParameterDirection.Output;
            parameter[10].Direction = ParameterDirection.ReturnValue;
            string[] str = new string[] { "@Userid", "@ReturnValue" };
            dal.ExtProc(ProName, parameter, str, out list);
        }

        public int AddTUserFriend(int userid,int pcid)
        {
            string sql = "insert into TUserFriends(Userid,Pcid)values(" + userid + "," + pcid + ")";
            int result = dal.IntExtSql(sql);
            return result;
        }
        /// <summary>
        /// 根据UserName和UserPwd判断用户是否存在
        /// </summary>
        /// <param name="UserName">用户账号</param>
        /// <param name="UserPwd">用户密码</param>
        /// <returns>存在true不存在false</returns>
        public bool VerifyUserByUnamePwd(string UserName,string UserPwd) {
            string sql = "select id from TUsers where UserName=@username and UserPwd=@userpwd";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@username",SqlDbType.NVarChar,20),
                new SqlParameter("@userpwd",SqlDbType.NVarChar,50)
            };
            parameter[0].Value = UserName;
            parameter[1].Value = UserPwd;
            DataTable dt = dal.ExtSql(sql, parameter);
            if (dt.Rows.Count > 0) {
                return true;
            }
            return false;
        }
    }
}

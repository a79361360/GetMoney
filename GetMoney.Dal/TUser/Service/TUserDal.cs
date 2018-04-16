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
            //DataSet ds = SqlHelper.PageResult(SqlHelper.SQLConnString, Param.TableName, Param.PrimaryKey, Param.Fields, Param.PageSize, Param.PageIndex, Param.Filter, Param.Group, Param.Order, ref Total);
            //return ds.Tables[0];
            DataTable dt = dal.PageResult(Param.TableName, Param.PrimaryKey, Param.Fields, Param.PageSize, Param.PageIndex, Param.Filter, Param.Group, Param.Order, ref Total);
            return dt;
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="UserName">用户账号</param>
        /// <param name="Pwd">密码</param>
        /// <param name="BankPwd">银行密码</param>
        /// <param name="NickName">呢称</param>
        /// <param name="TrueName">真实姓名</param>
        /// <param name="IdentityNum">身份证号码</param>
        /// <param name="Phone">手机号码</param>
        /// <param name="RegIp">注册IP</param>
        /// <param name="TxUrl">头像URI</param>
        /// <param name="list">注册结果返回</param>
        public void AddTUserByProce(string UserName, string Pwd, string BankPwd,string NickName,string TrueName,string IdentityNum,string Phone, string RegIp,string TxUrl, out Dictionary<string, object> list)
        {
            string ProName="SP_AddNewUser";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@UserName",SqlDbType.NVarChar,50),
                new SqlParameter("@Password",SqlDbType.NVarChar,50),
                new SqlParameter("@BankPwd",SqlDbType.NVarChar,50),
                new SqlParameter("@NickName",SqlDbType.NVarChar,20),
                new SqlParameter("@TrueName",SqlDbType.NVarChar,20),
                new SqlParameter("@IdentityNum",SqlDbType.NVarChar,20),
                new SqlParameter("@Phone",SqlDbType.NVarChar,12),
                new SqlParameter("@RegIP",SqlDbType.NVarChar,16),
                new SqlParameter("@TxUrl",SqlDbType.NVarChar,255),
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
        public bool EditTUser(int id, string truename, string identitynum, string phone,string BankNumber,int binid) {
            string sql = "Update TUsers set TrueName=@TrueName,IdentityNum=@IdentityNum,Phone=@Phone,BankNumber=@BankNumber,Bankbinid=@binid where id=@id";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@TrueName",SqlDbType.NVarChar,50),
                new SqlParameter("@IdentityNum",SqlDbType.NVarChar,20),
                new SqlParameter("@Phone",SqlDbType.NVarChar,16),
                new SqlParameter("@BankNumber",SqlDbType.NVarChar,50),
                new SqlParameter("@binid",SqlDbType.Int),
                new SqlParameter("@id",SqlDbType.Int)
            };
            parameter[0].Value = truename;
            parameter[1].Value = identitynum;
            parameter[2].Value = phone;
            parameter[3].Value = BankNumber;
            parameter[4].Value = binid;
            parameter[5].Value = id;
            int result = dal.IntExtSql(sql, parameter);
            if (result > 0) return true;
            else return false;
        }
        /// <summary>
        /// 好友操作,添加好友,拉黑,删除好友.
        /// </summary>
        /// <param name="userid">当前登入的用户ID</param>
        /// <param name="pcid">好友用户ID</param>
        /// <param name="type">操作类型1添加好友,2黑名单,3删除好友</param>
        /// <returns>通过list外围获得操作结果,成功为1</returns>
        public void MakeTUserFriend(int userid, int pcid, int type, out Dictionary<string, object> list)
        {
            string ProName = "SP_FriendUser";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Userid",SqlDbType.Int),
                new SqlParameter("@Pcid",SqlDbType.Int),
                new SqlParameter("@Type",SqlDbType.Int),
                new SqlParameter("@ReturnValue",SqlDbType.Int)
            };
            parameter[0].Value = userid;
            parameter[1].Value = pcid;
            parameter[2].Value = type;
            parameter[3].Direction = ParameterDirection.ReturnValue;
            string[] str = new string[] { "@ReturnValue" };
            dal.ExtProc(ProName, parameter, str, out list);
        }
        /// <summary>
        /// 根据UserName和UserPwd判断用户是否存在,存在返回-用户的ID,不存在返回-1
        /// </summary>
        /// <param name="UserName">用户账号</param>
        /// <param name="UserPwd">用户密码</param>
        /// <returns>存在用户的ID不存在-1</returns>
        public int VerifyUserByUnamePwd(string UserName,string UserPwd) {
            string sql = "select id from TUsers where UserName=@username and UserPwd=@userpwd";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@username",SqlDbType.NVarChar,50),
                new SqlParameter("@userpwd",SqlDbType.NVarChar,50)
            };
            parameter[0].Value = UserName;
            parameter[1].Value = UserPwd;
            DataTable dt = dal.ExtSql(sql, parameter);
            if (dt.Rows.Count > 0) {
                return Convert.ToInt32(dt.Rows[0]["id"]);
            }
            return -1;
        }
        /// <summary>
        /// 判断当前用户ID是否存在
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int VerfyUserById(int userid) {
            string sql = "select top 1 id from TUsers where id=@id";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@id",SqlDbType.Int)
            };
            parameter[0].Value = userid;
            var result = dal.ExtScalarSql(sql, parameter);
            if (result == null) return -1;
            return Convert.ToInt32(result);
        }
        /// <summary>
        /// 根据UserName取得用户ID
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public int VerifyUserName(string UserName) {
            string sql = "select id from TUsers where UserName=@username";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@username",SqlDbType.NVarChar,50)
            };
            parameter[0].Value = UserName;
            DataTable dt = dal.ExtSql(sql, parameter);
            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["id"]);
            }
            return -1;
        }
        /// <summary>
        /// 根据UserName取得用户DTO
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public DataTable FindUserByUserName(string UserName) {
            string sql = "SELECT [id],[UserName],[UserPwd],[BankPwd],[NickName],[UserJb],[TrueName],[IdentityNum],[Phone],[RegIP],[TxUrl],[State],[Addtime] FROM [TUsers] where UserName=@username";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@username",SqlDbType.NVarChar,50)
            };
            parameter[0].Value = UserName;
            DataTable dt = dal.ExtSql(sql, parameter);
            return dt;
        }
        /// <summary>
        /// 通过ID查询返回TUser对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable FindUserById(int id)
        {
            string sql = "SELECT id,UserName,UserPwd,BankPwd,NickName,UserJb,TrueName,BankNumber,Bankbinid,IdentityNum,Phone,RegIP,TxUrl,State,Addtime,bankName,bankNameEn FROM V_TUsers where id=@id";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@id",SqlDbType.Int)
            };
            parameter[0].Value = id;
            DataTable dt = dal.ExtSql(sql, parameter);
            return dt;
        }
        
        public void UpdateUserTx(int userid, string txurl, out Dictionary<string, object> list)
        {
            string ProName = "SP_UpdateUserTx";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Userid",SqlDbType.Int),
                new SqlParameter("@TxUrl",SqlDbType.NVarChar,255),
                new SqlParameter("@ReturnValue",SqlDbType.Int)
            };
            parameter[0].Value = userid;
            parameter[1].Value = txurl;
            parameter[2].Direction = ParameterDirection.ReturnValue;
            string[] str = new string[] { "@ReturnValue" };
            dal.ExtProc(ProName, parameter, str, out list);
        }
        public DataTable BankBin(long bin,int binlen) {
            string sql = "SELECT id,bankName,bankNameEn,cardName,cardType,bin,binLength,issueid FROM TBankBin WHERE bin=@bin AND binLength=@len";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@bin",SqlDbType.BigInt),
                new SqlParameter("@len",SqlDbType.Int),
            };
            parameter[0].Value = bin;
            parameter[1].Value = binlen;
            DataTable dt = dal.ExtSql(sql, parameter);
            return dt;
        }
    }
}

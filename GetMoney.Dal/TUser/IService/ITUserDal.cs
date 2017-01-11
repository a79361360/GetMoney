using GetMoney.Data.TUser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GetMoney.Dal
{
    public interface ITUserDal
    {
        DataTable ListUserPage(ref int Total, SqlPageParam Param);
        void AddTUserByProce(string UserName, string Pwd, string BankPwd, string NickName, string TrueName, string IdentityNum, string Phone, string RegIp, string TxUrl, out Dictionary<string, object> list);
        void MakeTUserFriend(int userid, int pcid, int type, out Dictionary<string, object> list);
        /// <summary>
        /// 根据UserName和UserPwd判断用户是否存在,存在返回-用户的ID,不存在返回-1
        /// </summary>
        /// <param name="UserName">用户账号</param>
        /// <param name="UserPwd">用户密码</param>
        /// <returns>存在用户的ID不存在-1</returns>
        int VerifyUserByUnamePwd(string UserName, string UserPwd);
    }
}

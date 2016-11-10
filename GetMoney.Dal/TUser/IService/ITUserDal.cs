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
        int VerifyUserByUnamePwd(string UserName, string UserPwd);
    }
}

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
        void AddTUserByProce(string UserName, string Pwd, string BankPwd, string NickName, string TrueName, string IdentityNum, string Phone, string RegIp, string TxUrl, out Dictionary<string, object> list);
        /// <summary>
        /// 好友操作,添加好友,拉黑,删除好友.(用户ID,好友ID,返回结果1为成功)
        /// </summary>
        /// <param name="userid">当前登入的用户ID</param>
        /// <param name="pcid">好友用户ID</param>
        /// <param name="type">操作类型1添加好友,2黑名单,3删除好友</param>
        /// <returns>通过list外围获得操作结果,成功为1</returns>
        void MakeTUserFriend(int userid, int pcid, int type, out Dictionary<string, object> list);
        /// <summary>
        /// 根据UserName和UserPwd判断用户是否存在,存在返回-用户的ID,不存在返回-1
        /// </summary>
        /// <param name="UserName">用户账号</param>
        /// <param name="UserPwd">用户密码</param>
        /// <returns>存在用户的ID不存在-1</returns>
        int VerifyUserByUnamePwd(string UserName, string UserPwd);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        int VerfyUserById(int userid);
        /// <summary>
        /// 根据UserName取得用户ID
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        int VerifyUserName(string UserName);
        /// <summary>
        /// 根据UserName取得用户DTO
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        DataTable FindUserByUserName(string UserName);
        /// <summary>
        /// 根据ID取得用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataTable FindUserById(int id);
        bool EditTUser(int id, string truename, string identitynum, string phone, string BankNumber, int binid);
        /// <summary>
        /// 将已经上传的头像更新到数据库
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="txurl">用户头像地址，虚拟路径完整路径传什么保存什么</param>
        /// <param name="list">数据库返回的数据,9用户不存在,1000执行失败回滚,1成功</param>
        void UpdateUserTx(int userid, string txurl, out Dictionary<string, object> list);
        /// <summary>
        /// 取得银行信息
        /// </summary>
        /// <param name="bin"></param>
        /// <param name="binlen"></param>
        /// <returns></returns>
        DataTable BankBin(long bin, int binlen);
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        int ExtTUserDayOnly(int userid, int type);
        /// <summary>
        /// 设置今天只允许一次
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        int SetTUserDayOnly(int userid, int type);
    }
}

using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GetMoney.Application
{
    public interface ITUserBll
    {
        /// <summary>
        /// 使用EF来进行用户信息添加,现在基本没有使用这个方法来进行注册用户
        /// </summary>
        /// <param name="dto"></param>
        void AddTUser(TUserDto dto);
        bool EditTUser(TUserDto dto);
        /// <summary>
        /// 注册用户(用户信息对象,返回结果)
        /// </summary>
        /// <param name="dto">用户信息对象</param>
        /// <param name="list">返回结果</param>
        void RegTUser(TUserDto dto, out Dictionary<string, object> list);
        /// <summary>
        /// 批量移出用户信息(用户ID列表)
        /// </summary>
        /// <param name="ids">用户ID列表</param>
        /// <returns></returns>
        bool RemoveTUsers(IList<UListDto> ids);
        /// <summary>
        /// 单个移除用户信息(用户ID)
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        bool RemoveTUser(string id);
        /// <summary>
        /// 取得翻页用户列表,带查询条件(总条数,每页大小,翻页索引,过滤条件)
        /// </summary>
        /// <param name="Total">总条数</param>
        /// <param name="pageSize">一页大小</param>
        /// <param name="pageIndex">当前页索引,首页为0</param>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        IList<TUserDto> ListTUserPage(ref int Total, int pageSize, int pageIndex, string filter);
        /// <summary>
        /// 翻页查询当前用户好友(总条数,每页大小,翻页索引,过滤条件)
        /// </summary>
        /// <param name="Total">总条数</param>
        /// <param name="pageSize">一页大小</param>
        /// <param name="pageIndex">当前页索引,首页为0</param>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        IList<FriendDto> ListFriendPage(ref int Total, int pageSize, int pageIndex, string filter);
        /// <summary>
        /// 添加好友,(用户ID,好友id列表)
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="list">好友列表对象</param>
        /// <returns>完成好友添加数量</returns>
        int AddTUserFriend(int userid, IList<UListDto> list);
        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        int AddTUserFriend(int pid, int userid);
        /// <summary>
        /// 验证用户(用户对象)
        /// </summary>
        /// <param name="dto">用户对象</param>
        /// <returns></returns>
        int VerifyTUsers(TUserDto dto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        int VerfyUserById(int userid);
        /// <summary>
        /// 用户名判断是否已经存在
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        int VerifyTUserByUName(TUserDto dto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        TUserDto FindUserByUserName(string UserName);
        /// <summary>
        /// 根据ID返回TUser对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TUserDto FindUserById(int id);
        /// <summary>
        /// 更新玩家的头像
        /// </summary>
        /// <param name="host">主机IP跟端口,外网的域名</param>
        /// <param name="userid">用户ID</param>
        /// <returns>完整的上传地址</returns>
        string UpdateFileTx(string host, string userid);
        string UpdateFileTx1(string host, string userid, int x, int y, int w, int h, Stream Img);
    }
}

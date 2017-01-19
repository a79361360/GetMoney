using GetMoney.Model;
using System;
using System.Collections.Generic;
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
        /// 验证用户(用户对象)
        /// </summary>
        /// <param name="dto">用户对象</param>
        /// <returns></returns>
        int VerifyTUsers(TUserDto dto);
    }
}

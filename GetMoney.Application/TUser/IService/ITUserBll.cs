using GetMoney.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Application
{
    public interface ITUserBll
    {
        void AddTUser(TUserDto dto);
        void RegTUser(TUserDto dto, out Dictionary<string, object> list);
        bool RemoveTUsers(string[] ids);
        bool RemoveTUser(string id);
        /// <summary>
        /// 取得翻页用户列表,不带查询条件
        /// </summary>
        /// <param name="Total"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IList<TUserDto> ListTUserPage(ref int Total, int pageSize, int pageIndex);
        /// <summary>
        /// 取得翻页用户列表,带查询条件
        /// </summary>
        /// <param name="Total"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IList<TUserDto> ListTUserPage(ref int Total, int pageSize, int pageIndex, string filter);
        /// <summary>
        /// 取得翻页我的好友列表,带查询条件
        /// </summary>
        /// <param name="Total"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IList<FriendDto> ListFriendPage(ref int Total, int pageSize, int pageIndex, string filter);
        int AddTUserFriend(int userid, List<UListDto> list);
    }
}

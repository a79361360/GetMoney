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
        bool RemoveTUsers(string[] ids);
        bool RemoveTUser(string id);
        IList<TUserDto> ListTUserPage(ref int Total, int pageSize, int pageIndex);
    }
}

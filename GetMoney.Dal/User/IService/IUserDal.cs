using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GetMoney.Dal.User
{
    interface IUserDal
    {
        DataTable ListUserPage(ref int Total, SqlPageParam Param);
    }
}

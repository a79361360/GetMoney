﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GetMoney.Dal
{
    public interface ITUserDal
    {
        DataTable ListUserPage(ref int Total, SqlPageParam Param);
    }
}
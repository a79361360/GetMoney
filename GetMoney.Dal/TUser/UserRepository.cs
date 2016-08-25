using GetMoney.Data.TUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Dal
{
    public class TUserRepository
   : Repository<TUser>, ITUserRepository
    {
        public TUserRepository(DBContextOfUnitWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}

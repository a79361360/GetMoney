using GetMoney.Data.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Dal
{
    public class UserRepository
   : Repository<User>, IUserRepository
    {
        public UserRepository(DBContextOfUnitWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}

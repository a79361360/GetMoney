using GetMoney.Data.OnlyNameTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Dal
{
    public class OnlyNameTestRepository
   : Repository<OnlyNameTest>, IOnlyNameTestRepository
    {
        public OnlyNameTestRepository(DBContextOfUnitWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}

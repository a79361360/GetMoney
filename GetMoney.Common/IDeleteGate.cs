using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Common
{
    public class IDeleteGate
    {
        public delegate bool Test(int type, int titleid, string imgurl);
    }
}

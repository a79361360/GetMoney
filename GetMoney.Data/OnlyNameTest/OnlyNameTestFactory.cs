using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Data.OnlyNameTest
{
    public class OnlyNameTestFactory
    {
        public static OnlyNameTest CreateOnly(string name, DateTime inputdate)
        {
            return new OnlyNameTest
            {
                Name = name,
                InputDate = inputdate
            };
        }
    }
}

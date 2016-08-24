using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetMoney.Data.OnlyNameTest
{
    public class OnlyNameTest : Entity
    {
        public new int id { get; set; }
        public string Name { get; set; }
        public DateTime InputDate { get; set; }

    }
}
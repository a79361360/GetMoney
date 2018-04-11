using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Model.Model.Nsoup
{
    public class Nsoup_bankDto
    {
        public int id { get; set; }
        public string bankName { get; set; }
        public string bankNameEn { get; set; }
        public string cardName { get; set; }
        public string cardType { get; set; }
        public long bin { get; set; }
        public int nLength { get; set; }
        public int binLength { get; set; }
        public long issueid { get; set; }
    }
}

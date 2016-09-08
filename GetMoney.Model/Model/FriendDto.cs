using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Model.Model
{
    public class FriendDto
    {
        public int Userid { get; set; }
        public int Pcid { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string TrueName { get; set; }
        public int UserJb { get; set; }
        public string IdentityNum { get; set; }
        public string Phone { get; set; }
        public string TxUrl { get; set; }
    }
}

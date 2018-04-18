using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Model
{
    public class TUOptionDTO
    {
        public int id { get; set; }
        public int userid { get; set; }
        public string title { get; set; }
        public int type { get; set; }
        public string nametype { get; set; }
        public string content { get; set; }
        public string rcontent { get; set; }
        public string addtime { get; set; }
    }
}

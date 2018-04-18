using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Model
{
    public class TNoticeDto
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ContentTxt { get; set; }
        public DateTime AddTime { get; set; }

    }
}

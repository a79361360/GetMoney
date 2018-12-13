using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Model.WxModel
{
    public class WxParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public WxParameter(string _name, string _value)
        {
            this.Name = _name;
            this.Value = _value;
        }
    }
}

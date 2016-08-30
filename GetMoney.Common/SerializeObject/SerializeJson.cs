using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Common.SerializeObject
{
    public static class SerializeJson<T>
    {
        public static List<T> JSONStringToList(string JsonStr)
        {
            List<T> objs = JsonConvert.DeserializeObject<List<T>>(JsonStr);
            return objs;
        } 
    }
}

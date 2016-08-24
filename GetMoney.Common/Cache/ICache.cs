using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Common.Cache
{
    public interface ICache
    {
        void Add(string key, object value);
        void Add(string key, object value, int? timeout);
        void Add(string key, object value, int? timeout,DateTime dt);
        void Update(string key, object value);
        void Delete(string key);
        void Delete();
        object GetValue(string key);
        object this[string key] { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Data
{
    public class Entity
    {
        int _id;
        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        #region override Methods
        /// <summary>
        /// 判断两个对象是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Entity left, Entity right)
        {
            if (object.Equals(left, null))
                return (object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }
        /// <summary>
        /// 判断两个对象是否不相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
        #endregion
    }
}

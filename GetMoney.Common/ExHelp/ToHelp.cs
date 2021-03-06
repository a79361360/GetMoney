﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 类型转换
    /// </summary>
    public static class ToHelp
    {
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static V ToAs<V>(this object value, V defaultV = default(V), bool err = false, Action<Exception> errAction = null)
        {
            var type = typeof(V);
            var val = value.ToAsV(type);
            var v = (V)val;
            return v;
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="toType"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static object ToAsV(this object value, Type toType, object defaultV = null, bool err = false, Action<Exception> errAction = null)
        {            
            //return result;
            object v = null;
            if (value != null)
            {
                var vstr = value.ToString();
                if (!string.IsNullOrEmpty(vstr))
                {
                    if (toType.IsGenericType)
                    {
                        toType = toType.GetGenericArguments()[0];
                    }
                    if (toType.Name == "Guid")
                    {
                        v = Convert.ChangeType(Guid.Parse(value.ToString()), toType);
                    }
                    else
                    {
                        v = Convert.ChangeType(value.ToString(), toType);
                    }
                }
            }
            if (v == null && toType.IsValueType)
            {
                var ary = Array.CreateInstance(toType, 1);
                v = ary.GetValue(0);
                //v = type.InvokeMember(null, BindingFlags.CreateInstance,
                //    null, null, null);
            }
            return v;
        }



        /// <summary>
        /// 实现数据的四舍五入法
        /// </summary>
        /// <param name="v">要进行处理的数据</param>
        /// <param name="x">保留的小数位数</param>
        /// <returns>四舍五入后的结果</returns>
        public static T Round<T>(this object v, int x)
        {
            return Round(v.ToAs<double>(), x).ToAs<T>();
        }


        // <summary>
        /// 实现数据的四舍五入法
        /// </summary>
        /// <param name="v">要进行处理的数据</param>
        /// <param name="x">保留的小数位数</param>
        /// <returns>四舍五入后的结果</returns>
        public static double Round(double d, int i)
        {
            var a = d;
            if (d >= 0)
            {
                d += 5 * Math.Pow(10, -(i + 1));
            }
            else
            {
                d += -5 * Math.Pow(10, -(i + 1));
            }
            string str = d.ToString();
            string[] strs = str.Split('.');
            int idot = str.IndexOf('.');
            string prestr = strs[0];
            string poststr = strs.Length > 1 ? strs[1] : "0";
            if (poststr.Length > i)
            {
                poststr = str.Substring(idot + 1, i);
            }
            string strd = prestr + "." + poststr;
            d = Double.Parse(strd);
            return d;
        }


    }
}

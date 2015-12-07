﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GetMoney.Framework.Common
{
    public static class Convertor
    {
        #region 数据格式转换
        /// <summary>
        ///  转换成Int
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static int ToInt(this object inputValue)
        {
            if (inputValue.IsInt())
            {
                return int.Parse(inputValue.ToStringValue());
            }
            return 0;
        }
        /// <summary>
        ///  转换成Int32
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static int ToInt32(this object inputValue)
        {
            if (inputValue.IsInt())
            {
                return Convert.ToInt32(inputValue);
            }
            return 0;
        }
        /// <summary>
        ///  转换成Int64
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static long ToInt64(this object inputValue)
        {
            if (inputValue.IsInt())
            {
                return Convert.ToInt64(inputValue);
            }
            return 0;
        }
        /// <summary>
        ///  转换成Double
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static Double ToDouble(this object inputValue)
        {
            if (!inputValue.IsNullOrEmpty())
            {
                return Convert.ToDouble(inputValue);
            }
            return 0;
        }
        /// <summary>
        ///  转换成Decimal
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static Decimal ToDecimal(this object inputValue)
        {
            if (!inputValue.IsNullOrEmpty())
            {
                return Convert.ToDecimal(inputValue);
            }
            return 0;
        }
        /// <summary>
        ///  转换成Int64
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static bool? ToBool(this object inputValue)
        {
            if (inputValue.IsNullOrEmpty())
            {
                return null;
            }
            return bool.Parse(inputValue.ToStringValue());
        }
        /// <summary>
        ///  转换obj 成string
        /// </summary>
        /// <param name="inputValue">object</param>
        /// <returns></returns>
        public static string ToStringValue(this object inputValue)
        {
            if (inputValue == null)
            {
                return "";
            }
            else
            {
                return inputValue.ToString();
            }
        }
        /// <summary>
        ///  转换成数据库字符串
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static string ToDBString(this object inputValue)
        {
            if (!inputValue.IsNullOrEmpty())
            {
                return ("'" + inputValue.ToStringValue().Trim().Replace("'", "''") + "'");
            }
            return "''";
        }
        /// <summary>
        ///  转换成时间类型yyyy-MM-dd
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object inputValue)
        {
            if (inputValue.IsNullOrEmpty())
            {
                return DateTime.MinValue;
            }
            return Convert.ToDateTime(inputValue);
        }
        /// <summary>
        ///  转换成时间类型
        /// </summary>
        /// <param name="inputValue">输入值</param>
        /// <param name="format">时间格式</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object inputValue, string format)
        {
            return DateTime.ParseExact(inputValue.ToStringValue(), format, null);
        }
        #endregion


    }
}

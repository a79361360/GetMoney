using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace GetMoney.Common
{
    /// <summary>
    /// Txt 操作类
    /// </summary>
    public sealed class Txt
    {
        internal Txt() { }

        /// <summary>
        /// 判断文件是否存在，若存在返回true，否则返回false并创建文件
        /// </summary>
        /// <returns></returns>
        public bool ExistFile(string filepath)
        {
            if (CommonManager.FileObj.ExistFile(filepath))
                return true;
            else
            {
                StreamWriter sw = new StreamWriter(CommonManager.FileObj.GetPhysicalPath(filepath), true, Encoding.UTF8);
                sw.Close();
                return false;
            }
        }

        /// <summary>
        /// 写日志，自动创建文件
        /// </summary>
        /// <param name="filePath">文件虚拟地址</param>
        /// <param name="log">内容</param>
        public void WriteLogs(string filePath, string log)
        {
            using (StreamWriter sw = new StreamWriter(CommonManager.FileObj.GetPhysicalPath(filePath), true, Encoding.UTF8))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + log);
            }
        }

        /// <summary>
        /// 写入txt文件
        /// </summary>
        /// <param name="Section">内容</param>
        /// <param name="append">是否追加文本，true为追加文本</param>
        public void WriteValue(string filepath,string Section, bool append)
        {
            StreamWriter sw = new StreamWriter(filepath, append, Encoding.UTF8);
            sw.WriteLine(Section);
            sw.Close();
        }

        /// <summary>
        /// 获取文本中某行内容
        /// </summary>
        /// <param name="rowid">行号</param>
        /// <returns></returns>
        public string GetSingleText(string filepath,int rowid)
        {
            if (ExistFile(filepath))
            {
                string temp = string.Empty;
                string[] arr = System.IO.File.ReadAllLines(CommonManager.FileObj.GetPhysicalPath(filepath), Encoding.UTF8);
                if (rowid <= arr.Length)
                {
                    temp = arr[rowid - 1];
                }
                return temp;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取文本所有内容
        /// </summary>
        /// <returns></returns>
        public string ReadAllText(string filepath)
        {
            if (ExistFile(filepath))
            {
                string strTemp = "";
                using (StreamReader sr = new StreamReader(CommonManager.FileObj.GetPhysicalPath(filepath), Encoding.UTF8))
                {
                    strTemp = sr.ReadToEnd();
                }
                return strTemp;
            }
            else
            {
                return "";
            }
        }
       
        /// <summary>
        /// 更新文本内容
        /// </summary>
        /// <param name="rowid">行号</param>
        /// <param name="text">内容</param>
        public void UpdateText(string filepath, int rowid, string text)
        {
            if (ExistFile(filepath))
            {
                string[] arr = System.IO.File.ReadAllLines(CommonManager.FileObj.GetPhysicalPath(filepath), Encoding.UTF8);
                if (rowid <= arr.Length)
                {
                    arr[rowid - 1] = text;
                }
                System.IO.File.WriteAllLines(CommonManager.FileObj.GetPhysicalPath(filepath), arr, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 删除文本内容
        /// </summary>
        /// <param name="listid"></param>
        public void DeleteText(string filepath, string listid)
        {
            if (string.IsNullOrEmpty(listid))
                return;
            string[] arr = System.IO.File.ReadAllLines(CommonManager.FileObj.GetPhysicalPath(filepath), Encoding.UTF8);
            string[] ids = listid.Split(',');

            foreach (string i in ids)
            {
                UpdateText(filepath,int.Parse(i), "");
            }
            string[] newArr = ReadAllText(filepath).Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            System.IO.File.WriteAllLines(CommonManager.FileObj.GetPhysicalPath(filepath), newArr, Encoding.UTF8);
        }
    }
}

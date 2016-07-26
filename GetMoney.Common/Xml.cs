using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace GetMoney.Common
{
    /// <summary>
    /// Xml 操作类
    /// </summary>
    public sealed class Xml
    {
        internal Xml() { }

        /// <summary>
        /// Xml序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="isAddNamespances">是否添加命名空间</param>
        /// <returns></returns>
        public string SerializeXml<T>(T obj,bool isAddNamespaces)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces();

            if (!isAddNamespaces)               
                Namespaces.Add(string.Empty, string.Empty);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                serializer.Serialize(ms, obj, Namespaces);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// Xml反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">xml字符串</param>
        /// <returns></returns>
        public T DeserializeXml<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                return (T)serializer.Deserialize(ms);
            }
        }

        /// <summary>
        /// Xml反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">文件相对路径</param>
        /// <returns></returns>
        public T DeserializeXmlByFile<T>(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (System.IO.FileStream fs = new System.IO.FileStream(CommonManager.FileObj.GetPhysicalPath(filePath), System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                return (T)serializer.Deserialize(fs);
            }
        }
        

        /// <summary>
        /// Xml反系列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">文件地址</param>
        /// <returns></returns>
        public T DeserializeXmlByUrl<T>(string url)
        {
            if (!url.Contains("http://") && !url.Contains("https://"))
                throw new Exception("文件地址格式不对");
            try
            {
                string response = CommonManager.WebObj.Get(url);
                return DeserializeXml<T>(response);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 保存Xml文件，自动系列化
        /// </summary>
        /// <typeparam name="T">保存的数据</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <param name="data"></param>
        public void SaveFile<T>(string filePath, T data)
        {
            string xmlStr = SerializeXml<T>(data, false);
            using (XmlTextWriter xmlwrite = new XmlTextWriter(CommonManager.FileObj.GetPhysicalPath(filePath), Encoding.UTF8))
            {
                xmlwrite.Formatting = Formatting.Indented;
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(xmlStr);
                xd.WriteContentTo(xmlwrite);
            }
        }
    }
}

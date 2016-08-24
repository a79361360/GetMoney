using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Common
{
    /// <summary>
    /// 通用管理类，单例模式（多线程）
    /// </summary>
    public class CommonManager
    {
        private static readonly object lockobj = new object();
        private static volatile Web _web = null;
        private static volatile File _file = null;
        private static volatile Xml _xml = null;
        private static volatile Txt _txt = null;        
        private static volatile Cache.Cache _cache = null;
        private static volatile Folder _folder = null;

        /// <summary>
        /// Web
        /// </summary>
        public static Web WebObj
        {
            get {
                if (_web == null)
                {
                    lock (lockobj)
                    {
                        if (_web == null)
                            _web = new Web();
                    }
                }
                return _web;
            }
        }

        /// <summary>
        /// File
        /// </summary>
        public static File FileObj
        {
            get {
                if (_file == null)
                {
                    lock (lockobj)
                    {
                        if (_file == null)
                            _file = new File();
                    }
                }
                return _file;
            }
        }

        /// <summary>
        /// Xml
        /// </summary>
        public static Xml XmlObj
        {
            get {
                if (_xml == null)
                {
                    lock (lockobj)
                    {
                        if (_xml == null)
                            _xml = new Xml();
                    }
                }
                return _xml;
            }
        }

        /// <summary>
        /// Txt
        /// </summary>
        public static Txt TxtObj
        {
            get
            {
                if (_txt == null)
                {
                    lock (lockobj)
                    {
                        if (_txt == null)
                            _txt = new Txt();
                    }
                }
                return _txt;
            }
        }

        /// <summary>
        /// Cache
        /// </summary>
        public static Cache.Cache CacheObj
        {
            get { 
                if(_cache==null)
                {
                    lock (lockobj)
                    {
                        if (_cache == null)
                            _cache = new Cache.Cache();
                    }
                }
                return _cache;
            }
        }

        /// <summary>
        /// Folder
        /// </summary>
        public static Folder FolderObj
        {
            get
            {
                if (_folder == null)
                {
                    lock (lockobj)
                    {
                        if (_folder == null)
                            _folder = new Folder();
                    }
                }
                return _folder;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GetMoney.Common
{
    /// <summary>
    /// Folder 操作类
    /// </summary>
    public sealed class Folder
    {
        /// <summary>
        /// 获取文件夹大小(btyes)
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns></returns>
        public long GetFolderSize(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new Exception("路径为空");
            if (!Directory.Exists(path))
                throw new Exception("文件夹不存在");

            long len = 0;

            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo fi in di.GetFiles())
                len += fi.Length;

            DirectoryInfo[] dis = di.GetDirectories();
            for (int i = 0; i < dis.Length; i++)
                len += GetFolderSize(dis[i].FullName);

            return len;
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns></returns>
        public bool DeleteFolder(string path)
        {
            if (Directory.Exists(path))
            {
                foreach (string file in Directory.GetFileSystemEntries(path))
                {
                    if (System.IO.File.Exists(file))
                        System.IO.File.Delete(file);
                    else
                        DeleteFolder(path);
                }

                try
                {
                    Directory.Delete(path);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 创建文件夹，存在则不创建
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <returns></returns>
        public bool CreateFolder(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

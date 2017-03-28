using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Net;
using System.Drawing;

namespace GetMoney.Common
{
    /// <summary>
    /// File 操作类
    /// </summary>
    public sealed class File
    {
        internal File() { }

        /// <summary>
        /// 获取物理路径
        /// </summary>
        /// <param name="virtualPath">虚拟路径</param>
        /// <returns></returns>
        public string GetPhysicalPath(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
                throw new Exception("虚拟路径为空");

            try
            {
                return HttpContext.Current.Server.MapPath(virtualPath);
            }
            catch
            {
                throw new Exception("错误的虚拟路径");
            }
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public bool ExistFile(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
                throw new Exception("路径为空");

            return System.IO.File.Exists(GetPhysicalPath(virtualPath));
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public bool DeleteFile(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
                throw new Exception("路径为空");

            try
            {
                System.IO.File.Delete(GetPhysicalPath(virtualPath));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取文件大小(btyes)
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public long GetFileSize(string virtualPath)
        {
            if (ExistFile(virtualPath))
                return new FileInfo(GetPhysicalPath(virtualPath)).Length;
            return 0;
        }

        /// <summary>
        /// 通过URL采集到图片并保存到本地地址上,如果name为空,随机生成图片名称
        /// </summary>
        /// <param name="url">采集的地址</param>
        /// <param name="path">本地的虚拟地址</param>
        /// <param name="name">生成图片的名称</param>
        /// <returns>图片Bitmap对象</returns>
        public Bitmap DowdLoad_ImgByUrl(string url,string path,string name)
        {
            Bitmap img = null;
            HttpWebRequest req;
            HttpWebResponse res = null;
            string phypath = GetPhysicalPath(path);    //将虚拟地址转为物理地址
            string imgpath = "";
            try
            {
                System.Uri httpUrl = new System.Uri(url);
                req = (HttpWebRequest)(WebRequest.Create(httpUrl));
                req.Timeout = 180000; //设置超时值10秒
                req.Method = "GET";
                res = (HttpWebResponse)(req.GetResponse());
                img = new Bitmap(res.GetResponseStream());//获取图片流
                string suffix = "." + httpUrl.LocalPath.Split('.')[1];  //后缀名
                if (string.IsNullOrEmpty(name)) {
                    name = DateTime.Now.ToFileTime().ToString();
                }
                imgpath = @"" + phypath + "/" + name + suffix;
                img.Save(imgpath);//随机名
            }

            catch (Exception ex)
            {
                string aa = ex.Message;
            }
            finally
            {
                if (res != null) res.Close();
            }
            return img;
        }
        /// <summary>
        /// 通过URL采集到图片并保存到本地地址上,如果name为空,随机生成图片名称
        /// </summary>
        /// <param name="url">采集的地址</param>
        /// <param name="path">本地的虚拟地址</param>
        /// <param name="name">生成图片的名称</param>
        /// <returns>保存图片的虚拟URL地址</returns>
        public string DowdLoad_ImgByUrl(string url, string path, string name,ref int count)
        {
            Bitmap img = null;
            HttpWebRequest req;
            HttpWebResponse res = null;
            string phypath = GetPhysicalPath(path);     //将虚拟地址转为物理地址
            string imgpath = "";                        //包含图片的虚拟地址
            try
            {
                System.GC.Collect();        //回收一下
                System.Uri httpUrl = new System.Uri(url);
                req = (HttpWebRequest)(WebRequest.Create(httpUrl));
                req.Timeout = 180000; //设置超时值10秒
                req.Method = "GET";
                bool ll = req.KeepAlive;
                res = (HttpWebResponse)(req.GetResponse());
                Stream stream = res.GetResponseStream();

                img = new Bitmap(stream);//获取图片流
                string suffix = "." + httpUrl.LocalPath.Split('.')[1];  //后缀名
                if (string.IsNullOrEmpty(name))
                {
                    name = DateTime.Now.ToFileTime().ToString();
                }
                phypath = @"" + phypath + "/" + name + suffix;  //物理地址
                imgpath = @"" + path + "/" + name + suffix;     //虚拟地址
                img.Save(phypath);//随机名

                if (req != null)
                {
                    req.Abort();
                    req = null;
                }
            }

            catch (Exception ex)
            {
                CommonManager.TxtObj.WriteLogs("/Logs/CatchImg_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "注册日志：" + ex.Message);
                count++;
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                    res = null;
                }
            }
            return imgpath;
        }


        public string TestStream(string url, string path, string name, ref int count)
        {
            //Bitmap img = null;
            HttpWebRequest req;
            HttpWebResponse res = null;
            string phypath = GetPhysicalPath(path);     //将虚拟地址转为物理地址
            string imgpath = "";                        //包含图片的虚拟地址
            try
            {
                System.GC.Collect();        //回收一下
                System.Uri httpUrl = new System.Uri(url);
                req = (HttpWebRequest)(WebRequest.Create(httpUrl));
                req.Timeout = 180000; //设置超时值10秒
                req.Method = "GET";
                bool ll = req.KeepAlive;
                res = (HttpWebResponse)(req.GetResponse());
                Stream stream = res.GetResponseStream();

                MemoryStream m = TestStreamForMemoryStream(stream);

                //img = new Bitmap(stream);//获取图片流
                string suffix = "." + httpUrl.LocalPath.Split('.')[1];  //后缀名
                if (string.IsNullOrEmpty(name))
                {
                    name = DateTime.Now.ToFileTime().ToString();
                }
                phypath = @"" + phypath + "/" + name + suffix;  //物理地址
                imgpath = @"" + path + "/" + name + suffix;     //虚拟地址
                //img.Save(phypath);//随机名

                FileStream fs = new FileStream(phypath, FileMode.OpenOrCreate);
                BinaryWriter w = new BinaryWriter(fs);
                w.Write(m.ToArray());
                fs.Close();
                m.Close();

                if (req != null)
                {
                    req.Abort();
                    req = null;
                }
            }

            catch (Exception ex)
            {
                CommonManager.TxtObj.WriteLogs("/Logs/CatchImg_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "注册日志：" + ex.Message);
                count++;
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                    res = null;
                }
            }
            return imgpath;
        }

        /// <summary>
        /// 返回内存流
        /// </summary>
        /// <param name="inStream"></param>
        /// <returns></returns>
        public MemoryStream TestStreamForMemoryStream(Stream inStream)
        {
            MemoryStream ms = new MemoryStream();
            byte[] buffer = new byte[1024];

            while (true)
            {
                int sz = inStream.Read(buffer, 0, 1024);
                if (sz == 0) break;
                ms.Write(buffer, 0, sz);
            }
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// 单个文件上传的时候使用这个方法
        /// </summary>
        /// <param name="virtualpath">虚拟路径,指定到图片目录</param>
        /// <param name="filename">文件名称,如果为空就使用原来图片的名称,不包括扩展名</param>
        /// <param name="allowsuffix">允许的扩展名,为空表示不限制</param>
        /// <returns></returns>
        public string HttpUploadFile(string virtualpath,string filename) {
            string path = "", suffix = "";
            if (HttpContext.Current.Request.Files.AllKeys.Length > 0)
            {
                try
                {
                    string filePath = HttpContext.Current.Server.MapPath(virtualpath);
                    if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
                    string fname= HttpContext.Current.Request.Files[0].FileName;
                    suffix = fname.Substring(fname.LastIndexOf(".") + 1, fname.Length - (fname.LastIndexOf(".") + 1));
                    if (string.IsNullOrEmpty(filename))
                    {
                        filename = HttpContext.Current.Request.Files[0].FileName;
                    }
                    //这里我直接用索引来获取第一个文件，如果上传了多个文件，可以通过遍历HttpContext.Current.Request.Files.AllKeys取“key值”，再通过HttpContext.Current.Request.Files[“key值”]获取文件
                    path = Path.Combine(filePath, filename);
                    HttpContext.Current.Request.Files[0].SaveAs(path);
                }
                catch { 

                }
            }
            return path;
        }
    }
}

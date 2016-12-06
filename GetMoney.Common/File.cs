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
        /// 上传图片文件
        /// </summary>
        /// <param name="url">提交的地址</param>
        /// <param name="poststr">发送的文本串   比如：user=eking&pass=123456  </param>
        /// <param name="fileformname">文本域的名称  比如：name="file"，那么fileformname=file  </param>
        /// <param name="filepath">上传的文件路径  比如： c:\12.jpg </param>
        /// <param name="cookie">cookie数据</param>
        /// <param name="refre">头部的跳转地址</param>
        /// <returns></returns>
        public string HttpUploadFile(string url, string poststr, string fileformname, string filepath, string cookie, string refre)
        {

            // 这个可以是改变的，也可以是下面这个固定的字符串
            string boundary = "—————————7d930d1a850658";

            // 创建request对象
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webrequest.Method = "POST";
            webrequest.Headers.Add("Cookie: " + cookie);
            webrequest.Referer = refre;

            // 构造发送数据
            StringBuilder sb = new StringBuilder();

            // 文本域的数据，将user=eking&pass=123456  格式的文本域拆分 ，然后构造
            foreach (string c in poststr.Split('&'))
            {
                string[] item = c.Split('=');
                if (item.Length != 2)
                {
                    break;
                }
                string name = item[0];
                string value = item[1];
                sb.Append("–" + boundary);
                sb.Append("\r\n");
                sb.Append("Content-Disposition: form-data; name=\"" + name + "\"");
                sb.Append("\r\n\r\n");
                sb.Append(value);
                sb.Append("\r\n");
            }

            // 文件域的数据
            sb.Append("–" + boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"icon\";filename=\"" + filepath + "\"");
            sb.Append("\r\n");

            sb.Append("Content-Type: ");
            sb.Append("image/pjpeg");
            sb.Append("\r\n\r\n");

            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

            //构造尾部数据
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n–" + boundary + "–\r\n");

            FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            long length = postHeaderBytes.Length + fileStream.Length + boundaryBytes.Length;
            webrequest.ContentLength = length;

            Stream requestStream = webrequest.GetRequestStream();

            // 输入头部数据
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

            // 输入文件流数据
            byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);

            // 输入尾部数据
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            // 返回数据流(源码)
            return sr.ReadToEnd();
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

                System.Drawing.Image ResourceImage = System.Drawing.Image.FromStream(stream);
                
                //img = new Bitmap(stream);//获取图片流

                string suffix = "." + httpUrl.LocalPath.Split('.')[1];  //后缀名
                if (string.IsNullOrEmpty(name))
                {
                    name = DateTime.Now.ToFileTime().ToString();
                }
                phypath = @"" + phypath + "/" + name + suffix;  //物理地址
                imgpath = @"" + path + "/" + name + suffix;     //虚拟地址
                //img.Save(phypath);//随机名
                ResourceImage.Save(phypath);
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
    }
}

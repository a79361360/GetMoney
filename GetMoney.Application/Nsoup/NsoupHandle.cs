using GetMoney.Common;
using GetMoney.Dal;
using GetMoney.Dal.Nsoup;
using GetMoney.Model;
using GetMoney.Model.Model.Nsoup;
using NSoup.Select;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace GetMoney.Application.Nsoup
{
    public class NsoupHandle
    {
        int ExecuteNum = 0;     //执行次数
        NsoupDal dal = new NsoupDal();
        /// <summary>
        /// 根据抓取地址,取得里面的下级地址,再保存到指定下载地址
        /// </summary>
        /// <param name="url">抓取地址</param>
        /// <param name="downpath">保存地址</param>
        /// <param name="hosturi">网址使用虚拟地址的时候取得HOST进行拼接,生成完整http访问地址</param>
        public void CatchUriByPUri(string url,string downpath,string hosturi) {
            Elements links = null;
            MyWebClient webClient = new MyWebClient();
            String HtmlString = GetHtmlString(webClient, url);
            //三次都在超时就暂时不抓了
            if (string.IsNullOrEmpty(HtmlString)) {
                return;
            }
            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(HtmlString);
            links = doc.Select(".t_one h3 a");
            foreach (var item in links)
            {
                string uri = hosturi + item.Attr("href");               //抓取图片的URI
                string text = FilterFloder(item.Text());                //将Title做为名称创建成文件夹,是否合法
                int titleid = dal.CreateTitle(101, text);               //添加类型到数据库
                string path = downpath + titleid;                       //存放图片的文件夹
                if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(path)) {
                    CatchImgPutPath(uri, path,titleid);
                }
                //return; //调试阶段不用执行那么多次
            }
        }
        /// <summary>
        /// 创建文件夹,并保存图片到此文件夹
        /// </summary>
        /// <param name="url">抓取图片的URI</param>
        /// <param name="path">保存图片的PATH</param>
        public void CatchImgPutPath(string url,string path,int titleid) {
            CreateFloder(path);     //创建文件夹
            Elements links = null;
            MyWebClient webClient = new MyWebClient();
            ExecuteNum = 0;     //将重连次数清零
            String HtmlString = GetHtmlString(webClient, url);
            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(HtmlString);
            links = doc.Select("#read_tpc img");
            foreach (var item in links) {
                string imguri = item.Attr("src");
                if (!string.IsNullOrEmpty(imguri))
                {
                    int count = 0; string filename = "";
                    string imgpath = TestStream(imguri, path,ref filename, ref count);
                    while (string.IsNullOrEmpty(imgpath) && count < 3)
                    {
                        imgpath = TestStream(imguri, path, ref filename, ref count);
                    }
                    if (!string.IsNullOrEmpty(imgpath)) {
                        dal.AddTitleDetail(101, titleid, filename, imgpath);
                    }
                }
            }
        }
        /// <summary>
        /// 一个网址连续抓取三次,如果三次都抓取超时或者失败,就不抓了
        /// </summary>
        /// <param name="webClient"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetHtmlString(MyWebClient webClient,string url)
        {
            string result = "";
            try
            {
                result = Encoding.GetEncoding("utf-8").GetString(webClient.DownloadData(url));
            }
            catch (Exception er)
            {
                ExecuteNum++;
                if (ExecuteNum > 3) { return ""; }
                GetHtmlString(webClient, url);
            }
            return result;
        }
        /// <summary>
        /// 将非法字符过滤成空
        /// </summary>
        /// <param name="floder"></param>
        private string FilterFloder(string floder) {
            //string illegal = "\"M\"\\a/ry/ h**ad:>> a\\/:*?\"| li*tt|le|| la\"mb.?";
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            foreach (char c in invalid)
            {
                //illegal = illegal.Replace(c.ToString(), "");
                floder = floder.Replace(c.ToString(), "");
            }
            return floder;
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path"></param>
        private void CreateFloder(string path) {
            path = CommonManager.FileObj.GetPhysicalPath(path);
            CommonManager.FolderObj.CreateFolder(path);
        }
        /// <summary>
        /// 获取图片流转换后保存到磁盘地址
        /// </summary>
        /// <param name="url"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public string TestStream(string url, string path, ref string name, ref int count)
        {
            //Bitmap img = null;
            HttpWebRequest req;
            HttpWebResponse res = null;
            string phypath = CommonManager.FileObj.GetPhysicalPath(path);     //将虚拟地址转为物理地址
            string imgpath = "";                        //包含图片的虚拟地址
            try
            {
                System.GC.Collect();        //回收一下
                System.Uri httpUrl = new System.Uri(url);
                req = (HttpWebRequest)(WebRequest.Create(httpUrl));
                req.Timeout = 180000; //设置超时值10秒
                req.Method = "GET";
                res = (HttpWebResponse)(req.GetResponse());
                Stream stream = res.GetResponseStream();

                CommonManager.TxtObj.WriteLogs("/Logs/CatchImg_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "开始转换成内存流：" + path);
                MemoryStream m = TestStreamForMemoryStream(stream);     //保存到内存流
                CommonManager.TxtObj.WriteLogs("/Logs/CatchImg_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "转换成内存流结束：" + path);
                string suffix = "." + httpUrl.LocalPath.Split('.')[1];  //后缀名
                if (string.IsNullOrEmpty(name))
                {
                    name = DateTime.Now.ToFileTime().ToString();
                }
                phypath = @"" + phypath + "/" + name + suffix;  //物理地址
                imgpath = @"" + path + "/" + name + suffix;     //虚拟地址

                CommonManager.TxtObj.WriteLogs("/Logs/CatchImg_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "开始创建成文件流：" + path);
                FileStream fs = new FileStream(phypath, FileMode.OpenOrCreate);
                CommonManager.TxtObj.WriteLogs("/Logs/CatchImg_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "创建文件流结束：" + path);
                BinaryWriter w = new BinaryWriter(fs);
                CommonManager.TxtObj.WriteLogs("/Logs/CatchImg_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "写入文件内容结束：" + path);
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
                CommonManager.TxtObj.WriteLogs("/Logs/CatchImg_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "绝对值地址：" + path + "Exception：" + ex.Message);
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
        /// 
        /// </summary>
        /// <param name="type">图片类型</param>
        /// <param name="TitleId">Titleid</param>
        /// <returns></returns>
        public IList<Nsoup_ImgDetailDto> ListImgByTid(int type,int TitleId) {
            IList<Nsoup_ImgDetailDto> list = DataTableToList.ModelConvertHelper<Nsoup_ImgDetailDto>.ConvertToModel(dal.ListDetailTitle(type, TitleId));
            return list;
        }
        public IList<Nsoup_ImgDetailDto> ListImgPage(ref int Total, int pageSize, int pageIndex, string filter)
        {
            SqlPageParam param = new SqlPageParam();
            param.TableName = "V_NsoupImg";
            param.PrimaryKey = "id";
            param.Fields = "[id],[Type],[TitleId],TitleName,[ImgUrl],Convert(varchar(19),[AddTime],12) AddTime,[ImgName]";
            param.PageSize = pageSize;
            param.PageIndex = pageIndex;
            param.Filter = filter;
            param.Group = "";
            param.Order = "id desc";
            IList<Nsoup_ImgDetailDto> list = DataTableToList.ModelConvertHelper<Nsoup_ImgDetailDto>.ConvertToModel(dal.ListDetailPage(ref Total, param));
            return list;
        }
        public IList<Nsoup_ImgDetailDto> ListImgTitlePage(ref int Total, int pageSize, int pageIndex, string filter) {
            SqlPageParam param = new SqlPageParam();
            param.TableName = "Nsoup_ImgTitle";
            param.PrimaryKey = "id";
            param.Fields = "[id],[Type],[Title] TitleName";
            param.PageSize = pageSize;
            param.PageIndex = pageIndex;
            param.Filter = filter;
            param.Group = "";
            param.Order = "id desc";
            IList<Nsoup_ImgDetailDto> list = DataTableToList.ModelConvertHelper<Nsoup_ImgDetailDto>.ConvertToModel(dal.ListDetailPage(ref Total, param));
            return list;
        }


        public IList<Nsoup_bankDto> binhtml(string url) {
            Elements links = null;
            MyWebClient webClient = new MyWebClient();
            String HtmlString = GetHtmlString(webClient, url);

            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(HtmlString);
            links = doc.Select("tr");
            IList<Nsoup_bankDto> list = new List<Nsoup_bankDto>();
            int index = 0;
            foreach (var item in links)
            {
                index++;
                if (index == 1) continue;
                try
                {
                    if (index == 679) {
                        string ll = "1";
                    }
                    Nsoup_bankDto dto = new Nsoup_bankDto();
                    string id = item.Select("td")[0].Text().Trim();
                    string bankName = item.Select("td")[1].Text().Trim();
                    string bankNameEn = item.Select("td")[2].Text().Trim();
                    string issueid = item.Select("td")[3].Text().Trim();
                    string cardName = item.Select("td")[4].Text().Trim();
                    string nLength = item.Select("td")[5].Text().Trim();
                    string binLength = item.Select("td")[6].Text().Trim();
                    string bin = item.Select("td")[7].Text().Trim() == "(NULL)" ? "0" : item.Select("td")[7].Text().Trim();
                    string cardType = item.Select("td")[8].Text().Trim();
                    dto.id = Convert.ToInt32(id); dto.bankName = bankName;
                    dto.bankNameEn = bankNameEn; dto.issueid = Convert.ToInt32(issueid);
                    dto.cardName = cardName; dto.nLength = Convert.ToInt32(nLength);
                    dto.binLength = Convert.ToInt32(binLength); dto.bin = Convert.ToInt64(bin);
                    dto.cardType = cardType;
                    list.Add(dto);
                }
                catch(Exception er) {
                    throw er;
                }
            }
            return list;
        }
    }
}

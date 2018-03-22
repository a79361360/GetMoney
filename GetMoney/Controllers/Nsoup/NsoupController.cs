using GetMoney.Application.Nsoup;
using GetMoney.Common;
using GetMoney.Framework;
using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetMoney.Controllers.Nsoup
{
    public class NsoupController : BaseController
    {
        NsoupHandle bll = new NsoupHandle();
        //
        // GET: /Nsoup/

        public ActionResult Index()
        {
            return View();
        }
        public void GetImgByUrl() {
            string url = "http://x2.pix378.pw/pw/thread.php?fid=15&page=2";
            System.Uri httpUrl = new System.Uri(url);
            string hosturl = httpUrl.AbsoluteUri.Substring(0, httpUrl.AbsoluteUri.LastIndexOf('/') + 1);
            NsoupHandle handle = new NsoupHandle();
            //handle.CatchUriByPUri(url, "/DownLoad/Img/", hosturl);
            handle.CatchUriByPUri(url, "/DownLoad/OriginalImg/", hosturl);
        }
        public ActionResult NsoupImgIndex() {
            return View();
        }
        /// <summary>
        /// 会单列表(翻页)
        /// </summary>
        /// <returns></returns>
        public ActionResult ListImgPage()
        {
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            string type = CommonManager.WebObj.Request("type", "");
            string text = CommonManager.WebObj.Request("text", "");
            string filter = "";
            if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(text))
            {
                filter = type + " like '%" + text + "%'";
            }
            int Total = 0;
            IList<Nsoup_ImgDetailDto> list = bll.ListImgPage(ref Total, pageSize, pageIndex, filter);
            if (list.Count > 0)
                return JsonFormat(new ExtJsonPage { success = true, code = 1000, msg = "查询成功！", total = Total, list = list });
            else
                return JsonFormat(new ExtJsonPage { success = false, code = -1000, msg = "查询失败！" });
        }
        /// <summary>
        /// 使用URL查询单图
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowImg() {
            string type = CommonManager.WebObj.Request("type", ""); //1直接使用ImgUrl,2查询出url列表
            IList<Nsoup_ImgDetailDto> list = new List<Nsoup_ImgDetailDto>();
            if (type == "1")
            {
                string imgurl = Request["ImgUrl"].ToString();
                Nsoup_ImgDetailDto dto = new Nsoup_ImgDetailDto();
                dto.ImgUrl = imgurl;
                list.Add(dto);
            }
            else if (type == "2") {
                string imgtype = CommonManager.WebObj.Request("imgtype", "");
                string titleid = CommonManager.WebObj.Request("titleid", "");
                list = bll.ListImgByTid(Convert.ToInt32(imgtype), Convert.ToInt32(titleid));
            }
            ViewBag.ListImg = list;
            return View();
        }
        public ActionResult ListImgTitle() {
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            string type = CommonManager.WebObj.Request("type", "");
            string text = CommonManager.WebObj.Request("text", "");
            string filter = "";
            if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(text))
            {
                filter = type + " like '%" + text + "%'";
            }
            int Total = 0;
            IList<Nsoup_ImgDetailDto> list = bll.ListImgTitlePage(ref Total, pageSize, pageIndex, filter);
            if (list.Count > 0)
                return JsonFormat(new ExtJsonPage { success = true, code = 1000, msg = "查询成功！", total = Total, list = list });
            else
                return JsonFormat(new ExtJsonPage { success = false, code = -1000, msg = "查询失败！" });
        }
        public ActionResult NsoupTitlePortal() {
            return View();
        }
    }
}

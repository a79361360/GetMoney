using GetMoney.Application.OnlyNameTest;
using GetMoney.Framework;
using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GetMoney.Controllers.OnlyNameTest
{
    public class OnlyNameTestController : BaseController
    {
        //
        // GET: /OnlyNameTest/
        IOnlyNameTestBll _bll = null;
        public OnlyNameTestController(IOnlyNameTestBll bll)
        {
            _bll = bll;
        }
        public void TestDbcontext()
        {
            OnlyNameTestDto dto = new OnlyNameTestDto();
            //新增
            dto.Name = "qiuqiu";
            dto.InputDate = DateTime.Now;
            _bll.Add(dto);
            //Db.OnlyNameTest.Add(dto);
            //Db.SaveChanges();

        }
        public ActionResult Test() {
            return View();
        }
        public ActionResult TTest()
        {
            Get_img();
            return View();
        }
        public Bitmap Get_img()
        {
            Bitmap img = null;
            HttpWebRequest req;
            HttpWebResponse res = null;
            try
            {
                System.Uri httpUrl = new System.Uri("http://img.my.csdn.net/uploads/201504/28/1430184401_2155.jpg");
                req = (HttpWebRequest)(WebRequest.Create(httpUrl));
                req.Timeout = 180000; //设置超时值10秒
                //req.UserAgent = "Mozilla/4.0";
                //req.Accept = "XXXXXX";
                req.Method = "GET";
                res = (HttpWebResponse)(req.GetResponse());
                img = new Bitmap(res.GetResponseStream());//获取图片流                
                img.Save(@"E:/" + DateTime.Now.ToFileTime().ToString() + ".jpg");//随机名
            }

            catch (Exception ex)
            {
                string aa = ex.Message;
            }
            finally
            {
                res.Close();
            }
            return img;
        }
    }
}

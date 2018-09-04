using GetMoney.Application.Email;
using GetMoney.Application.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetMoney.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public void LoginByUserPwd()
        {
            string name = Request.Form["login"].ToString();
            string pwd = Request.Form["password"].ToString();
            
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Default()
        {
            return View();
        }
        public ActionResult UserErr() {
            return View();
        }
        /// <summary>
        /// 基本的使用方法说明
        /// </summary>
        /// <returns></returns>
        public ActionResult UserRemark() {
            return View();
        }
        public void TestJob() {
            SystemScheduler _systemScheduler = SystemScheduler.CreateInstance();
            _systemScheduler.StartScheduler();
        }
        public void TestEmail() {
            EmailBLL bll = new EmailBLL();
            string content = "<p style='color:red;font-size:14px;'>邮件测试</p>";
            //bll.SendMail("592452713@qq.com", "Hello", content);
            string[] receivers = { "592452713@qq.com", "a79361360@163.com" };
            string[] filepath = { @"F:\奖励通知.docx", @"F:\111.txt" };
            bll.SendMail(receivers, filepath, "Hello", content);
        }
    }
}

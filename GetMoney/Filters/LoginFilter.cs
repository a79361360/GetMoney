using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetMoney.Filters
{
    public class LoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var isNull = HttpContext.Current.Session["uid"];
            if (isNull != null)
            {
                var user = HttpContext.Current.Session["uid"];
                //判断请求的用户是否有身份凭证
                if (user == null || (string)user == "")
                {
                    //设置页面的跳转 Account 是控制器   LogOn是路由名称
                    HttpContext.Current.Response.Redirect("/Home/Index");
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("/Home/Index");
            }
        }
    }
}
using GetMoney.Application;
using GetMoney.Common;
using GetMoney.Common.SerializeObject;
using GetMoney.Framework;
using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetMoney.Controllers.Order
{
    public class OrderController : BaseController
    {
        IOrderBll _bll = null;
        public OrderController(IOrderBll bll)
        {
            _bll = bll;
        }
        //
        // GET: /Order/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddOrder()
        { 
            OrderDto dto = new OrderDto();
            dto.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
            dto.PeoperNum = 20;
            dto.PeoperMoney = 2000;
            dto.MoneySendType = 1;
            dto.MeetType = 1;
            dto.MeetNum = 1;
            dto.MeetDate = "26";
            dto.MeetTime = Convert.ToDateTime("19:00");
            dto.InputDate = DateTime.Now;
            dto.State = 1;
            dto.Remark = "测试";
            _bll.AddOrder(dto);
            return JsonFormat(new ExtJson { success = true, msg = "添加成功！" });
        }
        public ActionResult Remove() {
            string id = "1";
            bool result = _bll.RemoveOrder(id);
            if (result) 
                return JsonFormat(new ExtJson { success = true, msg = "删除成功！" });
            else
                return JsonFormat(new ExtJson { success = false, msg = "删除失败！" });
        }
        public ActionResult ListOrderPage()
        {
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            string type = CommonManager.WebObj.Request("type", "");
            string text = CommonManager.WebObj.Request("text", "");
            string filter = "";
            if (!string.IsNullOrEmpty(text))
            {
                filter = type + " like '%" + text + "%'";
            }
            int Total = 0;
            IList<OrderDto> list = _bll.ListOrderPage(ref Total, pageSize, pageIndex, filter);
            if (list.Count > 0)
                return JsonFormat(new ExtJsonPage { success = true, code = 1000, msg = "查询成功！", total = Total, list = list });
            else
                return JsonFormat(new ExtJsonPage { success = false, code = -1000, msg = "查询失败！" });



        }
        public ActionResult OrderPortal() {
            return View();
        }
        public ActionResult CreateOrder() {
            string PeoperNum = Request.Form["peonum"];          //会员人数
            string Peoper = Request.Form["uids"];               //会员列表
            string PeoperMoney = Request.Form["peomoney"];      //会费金额
            string MoneySendType = Request.Form["moneystype"];  //会费发放方式
            string MeetType = Request.Form["meettype"];         //标会类型
            string MeetNum = Request.Form["meetnum"];           //每月标会次数
            string FirstDate = Request.Form["firstdate"];        //首次标会日期
            //string MeetDate = Request.Form["meetdate"];         //标会日期
            //string MeetTime = Request.Form["meettime"];         //标会时间
            string MeetDate = "20";         //标会日期
            string MeetTime = "19:00";         //标会时间
            OrderDto dto = new OrderDto();
            dto.PeoperNum = Convert.ToInt32(PeoperNum);
            IList<UListDto> list = SerializeJson<UListDto>.JSONStringToList(Peoper);    //会员列表
            dto.PeoperIds = _bll.ListToString(list);    //会员ids
            dto.PeoperMoney = Convert.ToInt32(PeoperMoney);
            dto.MoneySendType = Convert.ToInt32(MoneySendType);
            dto.MeetType = Convert.ToInt32(MeetType);
            dto.MeetNum = Convert.ToInt32(MeetNum);
            dto.FirstDate = Convert.ToDateTime(FirstDate);
            dto.MeetDate = MeetDate;
            dto.MeetTime = Convert.ToDateTime(MeetTime);
            int result = _bll.CreateOrder(dto);
            if (result == 1)
            {
                return JsonFormat(new ExtJson { success = true, msg = "添加成功！" });
            }
            else {
                return JsonFormat(new ExtJson { success = false, msg = "添加失败！" });
            }
        }
    }
}

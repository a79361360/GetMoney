using GetMoney.Application;
using GetMoney.Common;
using GetMoney.Common.SerializeObject;
using GetMoney.Filters;
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
        public ActionResult OrderListViewPortal() {
            string OrderNo = CommonManager.WebObj.Request("orderno", "");
            OrderDto dto = null;
            if (!string.IsNullOrEmpty(OrderNo)) {
                dto = _bll.GetOrderByOrderID(OrderNo);
                dto.List = _bll.OrderLists(OrderNo);
                ViewData["listuser"] = _bll.OrderListUser(dto.List[0].ID.ToString());
            }
            return View(dto);
        }
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
            dto.MoneySendType = (MnSdTypeEnum)1;
            dto.MeetType = 1;
            dto.MeetNum = 1;
            dto.FirstExtraDate = DateTime.Now;
            dto.ExtraDate = "";
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
        /// <summary>
        /// 会单列表(翻页)
        /// </summary>
        /// <returns></returns>
        public ActionResult ListOrderPage()
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
            IList<OrderDto> list = _bll.ListOrderPage(ref Total, pageSize, pageIndex, filter);
            if (list.Count > 0)
                return JsonFormat(new ExtJsonPage { success = true, code = 1000, msg = "查询成功！", total = Total, list = list });
            else
                return JsonFormat(new ExtJsonPage { success = false, code = -1000, msg = "查询失败！" });
        }
        /// <summary>
        /// 取得我的正在进行中/已经结束的会单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult MyListOrderPage() {
            if(Session["uid"]==null)
                return JsonFormat(new ExtJsonPage { success = false, code = -1001, msg = "登入状态已失效！" });
            int pageIndex = Convert.ToInt32(Request["pageIndex"]) - 1;      //页
            int pageSize = Convert.ToInt32(Request["pageSize"]);            //每页条数
            string state = CommonManager.WebObj.Request("state", "");       //1为正在进行中,2为已经结束的
            int uid = Convert.ToInt32(Session["uid"]);                      //登入的用户ID
            string btime = CommonManager.WebObj.Request("btime", "");       //开始时间
            string etime = CommonManager.WebObj.Request("etime", "");       //结束时间

            string filter = uid + " IN(SELECT DISTINCT Userid FROM dbo.Order_ListUsers WHERE OrderNo = View_OrderUser.OrderNo)";
            if (state != "-1")
                filter += " AND State=" + state;
            if (!string.IsNullOrEmpty(btime))
                filter += " and FirstDate>='" + btime + " 00:00:00'";
            if (!string.IsNullOrEmpty(etime))
                filter += " and FirstDate<='" + etime + " 23:59:59.999'";

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
        /// <summary>
        /// 创建会单
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateOrder() {
            if (Session["uid"] == null)
            {
                return JsonFormat(new ExtJsonPage { success = false, code = -1000, msg = "登入状态已失效！" });
            }
            string TouUserid = Session["uid"].ToString();
            string PeoperNum = Request.Form["peonum"];              //会员人数
            string Peoper = Request.Form["uids"];                   //会员列表
            string PeoperMoney = Request.Form["peomoney"];          //会费金额
            string LowestMoney = Request.Form["lowestmoney"];       //最低标会金额
            string Remark = Request.Form["remark"];                 //备注
            string MoneySendType = Request.Form["moneystype"];      //会费发放方式
            string MeetType = Request.Form["meettype"];             //标会类型
            string MeetNum = Request.Form["meetnum"];               //每N月标会次数
            string Meetextnum = Request.Form["meetextnum"];         //每N月加标次数
            string Address = Request.Form["address"];               //标会地址
            string FirstDate = Request.Form["firstdate"];           //首次标会日期
            string FirstExtraDate = CommonManager.WebObj.RequestForm("firstextradate", DateTime.Now.ToString());            //首次加标日期时间
            string ExtraDate = CommonManager.WebObj.RequestForm("extradate", "");            //自定义加标日期列表

            OrderDto dto = new OrderDto();
            dto.PeoperNum = Convert.ToInt32(PeoperNum);
            IList<UListDto> list = SerializeJson<UListDto>.JSONStringToList(Peoper);    //会员列表
            dto.PeoperIds = _bll.ListToString(list);    //会员ids
            dto.PeoperMoney = Convert.ToInt32(PeoperMoney);
            dto.LowestMoney = Convert.ToInt32(LowestMoney);
            dto.Remark = Remark;
            dto.TouUserid = Convert.ToInt32(TouUserid);
            dto.MoneySendType = (MnSdTypeEnum)Convert.ToInt32(MoneySendType);
            dto.MeetType = Convert.ToInt32(MeetType);
            dto.MeetNum = Convert.ToInt32(MeetNum);
            dto.Meetextnum = Convert.ToInt32(Meetextnum);
            //if (dto.MeetType == 1 || dto.MeetType == 2) {
            //    dto.MeetNum = 1;
            //}
            dto.Address = Address;
            dto.FirstDate = Convert.ToDateTime(FirstDate);
            if (!string.IsNullOrEmpty(FirstExtraDate))
            {
                dto.FirstExtraDate = Convert.ToDateTime(FirstExtraDate);
            }
            int result = _bll.CreateOrder(dto);
            if (result == 1)
            {
                return JsonFormat(new ExtJson { success = true, msg = "添加成功！" });
            }
            else {
                return JsonFormat(new ExtJson { success = false, msg = "添加失败！" });
            }
        }
        /// <summary>
        /// 当前会单的记录列表
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderLists() {
            string OrderNo = CommonManager.WebObj.Request("orderno", "");
            if (string.IsNullOrEmpty(OrderNo)) {
                return JsonFormat(new ExtJson { success = false, code = -1000, msg = "单号不能为空！" });
            }
            IList<OrderListDto> list = _bll.OrderLists(OrderNo);
            if (list.Count > 0)
                return JsonFormat(new ExtJson { success = true, code = 1000, msg = "查询成功！", jsonresult = list });
            else
                return JsonFormat(new ExtJson { success = false, code = -1000, msg = "查询失败！" });
        }
        /// <summary>
        /// 当前会单记录的用户明细
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderListUsers()
        {
            string OrderListID = CommonManager.WebObj.Request("listid", "");
            if (string.IsNullOrEmpty(OrderListID))
            {
                return JsonFormat(new ExtJson { success = false, code = -1000, msg = "单号不能为空！" });
            }
            IList<OrderListUserDto> list = _bll.OrderListUser(OrderListID);
            if (list.Count > 0)
                return JsonFormat(new ExtJson { success = true, code = 1000, msg = "查询成功！", jsonresult = list });
            else
                return JsonFormat(new ExtJson { success = false, code = -1000, msg = "查询失败！" });
        }
        /// <summary>
        /// 更新会单记录的最终竞标结果,并且更新状态(1为结束,2为未结束,3为异常)
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateOrderListState() { 
            //更新互助单记录及状态
            string OrderNo = CommonManager.WebObj.Request("orderno", "");
            string OrderListID = CommonManager.WebObj.Request("listid", "");
            if (string.IsNullOrEmpty(OrderNo) || string.IsNullOrEmpty(OrderListID))
            {
                return JsonFormat(new ExtJson { success = false, code = -1000, msg = "参数不能为空！" });
            }
            OrderListDto dto = _bll.UpdateOrderListState(OrderNo, OrderListID);
            if (dto !=null)
                return JsonFormat(new ExtJson { success = true, code = 1000, msg = "查询成功！", jsonresult = dto });
            else
                return JsonFormat(new ExtJson { success = false, code = -1000, msg = "查询失败！" });
        }
        /// <summary>
        /// 更新当前用户竞标金额
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateOrderListUserMoney()
        {
            if (Session["uid"] == null)
            {
                return JsonFormat(new ExtJsonPage { success = false, code = -1000, msg = "登入状态已失效！" });
            }
            //更新互助单记录及状态
            string OrderNo = CommonManager.WebObj.Request("orderno", "");
            string OrderListID = CommonManager.WebObj.Request("listid", "");
            //string Userid = CommonManager.WebObj.Request("userid", "");
            int Userid = Convert.ToInt32(Session["uid"]);               //当前登入的用户
            string Money = CommonManager.WebObj.Request("money", "");
            if (string.IsNullOrEmpty(OrderNo) || string.IsNullOrEmpty(OrderListID) || string.IsNullOrEmpty(Money))
            {
                return JsonFormat(new ExtJson { success = false, code = -1000, msg = "参数不能为空！" });
            }
            int result = _bll.UpdateOrderListUserMoney(OrderNo, OrderListID, Userid, Convert.ToInt32(Money));
            if (result == 1)
                return JsonFormat(new ExtJson { success = true, code = 1000, msg = "更新成功！" });
            else
                return JsonFormat(new ExtJson { success = false, code = -1000, msg = "更新失败！" });
        }
        /// <summary>
        /// 取得当前用户上一次的标金金额
        /// </summary>
        /// <returns></returns>
        public ActionResult GetOrderListUserPrvMoney() {
            if (Session["uid"] == null)
            {
                return JsonFormat(new ExtJsonPage { success = false, code = -1000, msg = "登入状态已失效！" });
            }
            int Userid = Convert.ToInt32(Session["uid"]);               //当前登入的用户
            string OrderNo = CommonManager.WebObj.Request("orderno", "");
            string OrderListID = CommonManager.WebObj.Request("listid", "");
            if (string.IsNullOrEmpty(OrderListID))
            {
                return JsonFormat(new ExtJson { success = false, code = -1000, msg = "参数不能为空！" });
            }
            //验证是否有权限去填写标金
            int verresult = _bll.VerUserUpdateMoney(Userid, OrderNo, OrderListID);
            if (verresult == 1)
            {
                OrderListUserDto dto = _bll.GetOrderListUserPrvMoney(Userid, OrderNo, OrderListID);
                if (dto != null)
                    return JsonFormat(new ExtJson { success = true, code = 1000, msg = "更新成功！", jsonresult = dto });
                else
                    return JsonFormat(new ExtJson { success = false, code = -1000, msg = "更新失败！" });
            }
            else {
                return JsonFormat(new ExtJson { success = false, code = -1001, msg = "验证权限失败！" });
            }
        }
        /// <summary>
        /// 移除会单
        /// </summary>
        /// <returns></returns>
        public ActionResult RemoveOrder() {
            string OrderNo = CommonManager.WebObj.Request("orderno", "");
            string OrderNos = CommonManager.WebObj.Request("ordernos", "");
            int result = -1;
            if (!string.IsNullOrEmpty(OrderNo)) {
                result = _bll.DelOrder(OrderNo);
            }
            if (!string.IsNullOrEmpty(OrderNos))
            {
                IList<UListDto> list = SerializeJson<UListDto>.JSONStringToList(OrderNos);    //会员列表
                result = 0;
                foreach (var item in list) {
                    int temp = _bll.DelOrder(item.orderno);
                    if (temp == 1) {
                        result = result + temp;
                    }
                }
            }
            if (result > 0)
            {
                return JsonFormat(new ExtJson { success = true, msg = "删除成功！" });
            }
            else {
                return JsonFormat(new ExtJson { success = false, msg = "删除失败！" });
            }
        }
        /// <summary>
        /// 会单记录的会钱缴付情况
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderPayMentPortal() {
            return View();
        }
        /// <summary>
        /// 手机会单主页
        /// </summary>
        /// <returns></returns>
        public ActionResult PIndex() {
            return View();
        }
    }
}

﻿using GetMoney.Application;
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
            dto.MoneySendType = (MnSdTypeEnum)1;
            dto.MeetType = 1;
            dto.MeetNum = 1;
            dto.MeetDate = "26";
            dto.MeetTime = "19:00";
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
            if (Session["uid"] == null)
            {
                return JsonFormat(new ExtJsonPage { success = false, code = -1000, msg = "登入状态已失效！" });
            }
            string TouUserid = Session["uid"].ToString();
            string PeoperNum = Request.Form["peonum"];              //会员人数
            string Peoper = Request.Form["uids"];                   //会员列表
            string PeoperMoney = Request.Form["peomoney"];          //会费金额
            string LowestMoney = Request.Form["lowestmoney"];       //最低标会金额
            string MoneySendType = Request.Form["moneystype"];      //会费发放方式
            string MeetType = Request.Form["meettype"];             //标会类型
            string MeetNum = Request.Form["meetnum"];               //每月标会次数
            string FirstDate = Request.Form["firstdate"];           //首次标会日期
            //string MeetDate = Request.Form["meetdate"];           //标会日期
            //string MeetTime = Request.Form["meettime"];           //标会时间
            string MeetDate = "20";                                 //标会日期
            string MeetTime = "19:00";                              //标会时间
            OrderDto dto = new OrderDto();
            dto.PeoperNum = Convert.ToInt32(PeoperNum);
            IList<UListDto> list = SerializeJson<UListDto>.JSONStringToList(Peoper);    //会员列表
            dto.PeoperIds = _bll.ListToString(list);    //会员ids
            dto.PeoperMoney = Convert.ToInt32(PeoperMoney);
            dto.LowestMoney = Convert.ToInt32(LowestMoney);
            dto.TouUserid = Convert.ToInt32(TouUserid);
            dto.MoneySendType = (MnSdTypeEnum)Convert.ToInt32(MoneySendType);
            dto.MeetType = Convert.ToInt32(MeetType);
            dto.MeetNum = Convert.ToInt32(MeetNum);
            dto.FirstDate = Convert.ToDateTime(FirstDate);
            dto.MeetDate = MeetDate;
            dto.MeetTime = MeetTime;
            int result = _bll.CreateOrder(dto);
            if (result == 1)
            {
                return JsonFormat(new ExtJson { success = true, msg = "添加成功！" });
            }
            else {
                return JsonFormat(new ExtJson { success = false, msg = "添加失败！" });
            }
        }

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
    }
}

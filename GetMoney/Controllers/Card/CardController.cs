using DataAccess;
using GetMoney.Application.Card;
using GetMoney.Framework;
using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetMoney.Controllers.Card
{
    public class CardController : BaseController
    {
        ICardBll _bll = null;
        public CardController(ICardBll bll)
        {
            _bll = bll;
        }
        public ActionResult Index()
        {
            return View();
        }
        public void AddCard() {
            CardDto dto = new CardDto();
            dto.CardCode = Convert.ToInt32(Request["CardCode"].ToString());
            dto.CardName = Request["CardName"].ToString();
            dto.CardBankType = Convert.ToInt32(Request["CardBankType"].ToString());
            dto.CardUseType = Convert.ToInt32(Request["CardUseType"].ToString());
            dto.CardAmount = Convert.ToInt32(Request["CardAmount"].ToString());

            DateTime CardBillDate;
            if (DateTime.TryParse(Request["CardBillDate"].ToString(), out CardBillDate) == false)
                throw new ArgumentNullException();
            dto.CardBillDate = CardBillDate;
            dto.CardDelayDay = Convert.ToInt32(Request["CardDelayDay"].ToString());


            dto.CardInputDate = DateTime.Now;
            dto.Remark = Request["Remark"].ToString();
            int num = 1;
            try
            {
                _bll.AddCard(dto);
                if (num > 0)
                {
                    JsonFormat(new ExtJson { success = true, msg = "添加成功！" });
                }
                else {
                    JsonFormat(new ExtJson { success = true, msg = "添加失败！" });
                }
            }
            catch
            {
                JsonFormat(new ExtJson { success = true, msg = "添加失败！" });
            }
        }

        public ActionResult Test() {
            ViewBag.title = _bll.test();
            return View();
        }
    }
}

using Common;
using DataAccess;
using GetMoney.Application.Card;
using GetMoney.Framework;
using GetMoney.Model;
using Newtonsoft.Json;
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

        public string CardList() {
            string strsql = "SELECT [ID],[CardCode],[CardName],[CardBankType],[CardUseType],[CardAmount],[CardBillDate],[CardDelayDay],[CardInputDate],[Remark] FROM [dbo].[Cards] WHERE ID=1";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.SQLConnString, CommandType.Text, strsql);
            var list = new List<CardDto>();
            if (Utils.HasMoreRow(ds))
            {
                list.AddRange(from DataRow dr in ds.Tables[0].Rows select DataRowToModel(dr));
            }
            return JsonConvert.SerializeObject(new UIDataGrid(0, list));
        }

        private CardDto DataRowToModel(DataRow row)
        {
            var model = new CardDto();
            if (row != null)
            {
                if (!row["ID"].IsNullOrEmpty())
                {
                    model.ID = row["ID"].ToInt32();
                }
                if (!row["CardCode"].IsNullOrEmpty())
                {
                    model.CardCode = row["CardCode"].ToInt32();
                }
                if (!row["CardName"].IsNullOrEmpty())
                {
                    model.CardName = row["CardName"].ToString();
                }
                if (!row["CardBankType"].IsNullOrEmpty())
                {
                    model.CardBankType = row["CardBankType"].ToInt32();
                }
                if (!row["CardUseType"].IsNullOrEmpty())
                {
                    model.CardUseType = row["CardUseType"].ToInt32();
                }
                if (!row["CardAmount"].IsNullOrEmpty())
                {
                    model.CardAmount = row["CardAmount"].ToInt32();
                }
                if (!row["CardBillDate"].IsNullOrEmpty())
                {
                    model.CardBillDate = row["CardBillDate"].ToDateTime();
                }
                if (!row["CardDelayDay"].IsNullOrEmpty())
                {
                    model.CardDelayDay = row["CardDelayDay"].ToInt32();
                }
                if (!row["CardInputDate"].IsNullOrEmpty())
                {
                    model.CardInputDate = row["CardInputDate"].ToDateTime();
                }
                if (!row["Remark"].IsNullOrEmpty())
                {
                    model.Remark = row["Remark"].ToString();
                }
            }
            return model;
        }
        public ActionResult Test() {
            ViewBag.title = _bll.test();
            return View();
        }
    }
}

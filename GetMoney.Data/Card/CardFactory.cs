using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Data.Card
{
    public class CardFactory
    {
        public static Card Create(
            int CardCode,
            string CardName,
            int CardBankType,
            int CardUseType,
            int CardAmount,
            DateTime CardBillDate,
            int CardDelayDay,
            DateTime CardInputDate,
            string Remark)
        {
            return new Card
            {
                CardCode = CardCode,
                CardName = CardName,
                CardBankType = CardBankType,
                CardUseType = CardUseType,
                CardAmount = CardAmount,
                CardBillDate = CardBillDate,
                CardDelayDay = CardDelayDay,
                CardInputDate = CardInputDate,
                Remark = Remark
            };
        }
    }
}

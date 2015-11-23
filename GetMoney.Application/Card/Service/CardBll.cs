using GetMoney.Data.Card;
using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Application.Card
{
    public class CardBll : ICardBll
    {
        ICardRepository _repostory;
        public CardBll(ICardRepository repostory) {
            _repostory = repostory;
        }
        public string test() {
            return "我这里只是这么返回";
        }


        public void AddCard(CardDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException();
            var _unitOfWork = _repostory.UnitOfWork;
            var _build = CardFactory.Create(
                dto.CardCode,
                dto.CardName,
                dto.CardBankType,
                dto.CardUseType,
                dto.CardAmount,
                dto.CardBillDate,
                dto.CardDelayDay,
                dto.CardInputDate,
                dto.Remark
                );
            _repostory.Add(_build);
            _unitOfWork.Commit();
        }
        public List<CardDto> FindCard(int? id) {

            string strsql = "SELECT [CardCode],[CardName],[CardBankType],[CardUseType],[CardAmount],[CardBillDate],[CardDelayDay],[CardInputDate],[Remark] FROM [dbo].[Card] WHERE ID" + id;
            List<CardDto> list = new List<CardDto>();
            return list;
        }

    }
}

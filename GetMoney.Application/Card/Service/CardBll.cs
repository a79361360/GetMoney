using GetMoney.Data.Card;
using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GetMoney.Framework.Common;


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



        public bool RemoveCards(string[] ids)
        {
            bool result = false;
            if (ids.Length == 0)
            {
                throw new ArgumentNullException();
            }
            else
            {
                try
                {
                    foreach (var id in ids)
                    {
                        int gid = id.ToInt32();
                        var _dto = _repostory.Get(gid);
                        _repostory.Remove(_dto);
                        _repostory.UnitOfWork.Commit();
                    }
                    result = true;
                }catch{
                    result = false;
                    return result;
                }
            }
            return result;
        }
    }
}

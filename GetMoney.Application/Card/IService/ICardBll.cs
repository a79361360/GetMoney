using GetMoney.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Application.Card
{
    public interface ICardBll
    {
        string test();
        void AddCard(CardDto dto);
        bool RemoveCards(string[] dis);
        List<CardDto> FindCard(int? id);
    }
}

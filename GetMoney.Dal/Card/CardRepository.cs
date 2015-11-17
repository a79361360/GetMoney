using GetMoney.Data.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Dal
{
    public class CardRepository
   : Repository<Card>, ICardRepository
    {
        public CardRepository(DBContextOfUnitWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}

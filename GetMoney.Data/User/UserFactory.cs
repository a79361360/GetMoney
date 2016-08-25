using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Data.User
{
    public class UserFactory
    {
        public static User Create(
            string UserName,
            string NickName,
            int UserJb,
            string Identity,
            string Phone,
            string TxUrl,
            int State)
        {
            return new User
            {
                UserName=UserName,
                NickName=NickName,
                UserJb=UserJb,
                Identity =Identity,
                Phone=Phone,
                TxUrl=TxUrl,
                State = State
            };
        }
    }
}

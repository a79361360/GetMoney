using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Data.TUser
{
    public class TUserFactory
    {
        public static TUser Create(
            string UserName,
            string NickName,
            int UserJb,
            string IdentityNum,
            string Phone,
            string TxUrl,
            int State,
            DateTime Addtime)
        {
            return new TUser
            {
                UserName=UserName,
                NickName=NickName,
                UserJb=UserJb,
                IdentityNum = IdentityNum,
                Phone=Phone,
                TxUrl=TxUrl,
                State = State,
                Addtime = Addtime
            };
        }
    }
}

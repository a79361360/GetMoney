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
            string UserPwd,
            string BankPwd,
            string NickName,
            int UserJb,
            string TrueName,
            string IdentityNum,
            string Phone,
            string Regip,
            string TxUrl,
            int State,
            DateTime Addtime)
        {
            return new TUser
            {
                UserName=UserName,
                UserPwd=UserPwd,
                BankPwd=BankPwd,
                NickName=NickName,
                UserJb=UserJb,
                TrueName=TrueName,
                IdentityNum = IdentityNum,
                Phone=Phone,
                RegIP=Regip,
                TxUrl=TxUrl,
                State = State,
                Addtime = Addtime
            };
        }
    }
}

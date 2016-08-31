using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Model.Model
{
    public class TUserDto
    {
        public int id { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string UserPwd { get; set; }
        /// <summary>
        /// 账户密码
        /// </summary>
        public string BankPwd { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 用户金币
        /// </summary>
        public int UserJb { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 用户身份证号
        /// </summary>
        public string IdentityNum { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 注册IP
        /// </summary>
        public string RegIP { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string TxUrl { get; set; }
        /// <summary>
        /// 用户的状态(1正常用户,2临时用户,3禁用用户)
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime Addtime { get; set; }
    }
}

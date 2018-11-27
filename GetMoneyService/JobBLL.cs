using GetMoney.Application.WeiX;
using GetMoney.Common;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoneyService
{
    public class JobBLL : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                //WeiXBLL wxbll = new WeiXBLL();
                //wxbll.WxTemplate_Expire();
                string Host = CommonManager.WebObj.GetCurHttpsHost();
                CommonManager.WebObj.Get("https://gm.cf518.cn/Wx/WxTemplate_Expire");
            }
            catch (Exception ex)
            {

            }
        }
    }
}

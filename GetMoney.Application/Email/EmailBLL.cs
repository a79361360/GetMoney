using GetMoney.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace GetMoney.Application.Email
{
    public class EmailBLL
    {
        string formemail = "a79361360@163.com";
        string toemail = "592452713@qq.com";
        string emailhost = "smtp.163.com";
        public bool SendMail(string receiveUser, string sendTitle, string sendContent)
        {
            MailMessage mail = new MailMessage(new MailAddress(formemail), new MailAddress(receiveUser));
            mail.IsBodyHtml = true;
            mail.Subject = sendTitle;
            mail.Body = sendContent;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Priority = MailPriority.High;

            SmtpClient client = new SmtpClient();
            client.Host = emailhost;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(formemail, "000000abc");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                client.Send(mail);
                return true;
            }
            catch(System.Net.Mail.SmtpException er)
            {
                CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "公众号授权 WxTemplate_Expire："+ er.Message);
                return false;
            }
        }
    }
}

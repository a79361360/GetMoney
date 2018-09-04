using GetMoney.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace GetMoney.Application.Email
{
    public class EmailBLL
    {
        string formemail = "a79361360@sina.com";
        string formpwd = "000000abc";
        public bool SendMail(string receiveUser, string sendTitle, string sendContent)
        {
            string emailhost = GetEmailHost(formemail);    //取得邮箱SMTP服务器地址
            MailMessage mail = new MailMessage(new MailAddress(formemail), new MailAddress(receiveUser));
            mail.IsBodyHtml = true;
            mail.Subject = sendTitle;
            mail.Body = sendContent;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Priority = MailPriority.High;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            SmtpClient client = new SmtpClient();
            client.Host = emailhost;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(formemail, formpwd);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Port = 25;

            try
            {
                CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "公众号授权 开始发送邮件：");
                client.Send(mail);
                return true;
            }
            catch(System.Net.Mail.SmtpException er)
            {
                CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "公众号授权 WxTemplate_Expire："+ er.Message);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiveUser"></param>
        /// <param name="filepath">文件的绝对地址，如果放到服务器上，必须要先上传</param>
        /// <param name="sendTitle"></param>
        /// <param name="sendContent"></param>
        /// <returns></returns>
        public bool SendMail(string[] receiveUser, string[] filepath, string sendTitle, string sendContent) {
            string emailhost = GetEmailHost(formemail);    //取得邮箱SMTP服务器地址
            SmtpClient client = new SmtpClient();
            client.Host = emailhost;
            client.Port = 25;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(formemail, formpwd);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(formemail);
            foreach (string item in receiveUser) {
                mail.To.Add(new MailAddress(item));
            }
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;
            mail.Subject = sendTitle;
            mail.Attachments.Clear();
            foreach (string item in filepath) {
                mail.Attachments.Add(new Attachment(item, MediaTypeNames.Application.Octet));
            }
            mail.Body = sendContent;
            try
            {
                CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "SendMail 批量 开始发送邮件：");
                client.Send(mail);
                return true;
            }
            catch (System.Net.Mail.SmtpException er)
            {
                CommonManager.TxtObj.WriteLogs("/Logs/WxController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "SendMail 批量 WxTemplate_Expire：" + er.Message);
                return false;
            }

        }
        public string GetEmailHost(string email)
        {
            if (email.Contains("@163"))
                return "smtp.163.com";
            else if (email.Contains("@126"))
                return "mail.126.com";
            else if (email.Contains("@qq"))
                return "smtp.qq.com";
            else if (email.Contains("@gmail"))
                return "mail.google.com";
            else if (email.Contains("@sina"))
                return "smtp.sina.com";
            else if (email.Contains("@sohu"))
                return "mail.sohu.com";
            else if (email.Contains("@yeah"))
                return "www.yeah.net";
            else if (email.Contains("@tom"))
                return "mail.tom.com";
            else if (email.Contains("@139"))
                return "mail.139.com";
            return string.Empty;
        }
    }
}

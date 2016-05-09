using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Threading;
using Portal.Domain.Aggregates.UserAgg.Events;
using EasyDDD.Infrastructure.Crosscutting.Event;

namespace Portal.Applications.Events.Handler
{
    public class ResetedPasswordSendEmailHandler : IEventHandler<ResetedPasswordEvent>
    {
        public ResetedPasswordSendEmailHandler()
        {
        }


        public void Handle(ResetedPasswordEvent evnt)
        {
            //发起线程来做，节省时间
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
            {
                SendEmail(evnt);
            }));
        }

        private static void SendEmail(ResetedPasswordEvent evnt)
        {
            try
            {
                // 此处仅为演示，所以邮件内容很简单。可以根据自己的实际情况做一些复杂的邮件功能，比如
                // 使用邮件模板或者邮件风格等。
                SendEmail(evnt.UserEmail,
                    "出口易 用户生成密码通知",
                    string.Format(@"<p><strong>亲爱的用户，您好：</strong><br/></p>
<p style='text-indent: 2em;'>您的新密码已生成成功，新密码:【{0}】，新密码将在{1}分钟后生效。</p>
<p style='text-indent: 2em;'>------------------------------------------------------------------------------------------</p>
<p style='text-indent: 2em;'>此邮件为系统所发，请勿直接回复。</p>
<p style='text-indent: 2em;'><a target='_blank' href='http://www.chukou1.com/'>www.chukou1.com<wbr/></a></p>
<p style='text-indent: 2em;'>此致</p>
<p style='text-indent: 2em;'>出口易团队</p>", evnt.NewPassword, ConfigurationManager.AppSettings["ResetPasswordValidTime"] ?? "5"));
            }
            catch (Exception ex)
            {
                // 如遇异常，直接记Log
                //Log<ResetedPasswordSendEmailHandler>.LogError("密码重置成功后，给客户发送新密码邮件失败", ex);
            }
        }

        /// <summary>
        /// 向指定的邮件地址发送邮件。
        /// </summary>
        /// <param name="to">需要发送邮件的邮件地址。</param>
        /// <param name="subject">邮件主题。</param>
        /// <param name="content">邮件内容。</param>
        private static void SendEmail(string to, string subject, string content)
        {
            var emailHost = GetConfigValue("smtpHost");
            var emailSender = GetConfigValue("emailSender");
            var emailUsername = GetConfigValue("emailUsername");
            var emailPassword = GetConfigValue("emailPassword");

            MailMessage msg = new MailMessage(emailSender,
                to,
                subject,
                content);
            msg.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient(emailHost);
            smtpClient.Credentials = new NetworkCredential(emailUsername, emailPassword);
            smtpClient.Send(msg);
        }

        private static string GetConfigValue(string key)
        {
            var val = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(val))
                throw new InvalidOperationException(
                    string.Format("应用程序配置`{0}`未设置", key));
            return val;
        }
    }
}

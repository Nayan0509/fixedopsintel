using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using WoodenAutomative.Domain.Models;
using WoodenAutomative.EntityFramework.Interfaces.Services;

namespace WoodenAutomative.EntityFramework.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        public EmailRepository()
        {
            
        }
        public bool SendEmail(EmailData emailData)
        {
            using (var emailClient = new SmtpClient())
            {
                try
                {
                    var emailMessage = new MimeMessage();
                    MailboxAddress emailFrom = new MailboxAddress("woodenAutomative", "trackgaddireports1@gmail.com");
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = MailboxAddress.Parse(emailData.EmailToId);
                    emailMessage.To.Add(emailTo);
                    emailMessage.Subject = emailData.EmailSubject;

                    //BodyBuilder emailBody = new BodyBuilder();

                    //emailBody.HtmlBody = emailData.EmailBody;
                    //emailMessage.Body = emailBody.ToMessageBody();
                    emailMessage.Body = new TextPart(TextFormat.Html) { Text = emailData.EmailBody };

                    emailClient.Connect("smtp.gmail.com", 587, true);
                    emailClient.Authenticate("trackgaddireports1@gmail.com", "txqrdkvxwrduspwy");
                    emailClient.Send(emailMessage);

                    return true;
                }
                catch (Exception ex)
                {
                    //Log Exception Details
                    return false;
                }
                finally
                {
                    emailClient.Disconnect(true);
                    emailClient.Dispose();
                }
            }
        }
    }
}

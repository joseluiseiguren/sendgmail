using System.Net;
using System.Net.Mail;

namespace jle.lib.gmail.send
{
    public class SendGmail
    {
        private readonly string _fromEmail;
        private readonly string _fromPassword;
        private readonly string _displayName;

        public SendGmail(string fromEmail, string fromPassword, string displayName)
        {
            _fromPassword = fromPassword;
            _fromEmail = fromEmail;
            _displayName = displayName;
        }

        public async Task SendAsync(string toEmail, string toName, string subject, string htmlBody, string? attachmentFileName = null, Stream? attachementContentStream = null)
        {
            var fromAddress = new MailAddress(_fromEmail, _displayName);

            var toAddress = new MailAddress(toEmail, toName);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, _fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            })
            {
                if (attachmentFileName != null && attachementContentStream != null)
                {
                    var contentType = new System.Net.Mime.ContentType();
                    contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;
                    contentType.Name = attachmentFileName;
                    message.Attachments.Add(new Attachment(attachementContentStream, contentType));
                }

                await smtp.SendMailAsync(message); 
            }
        }
    }
}
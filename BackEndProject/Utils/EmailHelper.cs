using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BackEndProject.Utils;
public class EmailHelper
{
    public async Task SendEmailAsync(MailRequestVM mailRequestVM)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(Constants.mail);
        email.To.Add(MailboxAddress.Parse(mailRequestVM.ToEmail));
        email.Subject = mailRequestVM.Subject;
        var builder = new BodyBuilder();
        if (mailRequestVM.Attachments != null)
        {
            byte[] fileBytes;
            foreach (var file in mailRequestVM.Attachments)
            {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                }
            }
        }
        builder.HtmlBody = mailRequestVM.Body;
        email.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        smtp.Connect(Constants.host, Constants.port, SecureSocketOptions.StartTls);
        smtp.Authenticate(Constants.mail, Constants.password);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }
}

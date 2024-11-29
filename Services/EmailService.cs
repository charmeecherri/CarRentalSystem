using CarRentalSystem.Utilities;
using MailKit.Security;
using MimeKit;
        

namespace CarRentalSystem.Services
{
        public class EmailService : IEmailService
        {

            public async Task SendEmailAsync(Message message)
            {
                MimeMessage mailMessage = CreateEmailMessage(message);
                await SendAsync(mailMessage);
            }

            private MimeMessage CreateEmailMessage(Message message)
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(EmailConfiguration.FromDisplayName, EmailConfiguration.From));
                emailMessage.To.AddRange(message.To);
                emailMessage.Subject = message.Subject;

                var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<p style = 'color:black'>{0}</p>", message.Content) };

                if (message.Attachments != null && message.Attachments.Any())
                {
                    byte[] fileBytes;
                    foreach (var attachment in message.Attachments)
                    {
                        using (var ms = new MemoryStream())
                        {
                            attachment.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }

                        bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                    }
                }

                emailMessage.Body = bodyBuilder.ToMessageBody();
                return emailMessage;
            }

            private async Task SendAsync(MimeMessage mailMessage)
            {
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    try
                    {
                        await client.ConnectAsync(EmailConfiguration.SmtpServer, EmailConfiguration.Port, SecureSocketOptions.StartTls);
                        await client.AuthenticateAsync(EmailConfiguration.UserName, EmailConfiguration.Password);

                        await client.SendAsync(mailMessage);
                    }
                    catch
                    {
                        //log an error message or throw an exception, or both.
                        throw;
                    }
                    finally
                    {
                        await client.DisconnectAsync(true);
                        client.Dispose();
                    }
                }
            }
        }
}




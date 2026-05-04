using Microsoft.Extensions.Options;
using ProdjectApi.Configuration;
using System.Net;
using System.Net.Mail;

namespace ProdjectApi.Service
{
    public class SmtpEmailService
    {
        private readonly SmtpSettings _settings;

        public SmtpEmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _settings = smtpSettings.Value;
        }

        public async Task SendVerificationEmail(string email, string code)
        {
            var body = $@"
            Для подтверждения email введите ниже представленный код:
            
            Ваш код подтверждения: <b>{code}</b>
            ";

            var from = new MailAddress("apikp_2@mail.ru");
            var to = new MailAddress(email);
            try
            {
            using var message = new MailMessage
            {
                From = from,
                Subject = "Подтвердите email",
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(to);

            using var smtp = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort)
            {
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                EnableSsl = _settings.EnableSsl,
                Timeout = 10000
            };

            await smtp.SendMailAsync(message);
        }
                catch (SmtpException smtpEx)
    {
                Console.WriteLine($"SMTP ошибка: {smtpEx.Message} (статус: {smtpEx.StatusCode})");
            }
    catch (Exception ex)
    {
                Console.WriteLine($"Ошибка отправки письма: {ex.Message}");
            }
        }
    }
}

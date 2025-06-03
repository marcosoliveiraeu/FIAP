
using FiapCloudGames.Application.Services.Interfaces;
using FiapCloudGames.Application.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace FiapCloudGames.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _remetente;

        public EmailService(IOptions<EmailSettings> options)
        {
            var settings = options.Value;
            _remetente = settings.Remetente;
            _smtpClient = new SmtpClient(settings.Host, settings.Port)
            {
                Credentials = new NetworkCredential(settings.Usuario, settings.Senha),
                EnableSsl = true
            };
        }

        public async Task EnviarEmailAsync(string destinatario, string assunto, string mensagem)
        {
            var mail = new MailMessage(_remetente, destinatario, assunto, mensagem)
            {
                IsBodyHtml = true
            };
            await _smtpClient.SendMailAsync(mail);
        }
    }
}


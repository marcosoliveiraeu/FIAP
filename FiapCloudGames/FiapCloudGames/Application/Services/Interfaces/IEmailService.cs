namespace FiapCloudGames.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task EnviarEmailAsync(string destinatario, string assunto, string mensagem);
    }
}

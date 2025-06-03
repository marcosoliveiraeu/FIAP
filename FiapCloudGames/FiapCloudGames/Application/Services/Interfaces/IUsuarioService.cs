using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Enuns;

namespace FiapCloudGames.Application.Services.Interfaces
{
    public interface IUsuarioService
    {

        Task<bool> VerificaEmailDupplicado(string email);

        Task CadastrarUsuarioAsync(string nome, string email, string senha);

        bool SenhaValida(string senha);

        Task<IEnumerable<Usuario>> ObterTodosAsync();

        Task AtualizarAsync(AtualizarUsuarioDto usuario);
        Task RemoverAsync(Guid id);

        Task<Usuario?> ObterPorIdAsync(Guid id);
    }
}

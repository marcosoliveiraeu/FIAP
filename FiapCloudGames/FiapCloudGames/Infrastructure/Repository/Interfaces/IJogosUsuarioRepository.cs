using FiapCloudGames.Domain.Entities;

namespace FiapCloudGames.Infrastructure.Repository.Interfaces
{
    public interface IJogosUsuarioRepository
    {
        Task AdicionarAsync(JogosUsuario jogoUsuario);

        Task<JogosUsuario> ObterPorIdAsync(Guid id);

        Task<IEnumerable<JogosUsuario>> ObterTodosAsync();
        Task<IEnumerable<JogosUsuario>> ObterPorJogoIdAsync(Guid jogoId);
        Task<IEnumerable<JogosUsuario>> ObterPorUsuarioIdAsync(Guid usuarioId);
        Task RemoverAsync(JogosUsuario jogosUsuario);
        Task<bool> ExisteAsync(Guid usuarioId, Guid jogoId);
    }
}

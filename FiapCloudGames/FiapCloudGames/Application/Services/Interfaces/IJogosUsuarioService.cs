using FiapCloudGames.Domain.Entities;

namespace FiapCloudGames.Application.Services.Interfaces
{
    public interface IJogosUsuarioService
    {
        Task AdicionarAsync(Guid usuarioId, Guid jogoId);
        Task<JogosUsuario> ObterPorIdAsync(Guid id);
        Task<IEnumerable<JogosUsuario>> ObterTodosAsync();

        Task<IEnumerable<JogosUsuario>> ObterPorUsuarioIdAsync(Guid usuarioId);

        Task<IEnumerable<JogosUsuario>> ObterPorJogoIdAsync(Guid jogoId);

        Task RemoverAsync(Guid id);

    }
}

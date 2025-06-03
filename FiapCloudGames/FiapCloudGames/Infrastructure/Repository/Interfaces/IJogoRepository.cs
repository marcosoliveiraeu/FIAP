using FiapCloudGames.Domain.Entities;

namespace FiapCloudGames.Infrastructure.Repository.Interfaces
{
    public interface IJogoRepository
    {
        Task<IEnumerable<Jogo>> ObterTodosAsync();
        Task<Jogo?> ObterPorIdAsync(Guid id);
        Task<Jogo?> ObterPorTituloAsync(string titulo);
        Task AdicionarAsync(Jogo jogo);
        Task AtualizarAsync(Jogo jogo);
        Task RemoverAsync(Guid id);
        Task<bool> ExistePorTituloAsync(string titulo, Guid? ignorarId = null);
    }
}

using FiapCloudGames.Domain.Entities;

namespace FiapCloudGames.Infrastructure.Repository.Interfaces
{
    public interface IPromocaoJogoRepository
    {
        Task AdicionarAsync(PromocaoJogo promocaoJogo);
        Task RemoverAsync(Guid id);
        Task<PromocaoJogo> ObterPorIdAsync(Guid id);
        Task<IEnumerable<PromocaoJogo>> ObterPorJogoIdAsync(Guid jogoId);
        Task<IEnumerable<PromocaoJogo>> ObterPorPromocaoIdAsync(Guid promocaoId);
        Task<bool> JogoEstaEmOutraPromocaoAtivaAsync(Guid jogoId);
        Task<IEnumerable<PromocaoJogo>> ObterTodosAsync();
        Task AtualizarAsync(PromocaoJogo promocaoJogo);

        Task<PromocaoJogo> ObterPromocaoAtivaPorJogoIdAsync(Guid id);
    }
}

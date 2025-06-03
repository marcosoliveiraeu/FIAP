using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Domain.Entities;

namespace FiapCloudGames.Application.Services.Interfaces
{
    public interface IPromocaoJogoService
    {
        Task AdicionarAsync(AdicionarJogoNaPromocaoDto dto);
        Task RemoverAsync(Guid id);
        Task<PromocaoJogo> ObterPorIdAsync(Guid id);
        Task<IEnumerable<PromocaoJogo>> ObterPorJogoIdAsync(Guid jogoId);
        Task<IEnumerable<PromocaoJogo>> ObterPorPromocaoIdAsync(Guid promocaoId);
        Task<IEnumerable<PromocaoJogo>> ObterTodosAsync();

    }
}

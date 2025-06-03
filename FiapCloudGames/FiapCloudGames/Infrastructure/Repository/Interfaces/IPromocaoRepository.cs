using FiapCloudGames.Domain.Entities;

namespace FiapCloudGames.Infrastructure.Repository.Interfaces
{
    public interface IPromocaoRepository
    {
        Task<List<Promocao>> ObterTodosAsync();
        Task<Promocao> ObterPorIdAsync(Guid id);
        Task AdicionarAsync(Promocao promocao);
        Task AtualizarAsync(Promocao promocao);
        Task RemoverAsync(Guid id);
        Task<bool> PromocaoEstaAtivaAsync(Guid promocaoId);


    }
}

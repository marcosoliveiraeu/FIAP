using FiapCloudGames.Domain.Entities;

namespace FiapCloudGames.Application.Services.Interfaces
{
    public interface IPromocaoService
    {
        Task<List<Promocao>> ObterTodosAsync();
        Task<Promocao> ObterPorIdAsync(Guid id);
        Task AdicionarAsync(Promocao promocao);
        Task AtualizarAsync(Promocao promocao);
        Task RemoverAsync(Guid id);
    }
}

using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Enuns;

namespace FiapCloudGames.Application.Services.Interfaces
{
    public interface IJogoService
    {
        Task<IEnumerable<JogoResponseDto>> ObterTodosAsync();
        Task<JogoResponseDto?> ObterPorIdAsync(Guid id);
        Task<JogoResponseDto?> ObterPorTituloAsync(string titulo);
        Task<Jogo> AdicionarAsync(CriarJogoDto jogo);
        Task AtualizarAsync(System.Guid id, string titulo, CategoriaJogo categoria, decimal preco, DateTime dataLancamento);
        Task RemoverAsync(Guid id);

    }
}

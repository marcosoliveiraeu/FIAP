using FiapCloudGames.Application.Services.Interfaces;
using FiapCloudGames.Domain.Enuns;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Exceptions;
using FiapCloudGames.Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using FiapCloudGames.Infrastructure.Repository;

namespace FiapCloudGames.Application.Services
{
    public class PromocaoService : IPromocaoService
    {

        private IPromocaoRepository _promocaoRepository;
        private IPromocaoJogoRepository _promocaoJogoRepository;

        public PromocaoService(IPromocaoRepository promocaoRepository, IPromocaoJogoRepository promocaoJogoRepository)
        {
            _promocaoRepository = promocaoRepository;
            _promocaoJogoRepository = promocaoJogoRepository;
        }

        public async Task<List<Promocao>> ObterTodosAsync()
        {
            return await _promocaoRepository.ObterTodosAsync();
        }

        public async Task<Promocao> ObterPorIdAsync(Guid id)
        {
            var promocao = await _promocaoRepository.ObterPorIdAsync(id);
            if (promocao == null)
                throw new KeyNotFoundException("Promoção não encontrada.");

            return promocao;
        }

        public async Task AdicionarAsync(Promocao promocao)
        {
            promocao.Id = Guid.NewGuid();
            promocao.DataInclusao = DateTime.UtcNow;
            promocao.DataAtualizacao = DateTime.UtcNow;

            await _promocaoRepository.AdicionarAsync(promocao);
        }

        public async Task AtualizarAsync(Promocao promocao)
        {
            var promocaoExistente = await _promocaoRepository.ObterPorIdAsync(promocao.Id);

            if (promocaoExistente == null)
                throw new KeyNotFoundException("Promoção não encontrada.");

            var percentualAlterado = promocaoExistente.PercentualDesconto != promocao.PercentualDesconto;

            if (percentualAlterado)
            {   
                // Obter os jogos relacionados a essa promoção
                var promocaoJogos = await _promocaoJogoRepository.ObterPorPromocaoIdAsync(promocao.Id);

                foreach (var pj in promocaoJogos)
                {
                    pj.ValorComDesconto = Math.Round(
                        pj.ValorOriginal - (pj.ValorOriginal * promocao.PercentualDesconto / 100), 2
                    );

                    await _promocaoJogoRepository.AtualizarAsync(pj);
                }

            }

            promocaoExistente.Titulo = promocao.Titulo;
            promocaoExistente.Descricao = promocao.Descricao;
            promocaoExistente.Status = promocao.Status;
            promocaoExistente.PercentualDesconto = promocao.PercentualDesconto;
            promocaoExistente.DataValidade = promocao.DataValidade;
            promocaoExistente.DataAtualizacao = DateTime.UtcNow;

            await _promocaoRepository.AtualizarAsync(promocao);
        }

        public async Task RemoverAsync(Guid id)
        {
            var promocao = await _promocaoRepository.ObterPorIdAsync(id);
            if (promocao == null)
                throw new KeyNotFoundException("Promoção não encontrada.");

            await _promocaoRepository.RemoverAsync(id);
        }

    }
}

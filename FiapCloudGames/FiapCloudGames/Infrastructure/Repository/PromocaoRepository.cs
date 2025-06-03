using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Enuns;
using FiapCloudGames.Infrastructure.Data;
using FiapCloudGames.Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure.Repository
{
    public class PromocaoRepository : IPromocaoRepository
    {
        private readonly DbContextFCG _dbContext;

        public PromocaoRepository(DbContextFCG dbContextFCG)
        {
            _dbContext = dbContextFCG;
        }

        public async Task<List<Promocao>> ObterTodosAsync()
        {
            return await _dbContext.Promocoes
                .Include(p => p.Jogos)
                .ThenInclude(pj => pj.Jogo)
                .ToListAsync();
        }

        public async Task<Promocao> ObterPorIdAsync(Guid id)
        {
            return await _dbContext.Promocoes
                .Include(p => p.Jogos) 
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AdicionarAsync(Promocao promocao)
        {
            await _dbContext.Promocoes.AddAsync(promocao);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Promocao promocao)
        {
            var promocaoExistente = await _dbContext.Promocoes
                .FirstOrDefaultAsync(p => p.Id == promocao.Id);

            if (promocaoExistente == null)
                throw new Exception("Promoção não encontrada.");

            promocaoExistente.Titulo = promocao.Titulo;
            promocaoExistente.Descricao = promocao.Descricao;
            promocaoExistente.Status = promocao.Status;
            promocaoExistente.PercentualDesconto = promocao.PercentualDesconto;
            promocaoExistente.DataValidade = promocao.DataValidade;
            promocaoExistente.DataAtualizacao = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoverAsync(Guid id)
        {
            var promocao = await _dbContext.Promocoes.FindAsync(id);

            if (promocao == null)
                throw new Exception("Promoção não encontrada.");

            _dbContext.Promocoes.Remove(promocao);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> PromocaoEstaAtivaAsync(Guid promocaoId)
        {
            return await _dbContext.Promocoes
                .AnyAsync(p => p.Id == promocaoId && p.Status == StatusPromocao.ATIVA);
        }



    }
}

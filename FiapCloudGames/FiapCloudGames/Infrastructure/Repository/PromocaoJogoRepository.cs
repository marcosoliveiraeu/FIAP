using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Enuns;
using FiapCloudGames.Infrastructure.Data;
using FiapCloudGames.Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure.Repository
{
    public class PromocaoJogoRepository : IPromocaoJogoRepository
    {
        private readonly DbContextFCG _dbContext;

        public PromocaoJogoRepository(DbContextFCG dbContextFCG)
        {
            _dbContext = dbContextFCG;
        }

        public async Task AdicionarAsync(PromocaoJogo promocaoJogo)
        {
            _dbContext.PromocaoJogos.Add(promocaoJogo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoverAsync(Guid id)
        {
            var entidade = await _dbContext.PromocaoJogos.FindAsync(id);
            if (entidade != null)
            {
                _dbContext.PromocaoJogos.Remove(entidade);
                await _dbContext.SaveChangesAsync();
            }
        }                

        public async Task<PromocaoJogo> ObterPorIdAsync(Guid id)
        {
            return await _dbContext.PromocaoJogos
               .Include(pj => pj.Promocao)
               .Include(pj => pj.Jogo)
               .FirstOrDefaultAsync(pj => pj.Id == id);
        }

        public async Task<IEnumerable<PromocaoJogo>> ObterPorJogoIdAsync(Guid jogoId)
        {
            return await _dbContext.PromocaoJogos
                .Include(pj => pj.Jogo)
                .Include(pj => pj.Promocao)
                .Where(pj => pj.JogoId == jogoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<PromocaoJogo>> ObterPorPromocaoIdAsync(Guid promocaoId)
        {
            return await _dbContext.PromocaoJogos
                .Include(pj => pj.Jogo)
                .Include(pj => pj.Promocao)
                .Where(pj => pj.PromocaoId == promocaoId)
                .ToListAsync();
        }

        public async Task<bool> JogoEstaEmOutraPromocaoAtivaAsync(Guid jogoId)
        {
            return await _dbContext.PromocaoJogos
            .AnyAsync(pj => pj.JogoId == jogoId &&
                            pj.Promocao.Status == StatusPromocao.ATIVA);
        }

        public async Task<IEnumerable<PromocaoJogo>> ObterTodosAsync()
        {
            return await _dbContext.PromocaoJogos
                .Include(pj => pj.Jogo)
                .Include(pj => pj.Promocao)
                .ToListAsync();
        }

        public async Task AtualizarAsync(PromocaoJogo promocaoJogo)
        {
            _dbContext.PromocaoJogos.Update(promocaoJogo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PromocaoJogo> ObterPromocaoAtivaPorJogoIdAsync(Guid jogoId)
        {
            var result =  await _dbContext.PromocaoJogos
                                    .Include(pj => pj.Jogo)
                                    .Include(pj => pj.Promocao)
                                    .Where(pj => pj.JogoId == jogoId &&
                                                 pj.Promocao.Status == StatusPromocao.ATIVA)
                                    .FirstOrDefaultAsync(); 

            return result;
        }
    }
}

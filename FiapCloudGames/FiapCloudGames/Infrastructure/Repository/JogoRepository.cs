using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Infrastructure.Data;
using FiapCloudGames.Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure.Repository
{
    public class JogoRepository : IJogoRepository
    {
        private readonly DbContextFCG _dbContext;

        public JogoRepository(DbContextFCG dbContextFCG)
        {
            _dbContext = dbContextFCG;
        }

        public async Task<IEnumerable<Jogo>> ObterTodosAsync()
        {
            return await _dbContext.Jogos.AsNoTracking().ToListAsync();
        }

        public async Task<Jogo?> ObterPorIdAsync(Guid id)
        {
            return await _dbContext.Jogos.FindAsync(id);
        }

        public async Task<Jogo?> ObterPorTituloAsync(string titulo)
        {
            return await _dbContext.Jogos
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Titulo.ToLower() == titulo.ToLower());
        }

        public async Task AdicionarAsync(Jogo jogo)
        {
            await _dbContext.Jogos.AddAsync(jogo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Jogo jogo)
        {
            _dbContext.Jogos.Update(jogo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoverAsync(Guid id)
        {
            var jogo = await ObterPorIdAsync(id);
            if (jogo != null)
            {
                _dbContext.Jogos.Remove(jogo);
                await _dbContext.SaveChangesAsync();
            }
        }


        public async Task<bool> ExistePorTituloAsync(string titulo, Guid? ignorarId = null)
        {
            return await _dbContext.Jogos
                .AnyAsync(j => j.Titulo.ToLower() == titulo.ToLower() && (!ignorarId.HasValue || j.Id != ignorarId.Value));
        }
    }
}

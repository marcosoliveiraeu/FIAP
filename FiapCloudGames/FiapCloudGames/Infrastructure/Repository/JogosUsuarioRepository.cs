using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Infrastructure.Data;
using FiapCloudGames.Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure.Repository
{
    public class JogosUsuarioRepository : IJogosUsuarioRepository
    {
        private readonly DbContextFCG _dbContext;

        public JogosUsuarioRepository(DbContextFCG dbContextFCG)
        {
            _dbContext = dbContextFCG;
        }

        public async Task AdicionarAsync(JogosUsuario jogoUsuario)
        {
            await _dbContext.JogosUsuarios.AddAsync(jogoUsuario);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<JogosUsuario> ObterPorIdAsync(Guid id)
        {
            return await _dbContext.JogosUsuarios
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task<IEnumerable<JogosUsuario>> ObterPorJogoIdAsync(Guid jogoId)
        {
            return await _dbContext.JogosUsuarios
                .Where(j => j.JogoId == jogoId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<JogosUsuario>> ObterPorUsuarioIdAsync(Guid usuarioId)
        {
            return await _dbContext.JogosUsuarios
                .Where(j => j.UsuarioId == usuarioId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<JogosUsuario>> ObterTodosAsync()
        {
            return await _dbContext.JogosUsuarios
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task RemoverAsync(JogosUsuario jogosUsuario)
        {
           
            if (jogosUsuario != null)
            {
                _dbContext.JogosUsuarios.Remove(jogosUsuario);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteAsync(Guid usuarioId, Guid jogoId)
        {
            return await _dbContext.JogosUsuarios
                .AnyAsync(j => j.UsuarioId == usuarioId && j.JogoId == jogoId);
        }
    }
}

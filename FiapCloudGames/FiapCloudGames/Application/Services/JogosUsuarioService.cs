using FiapCloudGames.Application.Services.Interfaces;
using FiapCloudGames.Domain.Enuns;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Exceptions;
using FiapCloudGames.Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FiapCloudGames.Application.Services
{
    public class JogosUsuarioService : IJogosUsuarioService
    {
        private readonly IJogosUsuarioRepository _jogosUsuarioRepository;
        private readonly IJogoRepository _jogoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPromocaoJogoRepository _promocaoJogoRepository;

        public JogosUsuarioService(
            IJogosUsuarioRepository jogosUsuarioRepository,
            IUsuarioRepository usuarioRepository,
            IJogoRepository jogoRepository,
            IPromocaoJogoRepository promocaoJogoRepository)
        {
            _jogosUsuarioRepository = jogosUsuarioRepository;
            _usuarioRepository = usuarioRepository;
            _jogoRepository = jogoRepository;
            _promocaoJogoRepository = promocaoJogoRepository;
        }

        public async Task AdicionarAsync(Guid usuarioId, Guid jogoId)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(jogoId)
                ?? throw new NotFoundException("Jogo não encontrado.");

            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId)
                ?? throw new NotFoundException("Usuário não encontrado.");

            var existe = await _jogosUsuarioRepository.ExisteAsync(usuarioId, jogoId);
            if (existe)
                throw new BusinessException("O usuário já possui este jogo.");


            var promocao = await _promocaoJogoRepository.ObterPromocaoAtivaPorJogoIdAsync(jogoId);

            var precoPago = promocao != null ? promocao.ValorComDesconto : jogo.Preco;

            var jogosUsuario = new JogosUsuario
            {
                Id = Guid.NewGuid(),
                JogoId = jogoId,
                UsuarioId = usuarioId,
                DataAquisicao = DateTime.UtcNow,
                PrecoPago = precoPago
            };

            await _jogosUsuarioRepository.AdicionarAsync(jogosUsuario);
        }

        public async Task<JogosUsuario> ObterPorIdAsync(Guid id)
        {
            var jogoUsuario = await _jogosUsuarioRepository.ObterPorIdAsync(id);
            if (jogoUsuario == null)
                throw new NotFoundException("Registro de jogo do usuário não encontrado.");

            return jogoUsuario;
        }

        public async Task<IEnumerable<JogosUsuario>> ObterPorJogoIdAsync(Guid jogoId)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(jogoId);
            if (jogo == null)
                throw new NotFoundException("Jogo não encontrado.");

            var result =  await _jogosUsuarioRepository.ObterPorJogoIdAsync(jogoId);
            if(result == null)
                throw new NotFoundException("Nenhum usuário está relacionado a esse jogo.");

            return result;
        }

        public async Task<IEnumerable<JogosUsuario>> ObterPorUsuarioIdAsync(Guid usuarioId)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            if (usuario == null)
                throw new NotFoundException("Usuário não encontrado.");

            var result =  await _jogosUsuarioRepository.ObterPorUsuarioIdAsync(usuarioId);
            if(result == null)
                throw new NotFoundException("Nenhum jogo foi encontrado para esse usuário.");

            return result;
        }

        public async Task<IEnumerable<JogosUsuario>> ObterTodosAsync()
        {
            var jogosUsuarios =  await _jogosUsuarioRepository.ObterTodosAsync();
            if (jogosUsuarios == null)
                throw new NotFoundException("Nenhum registro a ser retornado.");

            return jogosUsuarios;

        }

        public async  Task RemoverAsync(Guid id)
        {
            var entidade = await _jogosUsuarioRepository.ObterPorIdAsync(id);
            if (entidade == null)
                throw new NotFoundException("Registro de jogo do usuário não encontrado.");

            await _jogosUsuarioRepository.RemoverAsync(entidade);
        }
    }

}

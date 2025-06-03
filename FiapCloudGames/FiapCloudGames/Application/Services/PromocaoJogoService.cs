using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Application.Services.Interfaces;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Enuns;
using FiapCloudGames.Domain.Exceptions;
using FiapCloudGames.Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FiapCloudGames.Application.Services
{
    public class PromocaoJogoService : IPromocaoJogoService
    {

        private readonly IPromocaoJogoRepository _promocaoJogoRepository;
        private readonly IPromocaoRepository _promocaoRepository;
        private readonly IJogoRepository _jogoRepository;
        private readonly IEmailService _emailService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJogosUsuarioRepository _jogosUsuarioRepository;


        public PromocaoJogoService(
             IPromocaoJogoRepository promocaoJogoRepository,
             IPromocaoRepository promocaoRepository,
             IJogoRepository jogoRepository,
             IEmailService emailService,
             IUsuarioRepository usuarioRepository,
             IJogosUsuarioRepository jogosUsuarioRepository)
        {
            _promocaoJogoRepository = promocaoJogoRepository;
            _promocaoRepository = promocaoRepository;
            _jogoRepository = jogoRepository;
            _emailService = emailService;
            _usuarioRepository = usuarioRepository;
            _jogosUsuarioRepository = jogosUsuarioRepository;
        }

        public async Task AdicionarAsync(AdicionarJogoNaPromocaoDto promocaoJogoDto)
        {
            // Verificar se a promoção existe e está ativa
            var promocao = await _promocaoRepository.ObterPorIdAsync(promocaoJogoDto.PromocaoId)
                ?? throw new NotFoundException("Promoção não encontrada.");

            if (promocao.Status != Domain.Enuns.StatusPromocao.ATIVA)
                throw new BusinessException("A promoção não está ativa.");

            // Verificar se o jogo existe
            var jogo = await _jogoRepository.ObterPorIdAsync(promocaoJogoDto.JogoId)
                ?? throw new NotFoundException("Jogo não encontrado.");

            // Verificar se o jogo já está em outra promoção ativa
            var jogoEmOutraPromocao = await _promocaoJogoRepository.JogoEstaEmOutraPromocaoAtivaAsync(promocaoJogoDto.JogoId);
            if (jogoEmOutraPromocao)
                throw new BusinessException("O jogo já está vinculado a uma promoção ativa.");

            // Calcular valor com desconto
            var valorComDesconto = jogo.Preco - (jogo.Preco * (promocao.PercentualDesconto / 100));

            var promocaoJogo = new PromocaoJogo
            {
                Id = Guid.NewGuid(),
                PromocaoId = promocaoJogoDto.PromocaoId,
                JogoId = promocaoJogoDto.JogoId,
                ValorOriginal = jogo.Preco,
                ValorComDesconto = Math.Round(valorComDesconto, 2),
                DataInclusao = DateTime.UtcNow
            };

            await _promocaoJogoRepository.AdicionarAsync(promocaoJogo);

            if(promocaoJogoDto.EnviarNotificacao)
            {
                var todosUsuarios = await _usuarioRepository.ObterTodosAsync();

                // Buscar todos os usuários que possuem o jogo
                var usuariosComJogo = (await _jogosUsuarioRepository.ObterPorJogoIdAsync(jogo.Id))
                    .Select(ju => ju.UsuarioId)
                    .ToHashSet();

                // Filtrar usuários que NÃO possuem o jogo
                var usuariosParaNotificar = todosUsuarios
                    .Where(u => !usuariosComJogo.Contains(u.Id))
                    .ToList();

                var assunto = $"O jogo {jogo.Titulo} está em promoção!";
                var mensagem = $"Aproveite! O jogo <b>{jogo.Titulo}</b> está em promoção por R$ {valorComDesconto:F2}.";

                foreach (var usuario in usuariosParaNotificar)
                {
                    await _emailService.EnviarEmailAsync(usuario.Email, assunto, mensagem);
                }

            }


        }

        public async Task RemoverAsync(Guid id)
        {
            var promocaoJogo = await _promocaoJogoRepository.ObterPorIdAsync(id);
            if (promocaoJogo == null)
                throw new NotFoundException("Associação de promoção com jogo não encontrada.");

            await _promocaoJogoRepository.RemoverAsync(id);
        }

        public async Task<PromocaoJogo> ObterPorIdAsync(Guid id)
        {
            var promocaoJogo = await _promocaoJogoRepository.ObterPorIdAsync(id);
            if (promocaoJogo == null)
                throw new NotFoundException("Associação de promoção com jogo não encontrada.");

            return promocaoJogo;
        }

        public async Task<IEnumerable<PromocaoJogo>> ObterPorJogoIdAsync(Guid jogoId)
        {
            return await _promocaoJogoRepository.ObterPorJogoIdAsync(jogoId);
        }

        public async Task<IEnumerable<PromocaoJogo>> ObterPorPromocaoIdAsync(Guid promocaoId)
        {
            return await _promocaoJogoRepository.ObterPorPromocaoIdAsync(promocaoId);
        }

        public async Task<IEnumerable<PromocaoJogo>> ObterTodosAsync()
        {
            return await _promocaoJogoRepository.ObterTodosAsync();
        }

    }



}


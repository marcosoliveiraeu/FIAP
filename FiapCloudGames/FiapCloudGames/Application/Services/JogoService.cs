using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Application.Services.Interfaces;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Enuns;
using FiapCloudGames.Domain.Exceptions;
using FiapCloudGames.Infrastructure.Repository;
using FiapCloudGames.Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Globalization;

namespace FiapCloudGames.Application.Services
{
    public class JogoService : IJogoService
    {

        private IJogoRepository _jogoRepository;
        private IPromocaoJogoRepository _promocaoJogoRepository;
        private IPromocaoRepository _promocaoRepository;
        private IUsuarioRepository _usuarioRepository;
        private IEmailService _emailService;

        public JogoService( IJogoRepository jogoRepository,
                            IPromocaoJogoRepository promocaoJogoRepository,
                            IPromocaoRepository promocaoRepository,
                            IUsuarioRepository usuarioRepository,
                            IEmailService emailService)
        {
            _jogoRepository = jogoRepository;
            _promocaoJogoRepository = promocaoJogoRepository;
            _promocaoRepository = promocaoRepository;
            _usuarioRepository = usuarioRepository;
            _emailService = emailService;
        }

        public async Task<IEnumerable<JogoResponseDto>> ObterTodosAsync()
        {

            var jogos = await _jogoRepository.ObterTodosAsync();
            var promocaoJogos = await _promocaoJogoRepository.ObterTodosAsync();

            var response = jogos.Select(jogo =>
            {
                var promocaoJogo = promocaoJogos
                    .FirstOrDefault(pj => pj.JogoId == jogo.Id && pj.Promocao.Status == StatusPromocao.ATIVA);

                return new JogoResponseDto
                {
                    Id = jogo.Id,
                    Titulo = jogo.Titulo,
                    Categoria = jogo.Categoria,
                    Preco = jogo.Preco,
                    DataLancamento = jogo.DataLancamento,
                    DataAtualizacao = jogo.DataAtualizacao,
                    EstaEmPromocao = promocaoJogo != null,
                    PercentualDesconto = promocaoJogo?.Promocao.PercentualDesconto ?? 0,
                    ValorComDesconto = promocaoJogo?.ValorComDesconto ?? 0
                };
            });

            return response;
        }

        public async Task<JogoResponseDto?> ObterPorIdAsync(Guid id)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(id);

            if (jogo == null)
                return null;

            var promocaoJogo = (await _promocaoJogoRepository.ObterPorJogoIdAsync(id))
                .FirstOrDefault(pj => pj.Promocao.Status == StatusPromocao.ATIVA);

            return new JogoResponseDto
            {
                Id = jogo.Id,
                Titulo = jogo.Titulo,
                Categoria = jogo.Categoria,
                Preco = jogo.Preco,
                DataLancamento = jogo.DataLancamento,
                DataAtualizacao = jogo.DataAtualizacao,
                EstaEmPromocao = promocaoJogo != null,
                PercentualDesconto = promocaoJogo?.Promocao.PercentualDesconto ?? 0,
                ValorComDesconto = promocaoJogo?.ValorComDesconto ?? 0
            };
        }

        public async Task<JogoResponseDto?> ObterPorTituloAsync(string titulo)
        {
            var jogo = await _jogoRepository.ObterPorTituloAsync(titulo);
            if (jogo == null)
                return null;

            var promocaoJogo = (await _promocaoJogoRepository.ObterPorJogoIdAsync(jogo.Id))
                .FirstOrDefault(pj => pj.Promocao.Status == StatusPromocao.ATIVA);

            return new JogoResponseDto
            {
                Id = jogo.Id,
                Titulo = jogo.Titulo,
                Categoria = jogo.Categoria,
                Preco = jogo.Preco,
                DataLancamento = jogo.DataLancamento,
                DataAtualizacao = jogo.DataAtualizacao,
                EstaEmPromocao = promocaoJogo != null,
                PercentualDesconto = promocaoJogo?.Promocao.PercentualDesconto ?? 0,
                ValorComDesconto = promocaoJogo?.ValorComDesconto ?? 0
            };


        }

        public async Task<Jogo> AdicionarAsync(CriarJogoDto jogoDto)
        {
            // Verificar se já existe um jogo com esse título
            bool existe = await _jogoRepository.ExistePorTituloAsync(jogoDto.Titulo);
            if (existe)
            {
                throw new BusinessException("Já existe um jogo com esse título.");
            }

            var novoJogo = new Jogo
            {
                Id = Guid.NewGuid(),
                Titulo = jogoDto.Titulo,
                Categoria = jogoDto.Categoria,
                Preco = jogoDto.Preco,
                DataLancamento = jogoDto.DataLancamento,
                DataAtualizacao = DateTime.UtcNow
            };

            await _jogoRepository.AdicionarAsync(novoJogo);

            if (jogoDto.EnviarNotificacao)
            {
                var todosUsuarios = await _usuarioRepository.ObterTodosAsync();

                var assunto = $"O jogo {novoJogo.Titulo} chegou!";
                var mensagem = $"Aproveite! O jogo <b>{novoJogo.Titulo}</b> está em a venda por R$ {novoJogo.Preco:F2}.";

                foreach (var usuario in todosUsuarios)
                {
                    await _emailService.EnviarEmailAsync(usuario.Email, assunto, mensagem);
                }
            }

            return novoJogo;
        }

        public async Task AtualizarAsync(System.Guid id,string titulo,CategoriaJogo categoria, decimal preco , DateTime dataLancamento)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(id);
            if (jogo == null)
            {
                throw new NotFoundException("Jogo não encontrado.");
            }

            var existeTitulo = await _jogoRepository.ExistePorTituloAsync(jogo.Titulo, jogo.Id);
            if (existeTitulo)
                throw new BusinessException("Já existe outro jogo com este título.");

            var precoAlterado = jogo.Preco != preco;

            //se estiver alterando o preço , 
            if (precoAlterado)
            {
                //precisa ver se o jogo faz parte de alguma promoção
                var promocaoJogos = await _promocaoJogoRepository.ObterPorJogoIdAsync(jogo.Id);
                
                var promocaoAtiva = promocaoJogos
                    .FirstOrDefault(pj => pj.Promocao.Status == StatusPromocao.ATIVA);

                //se fizer parte de alguma , precisa atualizar os valores na PromocaoJogo 
                if (promocaoAtiva != null)
                {
                    // Atualizar o valor original e valor com desconto
                    promocaoAtiva.ValorOriginal = preco;

                    var percentualDesconto = promocaoAtiva.Promocao.PercentualDesconto;
                    promocaoAtiva.ValorComDesconto = Math.Round(preco - (preco * percentualDesconto / 100), 2);

                    await _promocaoJogoRepository.AtualizarAsync(promocaoAtiva);
                }

            }

            jogo.Titulo = titulo;
            jogo.Categoria = categoria;
            jogo.Preco = preco;
            jogo.DataLancamento = dataLancamento;
            jogo.DataAtualizacao = DateTime.UtcNow;

            
            await _jogoRepository.AtualizarAsync(jogo);
        }

        public async Task RemoverAsync(Guid id)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(id);
            if (jogo == null)
            {
                throw new NotFoundException("Jogo não encontrado.");
            }

            await _jogoRepository.RemoverAsync(id);
        }

    }
}
